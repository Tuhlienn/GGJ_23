using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hex
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap[] obstacleMaps;
        [SerializeField] private Tilemap goalMap;

        private readonly List<HexVector> _obstaclePositions = new();
        private readonly Dictionary<HexVector, IGridPlaceable> _nodes = new();
        private readonly List<HexVector> _goalPositions = new();

        private void Start()
        {
            foreach (Tilemap map in obstacleMaps)
            {
                _obstaclePositions.AddRange(GetFilledHexPositions(map));
            }

            _goalPositions.AddRange(GetFilledHexPositions(goalMap));
        }

        private static IEnumerable<HexVector> GetFilledHexPositions(Tilemap map)
        {
            var result = new List<HexVector>();
            foreach (Vector3Int position in map.cellBounds.allPositionsWithin)
            {
                if (!map.HasTile(position))
                    continue;

                result.Add(GetHexPositionFromGridPosition(position));
            }

            return result;
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

        private IGridPlaceable this[HexVector position] => _nodes.TryGetValue(position, out IGridPlaceable node) ? node : default;
        public bool HasNodeAtPosition(HexVector position) => this[position] != null;

        public bool HasObstacleAtPosition(HexVector position) => _obstaclePositions.Contains(position) || HasNodeAtPosition(position);
        public bool HasGoalAtPosition(HexVector position) => _goalPositions.Contains(position);
        public bool AllGoalsReached(IEnumerable<HexVector> positions) => _goalPositions.All(positions.Contains);

        public void AddNodeAtPosition(IGridPlaceable node, HexVector position)
        {
            _nodes.TryAdd(position, node);
        }

        public void Clear()
        {
            _nodes.Clear();
        }
    }
}
