// � 2015-2017 Sitecore Corporation A/S. All rights reserved.

using System.ComponentModel.Composition;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Emitting.FileAndSqlEmitting;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.Tasks.Building;

namespace Sitecore.Pathfinder.Tasks
{
    public class CopyProject : BuildTaskBase
    {
        [ImportingConstructor]
        public CopyProject([NotNull] ICompositionService compositionService) : base("copy-project")
        {
            CompositionService = compositionService;
        }

        [NotNull]
        public ICompositionService CompositionService { get; }

        public override void Run(IBuildContext context)
        {
            context.Trace.TraceInformation(Msg.D1027, "Copying project to website...");

            var projectEmitter = CompositionService.Resolve<FileAndSqlProjectEmitter>();

            projectEmitter.Emit(context.Project);
        }
    }
}
