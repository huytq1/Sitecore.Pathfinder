﻿// © 2015-2017 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Composition;
using Sitecore.Pathfinder.Configuration;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Parsing;

namespace Sitecore.Pathfinder.Languages.Serialization
{
    [Export(typeof(IParser)), Shared]
    public class SerializationFileParser : ParserBase
    {
        private const string FileExtension = ".item";

        [ImportingConstructor]
        public SerializationFileParser([NotNull] IFactory factory) : base(Constants.Parsers.Items)
        {
            Factory = factory;
        }

        [NotNull]
        protected IFactory Factory { get; }

        public override bool CanParse(IParseContext context) => context.Snapshot.SourceFile.AbsoluteFileName.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);

        public override void Parse(IParseContext context)
        {
            var serializationFile = Factory.SerializationFile(context.Project, context.Snapshot, context.FilePath);
            context.Project.AddOrMerge(serializationFile);
        }
    }
}
