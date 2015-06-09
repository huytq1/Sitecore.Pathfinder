﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Documents;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.Projects;

namespace Sitecore.Pathfinder.Parsing
{
    [Export(typeof(IParseService))]
    public class ParseService : IParseService
    {
        [ImportingConstructor]
        public ParseService([NotNull] ICompositionService compositionService, [NotNull] ISnapshotService snapshotService)
        {
            CompositionService = compositionService;
            SnapshotService = snapshotService;
        }

        [NotNull]
        [ImportMany]
        public IEnumerable<IParser> Parsers { get; private set; }

        [NotNull]
        protected ICompositionService CompositionService { get; }

        [NotNull]
        protected ISnapshotService SnapshotService { get; }

        public virtual void Parse(IProject project, ISourceFile sourceFile)
        {
            var textDocument = SnapshotService.LoadSnapshot(project, sourceFile);

            var parseContext = CompositionService.Resolve<IParseContext>().With(project, textDocument);

            try
            {
                foreach (var parser in Parsers.OrderBy(c => c.Sortorder))
                {
                    if (parser.CanParse(parseContext))
                    {
                        parser.Parse(parseContext);
                    }
                }
            }
            catch (Exception ex)
            {
                parseContext.Trace.TraceError(string.Empty, sourceFile.FileName, TextPosition.Empty, ex.Message);
            }
        }
    }
}
