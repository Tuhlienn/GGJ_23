using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HexGrid
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap levelMap;

        private readonly List<HexVector> _obstaclePositions = new();
        private readonly Dictionary<HexVector, IGridPlaceable> _nodes = new();

        private void Start()
        {
            foreach (Vector3Int position in levelMap.cellBounds.allPositionsWithin)
            {
                if (!levelMap.HasTile(position))
                    continue;

                HexVector hexPosition = GetHexPositionFromGridPosition(position);
                _obstaclePositions.Add(hexPosition);
            }
        }

        // row = -position.x
        // col = position.y
        private static HexVector GetHexPositionFromGridPosition(Vector3Int position)
        {
            int r = position.y;
            int s = -position.x - (r + (r & 1)) / 2;
            int q = -(r + s);
            var hexPosition = new HexVector(q, r);
            return hexPosition;
        }

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
            foreach (HexVector position in _obstaclePositions)
            {
                AddNodeAtPosition(new ObstacleNode(), position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var obstacle in _obstaclePositions)
            {
                Gizmos.DrawSphere(obstacle.ToWorldPosition(), 0.2f);
            }
        }
    }
}
