﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.IO;

namespace Sitecore.Pathfinder.Projects
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProjectDirectoryVisitor
    {
        [ImportingConstructor]
        public ProjectDirectoryVisitor([NotNull] IFileSystemService fileSystem)
        {
            FileSystem = fileSystem;

            IgnoreDirectories = Enumerable.Empty<string>();
            IgnoreFileNames = Enumerable.Empty<string>();
        }

        [NotNull]
        protected IFileSystemService FileSystem { get; }

        [NotNull]
        protected IEnumerable<string> IgnoreDirectories { get; set; }

        [NotNull]
        protected IEnumerable<string> IgnoreFileNames { get; set; }

        public virtual void Visit([NotNull] ProjectOptions projectOptions, [NotNull] ICollection<string> sourceFileNames)
        {
            if (!FileSystem.DirectoryExists(projectOptions.ProjectDirectory))
            {
                return;
            }

            Visit(projectOptions, sourceFileNames, projectOptions.ProjectDirectory);
        }

        [NotNull]
        public ProjectDirectoryVisitor With([NotNull] IEnumerable<string> ignoreDirectories, [NotNull] IEnumerable<string> ignoreFileNames)
        {
            IgnoreDirectories = ignoreDirectories;
            IgnoreFileNames = ignoreFileNames;
            return this;
        }

        protected virtual bool IgnoreDirectory([NotNull] string directory)
        {
            var directoryName = Path.GetFileName(directory);

            return IgnoreDirectories.Contains(directoryName, StringComparer.OrdinalIgnoreCase);
        }

        protected virtual bool IgnoreFileName([NotNull] string fileName)
        {
            var name = Path.GetFileName(fileName);

            return IgnoreFileNames.Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        protected virtual void Visit([NotNull] ProjectOptions projectOptions, [NotNull] ICollection<string> sourceFileNames, [NotNull] string directory)
        {
            var fileNames = FileSystem.GetFiles(directory);
            foreach (var fileName in fileNames)
            {
                if (!IgnoreFileName(fileName))
                {
                    sourceFileNames.Add(fileName);
                }
            }

            var subdirectories = FileSystem.GetDirectories(directory);
            foreach (var subdirectory in subdirectories)
            {
                if (!IgnoreDirectory(subdirectory))
                {
                    Visit(projectOptions, sourceFileNames, subdirectory);
                }
            }
        }
    }
}
