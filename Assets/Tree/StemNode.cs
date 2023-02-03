using HexGrid;

public class StemNode
{
    private readonly HexVector _position;
    private readonly HexVector _entryDirection;

    public StemNode(HexVector position, HexVector entryDirection)
    {
        _position = position;
        _entryDirection = entryDirection;
    }
}
