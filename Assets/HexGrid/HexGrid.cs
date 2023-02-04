using System.Collections.Generic;
using UnityEngine;

namespace HexGrid
{
    public class HexGrid : MonoBehaviour
    {
        private readonly Dictionary<HexVector, IGridPlaceable> _nodes = new();

        public IGridPlaceable this[HexVector position] => _nodes.TryGetValue(position, out IGridPlaceable node) ? node : default;

        public bool HasNodeAtPosition(HexVector position) => this[position] != null;

        public void AddNodeAtPosition(IGridPlaceable node, HexVector position)
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
