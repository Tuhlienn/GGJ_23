using HexGrid;

public class BranchNode
{
    public enum NodeType { Default, Root }

    public HexVector Position { get; }
    public HexVector EntryDirection { get; }
    public NodeType Type { get; }

    public BranchNode(HexVector position, HexVector entryDirection, NodeType nodeType = NodeType.Default)
    {
        Position = position;
        EntryDirection = entryDirection;
        Type = nodeType;
    }
}
