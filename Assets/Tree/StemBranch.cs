using System;
using Extensions;
using HexGrid;

public class StemBranch
{
    private readonly HexGrid<StemNode> _grid;
    private HexVector _position;
    private HexVector _direction;

    private static readonly Action<StemBranch>[] PossibleActions =
    {
        branch => branch.MoveForward(),
        branch => branch.MoveLeft(),
        branch => branch.MoveRight()
    };

    public StemBranch(HexGrid<StemNode> grid, HexVector startPosition, HexVector startDirection)
    {
        _grid = grid;
        _position = startPosition ?? HexVector.Zero;
        _direction = startDirection ?? HexVector.Up;
    }

    public void Tick()
    {
        PossibleActions.Random().Invoke(this);
    }

    private void MoveForward()
    {
        HexVector newPosition = _position + _direction;

        if (_grid.HasNodeAtPosition(newPosition))
        {
            // TODO: Stop
            return;
        }

        var newStemNode = new StemNode(newPosition, _direction);
        _grid.AddNodeAtPosition(newStemNode, newPosition);
        _position = newPosition;
    }

    private void MoveLeft()
    {
        _direction = _direction.RotateLeft();
        MoveForward();
    }

    private void MoveRight()
    {
        _direction = _direction.RotateRight();
        MoveForward();
    }
}
