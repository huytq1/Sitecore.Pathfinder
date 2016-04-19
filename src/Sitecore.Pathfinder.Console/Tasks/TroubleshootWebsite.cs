// � 2015 Sitecore Corporation A/S. All rights reserved.

using Sitecore.Pathfinder.Tasks.Building;

namespace Sitecore.Pathfinder.Tasks
{
    public class TroubleshootWebsite : RequestBuildTaskBase
    {
        public TroubleshootWebsite() : base("troubleshoot-website")
        {
        }

        public override void Run(IBuildContext context)
        {
            context.Trace.TraceInformation(Msg.G1010, Texts.Troubleshooting___);

            var url = MakeWebsiteTaskUrl(context, "TroubleshootWebsite");

            Post(context, url);
        }

        public override void WriteHelp(HelpWriter helpWriter)
        {
            helpWriter.Summary.Write("Tries to fix a non-working website.");
            helpWriter.Remarks.Write("Republishing the Master database, rebuilds search indexes and rebuild the Link database.");
        }
    }
}
