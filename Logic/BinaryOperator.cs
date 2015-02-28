// somerby.net/mack/logic
// Copyright (C) 2015 MacKenzie Cumings
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
  /// <summary>
  /// a binary operator that represents a function of the truth values of two matrices
  /// </summary>
  internal abstract class BinaryOperator : Matrix
	{
	  private readonly Matrix mLeft;
    private readonly Matrix mRight;

    internal BinaryOperator( Matrix aLeft, Matrix aRight )
    {
      this.mLeft = aLeft;
      this.mRight = aRight;
    }

    protected Matrix Left
    {
      get { return mLeft; }
    }

    protected Matrix Right
    {
      get { return mRight; }
    }

    internal override IEnumerable<Tuple<UniversalGeneralization, Matrix>> ClosedPredications
    {
      get { return Left.ClosedPredications.Union( Right.ClosedPredications ); }
    }
    
    protected abstract string Connector
    {
      get;
    }

    internal override IEnumerable<Tuple<Matrix, Matrix>> DirectDependencies
    {
      get
      {
        yield return Tuple.Create( this as Matrix, Left );
        yield return Tuple.Create( this as Matrix, Right );

        foreach ( Tuple<Matrix, Matrix> lPair in Left.DirectDependencies )
        {
          yield return lPair;
        }

        foreach ( Tuple<Matrix, Matrix> lPair in Right.DirectDependencies )
        {
          yield return lPair;
        }
      }
    }

    internal override IEnumerable<Variable> FreeVariables
    {
      get { return Left.FreeVariables.Union( Right.FreeVariables ); }
    }

    internal override IEnumerable<Necessity> FreeModalities
    {
      get { return Left.FreeModalities.Union( Right.FreeModalities ); }
    }

    internal override IEnumerable<Variable> IdentifiedVariables
    {
      get { return Left.IdentifiedVariables.Union( Right.IdentifiedVariables ); }
    }

    internal override IEnumerable<Necessity> ModalitiesInIdentifications
    {
      get { return Left.ModalitiesInIdentifications.Union( Right.ModalitiesInIdentifications ); }
    }

    internal override bool ContainsModalities
    {
      get { return Left.ContainsModalities || Right.ContainsModalities; }
    }

    public override bool IsPropositional
    {
      get { return mLeft.IsPropositional && mRight.IsPropositional; }
    }

    internal override IEnumerable<Matrix> Matrices
    {
      get
      {
        yield return this;

        foreach ( Matrix lMatrix in Left.Matrices )
        {
          yield return lMatrix;
        }

        foreach ( Matrix lMatrix in Right.Matrices )
        {
          yield return lMatrix;
        }
      }
    }

    internal override int MaxmimumNumberOfDistinguishableObjects
    {
      get
      {
        return Math.Max(
          Left.MaxmimumNumberOfDistinguishableObjects,
          Right.MaxmimumNumberOfDistinguishableObjects );
      }
    }

    internal override int MaxmimumNumberOfModalitiesInIdentifications
    {
      get
      {
        return Math.Max(
          Left.MaxmimumNumberOfModalitiesInIdentifications,
          Right.MaxmimumNumberOfModalitiesInIdentifications );
      }
    }

    internal override IEnumerable<Matrix> NonNullPredications
    {
      get { return Left.NonNullPredications.Union( Right.NonNullPredications ); }
    }

    internal override IEnumerable<NullPredicate> NullPredicates
    {
      get { return Left.NullPredicates.Union( Right.NullPredicates ); }
    }

    internal override IEnumerable<UnaryPredicate> UnaryPredicates
    {
      get { return Left.UnaryPredicates.Union( Right.UnaryPredicates ); }
    }

    internal override void AssignModality( Necessity aNecessity )
    {
      Left.AssignModality( aNecessity );
      Right.AssignModality( aNecessity );
    }

    public override string ToString()
    {
      return string.Format( "({0}{1}{2})", Left, Connector, Right );
    }

    public override bool Equals( object obj )
    {
      BinaryOperator that = obj as BinaryOperator;

      return this.Left.Equals( that.Left ) && this.Right.Equals( that.Right );
    }

    public override int GetHashCode()
    {
      return Left.GetHashCode() ^ Right.GetHashCode();
    }
	}
}

