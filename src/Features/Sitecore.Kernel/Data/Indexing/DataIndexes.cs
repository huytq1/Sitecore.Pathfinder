// � 2015 Sitecore Corporation A/S. All rights reserved.

using System;

namespace Sitecore.Data.Indexing
{
    public class DataIndexes
    {
        [NotNull]
        public Index this[int index]
        {
            get { throw new NotImplementedException(); }
        }

        [NotNull]
        public Index this[[NotNull] string name]
        {
            get { throw new NotImplementedException(); }
        }

        public int Count { get; private set; }
    }
}