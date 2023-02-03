using System.Collections.Generic;
using HexGrid;

public class Stem
{
    private readonly HexGrid<StemNode> _grid;
    public List<StemBranch> Branches { get; }

    public Stem()
    {
        _grid = new HexGrid<StemNode>();
        Branches = new List<StemBranch>
        {
            new(_grid, HexVector.Zero, HexVector.Up)
        };
    }

    public void Tick()
    {
        foreach (StemBranch branch in Branches)
        {
            branch.Tick();
        }
    }
}
