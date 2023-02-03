using HexGrid;

public class StemNode
{
    public HexVector Position { get; }
    public HexVector EntryDirection { get; }

    public StemNode(HexVector position, HexVector entryDirection)
    {
        Position = position;
        EntryDirection = entryDirection;
    }
}
