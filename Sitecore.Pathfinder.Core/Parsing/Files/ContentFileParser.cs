﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.ComponentModel.Composition;
using System.IO;
using Sitecore.Pathfinder.Extensions;

namespace Sitecore.Pathfinder.Parsing.Files
{
    [Export(typeof(IParser))]
    public class ContentFileParser : ParserBase
    {
        public ContentFileParser() : base(Constants.Parsers.ContentFiles)
        {
        }

        public override bool CanParse(IParseContext context)
        {
            var fileExtensions = " " + context.Configuration.GetString(Constants.Configuration.ContentFiles) + " ";
            var extension = " " + Path.GetExtension(context.Snapshot.SourceFile.FileName) + " ";

            return fileExtensions.IndexOf(extension, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public override void Parse(IParseContext context)
        {
            var contentFile = context.Factory.ContentFile(context.Project, context.Snapshot);
            context.Project.AddOrMerge(contentFile);
        }
    }
}
