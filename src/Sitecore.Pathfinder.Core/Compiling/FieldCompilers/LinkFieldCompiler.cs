﻿using System;
using System.Composition;
using System.Linq;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.Projects;
using Sitecore.Pathfinder.Projects.Items;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Compiling.FieldCompilers
{
    [Export(typeof(IFieldCompiler)), Shared]
    public class LinkFieldCompiler : FieldCompilerBase
    {
        [ImportingConstructor]
        public LinkFieldCompiler([NotNull] ITraceService trace) : base(Constants.FieldCompilers.Normal)
        {
            Trace = trace;
        }

        public override bool IsExclusive => true;

        [NotNull]
        protected ITraceService Trace { get; }

        public override bool CanCompile(IFieldCompileContext context, Field field)
        {
            var type = field.TemplateField.Type;
            return string.Equals(type, "general link", StringComparison.OrdinalIgnoreCase) || string.Equals(type, "link", StringComparison.OrdinalIgnoreCase);
        }

        public override string Compile(IFieldCompileContext context, Field field)
        {
            var qualifiedName = field.Value.Trim();
            if (string.IsNullOrEmpty(qualifiedName))
            {
                return string.Empty;
            }

            if (qualifiedName == "<link />" || qualifiedName == "<link/>")
            {
                return string.Empty;
            }

            var item = field.Item.Project.Indexes.FindQualifiedItem<IProjectItem>(qualifiedName);

            if (item == null)
            {
                item = field.Item.Project.Files.FirstOrDefault(f => string.Equals(f.FilePath, qualifiedName, StringComparison.OrdinalIgnoreCase));
            }

            if (item == null)
            {
                Trace.TraceError(Msg.C1049, Texts.Link_field_reference_not_found, TraceHelper.GetTextNode(field.ValueProperty, field.FieldNameProperty), qualifiedName);
                return string.Empty;
            }

            return $"<link text=\"\" linktype=\"internal\" url=\"\" anchor=\"\" title=\"\" class=\"\" target=\"\" querystring=\"\" id=\"{item.Uri.Guid.Format()}\" />";
        }
    }
}
