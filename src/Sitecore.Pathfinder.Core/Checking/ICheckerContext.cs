﻿// © 2015-2017 Sitecore Corporation A/S. All rights reserved.

using System.Collections.Generic;
using System.Globalization;
using Sitecore.Pathfinder.Configuration;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.IO;
using Sitecore.Pathfinder.Projects;

namespace Sitecore.Pathfinder.Checking
{
    public interface ICheckerContext
    {
        [NotNull]
        IDictionary<string, CheckerSeverity> Checkers { get; }

        [NotNull]
        CultureInfo Culture { get; }

        [NotNull]
        IFactory Factory { get; }

        [NotNull]
        IFileSystem FileSystem { get; }

        [NotNull]
        IProjectBase Project { get; }

        [NotNull]
        ICheckerContext With([NotNull] IProjectBase project);
    }
}
