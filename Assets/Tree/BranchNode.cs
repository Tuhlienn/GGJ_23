using HexGrid;

public class BranchNode : IGridPlaceable
{
    public HexVector Position { get; }
    public HexVector EntryDirection { get; }

    public BranchNode(HexVector position, HexVector entryDirection)
    {
        Position = position;
        EntryDirection = entryDirection;
    }
}
