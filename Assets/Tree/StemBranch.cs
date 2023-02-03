using System;
using System.Collections.Generic;
using Extensions;
using HexGrid;
using UnityEngine;

public class StemBranch
{
    private readonly HexGrid<StemNode> _grid;
    private HexVector _position;
    private HexVector _direction;
    public List<StemNode> Path { get; }

    private static readonly Action<StemBranch>[] PossibleActions =
    {
        branch => branch.MoveForward(),
        branch => branch.MoveLeft(),
        branch => branch.MoveRight()
    };

    public StemBranch(HexGrid<StemNode> grid, HexVector startPosition, HexVector startDirection)
    {
        _grid = grid;
        Path = new List<StemNode>();
        _position = startPosition ?? HexVector.Zero;
        _direction = startDirection ?? HexVector.Up;
    }

    public void Tick()
    {
        PossibleActions.Random().Invoke(this);
    }

    private void MoveForward()
    {
        MoveBy(_direction);
    }

    private void MoveLeft()
    {
        MoveBy(_direction.RotateLeft());
    }

    private void MoveRight()
    {
        MoveBy(_direction.RotateRight());
    }

    private void MoveBy(HexVector direction)
    {
        HexVector newPosition = _position + direction;

        if (_grid.HasNodeAtPosition(newPosition))
        {
            // TODO: Stop
            return;
        }

        var newStemNode = new StemNode(newPosition, _direction);
        _grid.AddNodeAtPosition(newStemNode, newPosition);
        Path.Add(newStemNode);
        _position = newPosition;
        _direction = direction;

        Debug.Log($"Moved by {_direction} to {newPosition} | {newPosition.ToWorldPosition()}");
    }
}
