﻿// © 2015-2017 Sitecore Corporation A/S. All rights reserved.

using Sitecore.Pathfinder.Diagnostics;

namespace Sitecore.Pathfinder.ProjectTrees
{
    public class ProjectTreeUri
    {
        public ProjectTreeUri([NotNull] string uri)
        {
            Uri = uri;
        }

        [NotNull]
        public string Uri { get; }

        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ProjectTreeUri)obj);
        }

        public override int GetHashCode() => Uri.GetHashCode();

        protected bool Equals([NotNull] ProjectTreeUri other) => string.Equals(Uri, other.Uri);
    }
}
