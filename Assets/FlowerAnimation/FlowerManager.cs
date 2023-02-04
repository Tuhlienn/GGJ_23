using System.Collections.Generic;
using UnityEngine;
using Tree;
using HexGrid;

public class FlowerManager : MonoBehaviour
{
    [SerializeField] private FlowerAnimator flowerPrefab;
    private TreeGrowthManager treeManager;
    private Dictionary<HexVector, FlowerAnimator> flowers = new();

    void Awake()
    {
        treeManager = FindObjectOfType<TreeGrowthManager>();
        treeManager.OnBranchEnded += TreeGrowthManager_OnBranchEnded;
    }

    void TreeGrowthManager_OnBranchEnded(TreeBranch branch)
    {
        HexVector girdPos = branch.Position;

        if(this.flowers.ContainsKey(girdPos))
            return;

        Vector3 worldPos = girdPos.ToWorldPosition();
        worldPos.z = -1;

        var flower = Instantiate(flowerPrefab, this.transform);
        flower.transform.position = worldPos;
        flower.ShowBud();
        flower.ShowFlower();
    }
}
