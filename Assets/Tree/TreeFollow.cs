using System.Collections.Generic;
using System.Linq;
using Tree;
using UnityEngine;

public class TreeFollow : MonoBehaviour
{
    [SerializeField] private TreeGrowthManager treeGrowthManager;
    [SerializeField] private float minYPosition;
    private float _highestY;

    private void Start()
    {
        treeGrowthManager.OnNodesUpdate += OnNodesAdded;
        treeGrowthManager.OnNodesReset += OnNodesReset;
        _highestY = minYPosition;
        UpdatePosition();
    }

    private void OnNodesReset()
    {
        _highestY = minYPosition;
        UpdatePosition();
    }

    private void OnNodesAdded(List<(BranchNode current, BranchNode next)> added)
    {
        Vector3 highest = added.SelectMany(entry => new List<Vector3>
            {
                entry.current?.Position.ToWorldPosition() ?? Vector2.zero,
                entry.next?.Position.ToWorldPosition() ?? Vector2.zero
            })
            .OrderBy(position => position.y)
            .LastOrDefault();

        if (highest.y <= _highestY || highest.y < minYPosition)
            return;

        _highestY = highest.y;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = _highestY;
        transform.position = newPosition;
    }
}
