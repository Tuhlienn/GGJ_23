using System;
using Unity.Mathematics;
using UnityEngine;

namespace Hex
{
    public readonly struct HexVector
    {
        private const float UnitSize = 0.5f;

        private readonly int _q;
        private readonly int _r;
        private readonly int _s;

        public HexVector(int q, int r)
        {
            _q = q;
            _r = r;
            _s = -(q + r);
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

        public bool IsBelowGround => ToWorldPosition().y <= 0;
        public bool IsOnLeftScreenHalf => ToWorldPosition().x <= 0;

        public HexVector RotateLeft()
        {
            return new HexVector(-_s, -_q);
        }

        public HexVector RotateRight()
        {
            return new HexVector(-_r, -_s);
        }

        public Vector2 ToWorldPosition()
        {
            float x = UnitSize * (_r * 3.0f / 2.0f);
            float y = UnitSize * (_q * math.sqrt(3) + _r * math.sqrt(3) / 2f);
            return new Vector2(x, y);
        }

        public override string ToString()
        {
            return $"HexVector({_q},{_r},{_s})";
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
