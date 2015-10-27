﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.IO;
using System.Text;
using Sitecore.Pathfinder.CodeGeneration;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.IO;
using Sitecore.Pathfinder.Projects.Templates;

namespace Sitecore.Pathfinder.CodeGen
{
    public class PocoTemplate : ICodeGenerator
    {
        public bool CanGenerate(object instance)
        {
            return instance is Template;
        }

        public void Generate(string baseFileName, object instance)
        {
            var model = (Template)instance;

            var className = model.ShortName.GetSafeCodeIdentifier();

            var parts = PathHelper.GetItemParentPath(model.QualifiedName).Split(Constants.Slash, StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < parts.Length; index++)
            {
                parts[index] = parts[index].GetSafeCodeIdentifier().Capitalize();
            }

            var safeNamespace = string.Join(".", parts);

            using (var output = new StreamWriter(baseFileName + ".g.cs", false, Encoding.UTF8))
            {
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
                        output.WriteLine("        public string " + field.FieldName.GetSafeCodeIdentifier() + " { get { return InnerItem[\"" + field.FieldName + "\"]; } }");
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
}
