// � 2015-2017 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Sitecore.Pathfinder.Configuration;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Parsing.Items;
using Sitecore.Pathfinder.Parsing.References;
using Sitecore.Pathfinder.Projects;
using Sitecore.Pathfinder.Projects.References;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Languages.Json
{
    [Export(typeof(ITextNodeParser)), Shared]
    public class JsonLayoutTextNodeParser : LayoutTextNodeParserBase
    {
        [ImportingConstructor]
        public JsonLayoutTextNodeParser([NotNull] IFactory factory, [NotNull] IReferenceParserService referenceParserService) : base(factory, referenceParserService, Constants.TextNodeParsers.Layouts)
        {
        }

        public override bool CanParse(ItemParseContext context, ITextNode textNode) => textNode.Key == "Layout" && textNode.Snapshot is JsonTextSnapshot && textNode.Snapshot.SourceFile.AbsoluteFileName.EndsWith(".layout.json", StringComparison.OrdinalIgnoreCase);

        protected override void ParseRenderingReferences(ItemParseContext context, ICollection<IReference> references, IProjectItem projectItem, ITextNode renderingTextNode)
        {
            var childNode = renderingTextNode.ChildNodes.FirstOrDefault();
            if (childNode != null)
            {
                base.ParseRenderingReferences(context, references, projectItem, childNode);
            }
        }
    }
}
