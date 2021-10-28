#nullable enable

using System;

namespace Dev
{
    public sealed class OptimizedAttribute : Attribute
    {
        public readonly double MinMilliseconds;
        public readonly double MaxMilliseconds;

        public OptimizedAttribute(double minMilliseconds, double maxMilliseconds)
        {
            MinMilliseconds = minMilliseconds;
            MaxMilliseconds = maxMilliseconds;
        }

        public OptimizedAttribute(double milliseconds) : this(milliseconds, milliseconds) { }
    }
}
