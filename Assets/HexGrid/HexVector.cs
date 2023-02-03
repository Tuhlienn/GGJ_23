using System;
using UnityEngine;

namespace HexGrid
{
    public class HexVector
    {
        private const int UnitSize = 1;
        private const float HorizontalOffset = 1.5f;
        private const float VerticalOffset = 0.866f;

        private readonly int _q;
        private readonly int _r;

        public HexVector(int q, int r)
        {
            _q = q;
            _r = r;
        }

        public static HexVector Zero => new(0, 0);
        public static HexVector Up => new(1, 0);
        public static HexVector Down => new(-1, 0);
        public static HexVector UpRight => new(0, 1);
        public static HexVector DownLeft => new(0, -1);
        public static HexVector UpLeft => new(1, -1);
        public static HexVector DownRight => new(-1, 1);

        public HexVector Upper => this + Up;
        public HexVector Lower => this + Down;
        public HexVector UpperRight => this + UpRight;
        public HexVector LowerLeft => this + DownLeft;
        public HexVector UpperLeft => this + UpLeft;
        public HexVector LowerRight => this + DownRight;

        public HexVector RotateLeft()
        {
            return new HexVector(_q + _r, -_q);
        }

        public HexVector RotateRight()
        {
            return new HexVector(-_r, _q + _r);
        }

        public Vector2 ToPosition()
        {
            float x = _r * HorizontalOffset * UnitSize;
            float y = (_q + _r * VerticalOffset) * UnitSize;
            return new Vector2(x, y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals ((HexVector) obj);
        }

        private bool Equals(HexVector other)
        {
            return _q == other._q && _r == other._r;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_q, _r);
        }

        public static bool operator ==(HexVector left, HexVector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HexVector left, HexVector right)
        {
            return !Equals(left, right);
        }

        public static HexVector operator +(HexVector left, HexVector right)
        {
            return new HexVector(left._q + right._q, left._r + right._r);
        }

        public static HexVector operator -(HexVector left, HexVector right)
        {
            return new HexVector(left._q - right._q, left._r - right._r);
        }
    }
}
