﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.ComponentModel.Composition;
using System.IO;
using Sitecore.Pathfinder.CodeGeneration;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.IO;
using Sitecore.Pathfinder.Projects.Templates;

namespace Sitecore.Pathfinder.CodeGen
{
    [Export(typeof(ICodeGenerator))]
    public class PocoTemplate : ICodeGenerator
    {
        public bool CanGenerate(object instance)
        {
            return instance is Template;
        }

        public void Generate(TextWriter output, string fileName, object instance)
        {
            var model = (Template)instance;

            var className = model.ShortName.GetSafeCodeIdentifier();

            var parts = PathHelper.GetItemParentPath(model.QualifiedName).Split(Constants.Slash, StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < parts.Length; index++)
            {
                parts[index] = parts[index].GetSafeCodeIdentifier().Capitalize();
            }

            var safeNamespace = string.Join(".", parts);

            output.WriteLine(@"//------------------------------------------------------------------------------");
            output.WriteLine(@"// <auto-generated>");
            output.WriteLine(@"// This code was generated by a tool.");
            output.WriteLine(@"//");
            output.WriteLine(@"// Changes to this file may cause incorrect behavior and will be lost if");
            output.WriteLine(@"// the code is regenerated.");
            output.WriteLine(@"//");
            output.WriteLine(@"// </auto-generated>");
            output.WriteLine(@"//------------------------------------------------------------------------------");
            output.WriteLine();
            output.WriteLine(@"#pragma warning disable 1591");
            output.WriteLine();
            output.WriteLine(@"using Sitecore;");
            output.WriteLine(@"using Sitecore.Data.Items;");
            output.WriteLine();
            output.WriteLine($"namespace {safeNamespace}");
            output.WriteLine(@"{");
            output.WriteLine(@"    #region Designer generated code");
            output.WriteLine(@"");
            output.WriteLine(@"    [System.Diagnostics.DebuggerStepThrough()]");
            output.WriteLine(@"    [System.CodeDom.Compiler.GeneratedCode(""Sitecore.Pathfinder"", ""1.0.0.0"")]");
            output.WriteLine($"    public partial class {className}");
            output.WriteLine(@"    {");
            output.WriteLine($"        public {className}(Item innerItem)");
            output.WriteLine(@"        {");
            output.WriteLine(@"          InnerItem = innerItem;");
            output.WriteLine(@"        }");
            output.WriteLine();
            output.WriteLine(@"        [NotNull]");
            output.WriteLine(@"        [System.CodeDom.Compiler.GeneratedCodeAttribute(""Sitecore.Pathfinder"", ""1.0.0.0"")]");
            output.WriteLine(@"        public Item InnerItem { get; private set; }");
            output.WriteLine();

            foreach (var section in model.Sections)
            {
                foreach (var field in section.Fields)
                {
                    output.WriteLine("        [NotNull]");
                    output.WriteLine("        [System.CodeDom.Compiler.GeneratedCode(\"Sitecore.Pathfinder\", \"1.0.0.0\")]");
                    output.WriteLine("        public string " + field.FieldName.Value.GetSafeCodeIdentifier() + " { get { return InnerItem[\"" + field.FieldName.Value + "\"]; } }");
                    output.WriteLine();
                }
            }

            output.WriteLine(@"    }");
            output.WriteLine(@"");
            output.WriteLine(@"    #endregion");
            output.WriteLine(@"}");
            output.WriteLine(@"");
            output.WriteLine(@"#pragma warning restore 1591");
        }
    }
}