using System.Collections.Generic;

namespace HexGrid
{
    public class HexGrid<T>
    {
        private readonly Dictionary<HexVector, T> _nodes = new();
        public bool HasNodeAtPosition(HexVector position) => _nodes[position] != null;
        public void AddNodeAtPosition(T node, HexVector position) => _nodes[position] = node;
    }
}
