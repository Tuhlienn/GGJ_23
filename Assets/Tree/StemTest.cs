using System.Linq;
using UnityEngine;

namespace Tree
{
    public class StemTest : MonoBehaviour
    {
        [SerializeField] private TreeGrowthManager manager;

        private void Start()
        {
            manager.StartNewTree();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                manager.StartNewTree();
                manager.SetRunning(true);
            }
        }

        private void OnDrawGizmos()
        {
            if (!manager || manager.InstructionMovers == null || !manager.InstructionMovers.Any())
                return;

            Gizmos.color = Color.green;

            foreach (IInstructionMover instructionMover in manager.InstructionMovers)
            {
                var branch = (TreeBranch)instructionMover;
                Vector3[] points = branch.Path
                    .Select(node => node.Position.ToWorldPosition())
                    .Select(pos => new Vector3(pos.x, pos.y))
                    .ToArray();

                Gizmos.DrawLineStrip(points, false);
            }
        }
    }
}
