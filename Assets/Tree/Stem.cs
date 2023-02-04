using System.Collections.Generic;
using HexGrid;
using UnityEngine;

public class Stem
{
    public HexGrid<StemNode> Grid { get; }
    public List<StemBranch> Branches { get; }

    public Stem()
    {
        Grid = new HexGrid<StemNode>();
        Branches = new List<StemBranch>
        {
            new(this, HexVector.Zero, HexVector.Up)
        };
    }

    public void AddBranch(StemBranch newBranch)
    {
        Branches.Add(newBranch);
    }

    public void RemoveBranch(StemBranch branch)
    {
        Branches.Remove(branch);
    }

    public void Tick()
    {
        Debug.Log($"Updating {Branches.Count} Branches");
        var lastBranches = new List<StemBranch>(Branches);
        foreach (StemBranch branch in lastBranches)
        {
            branch.Tick();
        }
    }
}
