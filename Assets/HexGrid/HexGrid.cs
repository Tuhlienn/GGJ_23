using System.Collections.Generic;

namespace HexGrid
{
    public class HexGrid<T>
    {
        private readonly Dictionary<HexVector, T> _nodes = new();

        public T this[HexVector position] => _nodes.TryGetValue(position, out T node) ? node : default;

        public bool HasNodeAtPosition(HexVector position) => this[position] != null;

        public void AddNodeAtPosition(T node, HexVector position)
        {
            if (this[position] == null)
                _nodes[position] = node;
        }

        public void Clear()
        {
            _nodes.Clear();
        }
    }
}
