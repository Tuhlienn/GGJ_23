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

    public void SpawnFlower(BranchNode branchNode)
    {
        if(branchNode == null)
        {
            branchNode = new BranchNode(new HexVector(0,0), HexVector.Up);
        }

        if(this.flowers.ContainsKey(branchNode.Position))
            return;


        

        //Vector3 currentHexPos = branchNode.Position.ToWorldPosition();

        Vector3 pos = branchNode.Position.ToWorldPosition();


        pos.z = -1;

        var flower = Instantiate(flowerPrefab, this.transform);
        flower.transform.position = pos;
        flower.ShowBud();
        flower.ShowFlower();

        flowers.Add(branchNode.Position, flower);
    }
}
