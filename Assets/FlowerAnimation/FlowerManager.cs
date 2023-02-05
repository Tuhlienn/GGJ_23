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
        treeManager.OnFlowerEvent += SpawnFlower;
        treeManager.OnNodesReset += ClearFlowers;
    }

    public void ClearFlowers()
    {
        foreach(FlowerAnimator f in flowers.Values)
    	    Destroy(f.gameObject);
        flowers.Clear();
    }

    public void SpawnFlower(HexVector gridPos)
    {
        if(this.flowers.ContainsKey(gridPos))
            return;

        Vector3 worldPos = gridPos.ToWorldPosition();
        worldPos.z = -1;

        var flower = Instantiate(flowerPrefab, this.transform);
        flower.transform.position = worldPos;
        flower.ShowBud();
        flower.ShowFlower();

        flowers.Add(gridPos, flower);
    }
}
