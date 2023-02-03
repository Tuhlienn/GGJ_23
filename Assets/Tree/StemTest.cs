using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tree
{
    public class StemTest : MonoBehaviour
    {
        [SerializeField] private float tickTime = 1;

        private float _timer;
        private Stem _stem;

        private void Start()
        {
            _stem = new Stem();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < tickTime)
                return;

            _stem.Tick();
            _timer -= tickTime;
        }

        private void OnDrawGizmos()
        {
            if (_stem?.Branches == null || !_stem.Branches.Any())
                return;

            Handles.color = Color.green;

            foreach (StemBranch branch in _stem.Branches)
            {
                Vector3[] points = branch.Path
                    .Select(node => node.Position.ToWorldPosition())
                    .Select(pos => new Vector3(pos.x, pos.y))
                    .ToArray();
                Handles.DrawPolyLine(points);
            }
        }
    }
}
