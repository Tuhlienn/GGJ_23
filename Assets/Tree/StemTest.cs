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

        // private void OnDrawGizmos()
        // {
        //     if (!manager || manager.Branches == null || !manager.Branches.Any())
        //         return;
        //
        //     Gizmos.color = Color.green;
        //
        //     foreach (TreeBranch branch in manager.Branches)
        //     {
        //         Vector3[] points = branch.Path
        //             .Select(node => node.Position.ToWorldPosition())
        //             .Select(pos => new Vector3(pos.x, pos.y))
        //             .ToArray();
        //
        //         Gizmos.DrawLineStrip(points, false);
        //     }
        // }
    }
}
