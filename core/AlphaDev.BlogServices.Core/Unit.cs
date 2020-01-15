using System;

namespace AlphaDev.BlogServices.Core
{
    public class Unit
    {
        private static readonly Lazy<Unit> UnitValue;

        static Unit()
        {
            UnitValue = new Lazy<Unit>(() => new Unit());
        }

        private Unit()
        {
        }

        public static Unit Value => UnitValue.Value;
    }
}