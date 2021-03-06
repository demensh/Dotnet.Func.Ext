﻿namespace Dotnet.Func.Ext.Algebraic
{
    using Data;
    using System;
    using System.Collections.Generic;
    using static Data.Units;
    using static Signatures;

    public static class Comparers
    {
        public static IEqualityComparer<val> Eq<val>(Func<val, val, bool> pred, Func<val, int> hash) =>
            new AEqComparer<val>(pred, hash);

        public static IEqualityComparer<val> Eq<val, alg>(alg algebra) where alg : REquality<val, Unit>, SHashable<val> =>
            new AEqComparer<val>(algebra.Equal, algebra.GetHashCode);

        public static IComparer<val> Cmp<val>(Func<val, val, int> comparer) =>
            new AOrdComparer<val>((x, y) => comparer(x, y).ToOrd());

        public static IComparer<val> Cmp<val>(ROrder<val, Unit> order) =>
            new AOrdComparer<val>(order.Compare);

        public static IEqualityComparer<val> Map<val, key>(this IEqualityComparer<key> that, Func<val, key> f) =>
            Eq<val>((x, y) => that.Equals(f(x), f(y)), v => that.GetHashCode(f(v)));
    }
}
