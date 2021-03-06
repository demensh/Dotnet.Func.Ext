﻿namespace Dotnet.Func.Ext.Algebraic
{
    using System;
    using System.Collections.Generic;
    using static Data.Eithers;
    using static Data.Optionals;
    using static Data.Orders;
    using static Data.Tuples;
    using static Data.Units;

    public static class Signatures
    {
        public interface IEither<out left, out right>
        {
            res Case<leftCtx, rightCtx, res>(leftCtx LeftCtx, Func<leftCtx, left, res> Left, rightCtx RightCtx, Func<rightCtx, right, res> Right);
        }

        // (left + right) -> ((left -> res) * (right -> res)) -> res
        public interface SSumProj<type, left, right>
        {
            res Project<res>(type Value, Pair<Func<left, res>, Func<right, res>> Projector);
        }
        
        // arg -> ((arg -> left) + (arg -> right)) -> (left + right)
        public interface SSumInj<type, left, right>
        {
            type Inject<arg>(arg Arg, Either<Func<arg, left>, Func<arg, right>> Injector);
        }

        // (left * right) -> ((left -> res) + (right -> res)) -> res
        public interface SProdProj<type, left, right>
        {
            res Project<res>(type Value, Either<Func<left, res>, Func<right, res>> Projector);
        }

        // arg -> ((arg -> left) * (arg -> right)) -> (left * right)
        public interface SProdInj<type, left, right>
        {
            type Inject<arg>(arg Arg, Pair<Func<arg, left>, Func<arg, right>> Injector);
        }

        // stored -> container stored
        public interface SUnitInj<container, stored>
        {
            container Inj(stored Value);
        }

        // container stored -> stored
        public interface SUnitProj<container, stored>
        {
            stored Proj(container Value);
        }

        public interface REquality<type, mark> : SBinOp<type, type, bool, Equative<mark>>
        {
        }
        
        public interface ROrder<type, mark> : REquality<type, mark>, SLattice<type, mark>
        {
        }

        public interface SNullOp<type, mark>
        {
            type NullOp();
        }

        public interface SUnOp<type, res, mark>
        {
            res UnOp(type t);
        }

        public interface SBinOp<left, right, res, mark>
        {
            res BinOp(left l, right r);
        }

        public interface SNeutral<type, mark> : SNullOp<type, mark> { }

        public interface SInv<type, mark> : SUnOp<type, type, mark> { }

        public interface SSemigroup<type, mark> : SBinOp<type, type, type, mark> { }
        
        public interface SLattice<type, mark> : SSemigroup<type, Infimum<mark>>, SSemigroup<type, Supremum<mark>> { }

        public interface SBounded<type, mark> : SLattice<type, mark>, SNeutral<type, Infimum<mark>>, SNeutral<type, Supremum<mark>> { }

        public interface SMonoid<type, mark> : SSemigroup<type, mark>, SNeutral<type, mark> { }
        
        public interface SGroup<type, mark> : SMonoid<type, mark>, SInv<type, mark> { }
        
        public interface SRing<type, mark> : SGroup<type, Additive<mark>>, SMonoid<type, Multiplicative<mark>> { }

        public interface SField<type, mark> : SRing<type, mark>, SGroup<type, Multiplicative<mark>> { }

        public interface SEnum<type>
        {
            Opt<type> Succ(type v);
            Opt<type> Pred(type v);
        }

        public interface SRangeable<type>
        {
            IEnumerable<type> EnumFrom(type from);
            IEnumerable<type> EnumFromThen(type from, type then);
            IEnumerable<type> EnumFromTo(type from, type to);
            IEnumerable<type> EnumFromThenTo(type from, type then, type to);
        }

        public interface SList<type, element> : SSumInj<type, Unit, Pair<element, type>>, SSumProj<type, Unit, Pair<element, type>> { }

        public interface SHashable<type>
        {
            int GetHashCode(type t);
        }
        
    }
}
