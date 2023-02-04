using System;
using System.Collections.Generic;
using Extensions;
using HexGrid;
using UnityEngine;

public class StemBranch
{
    private readonly Stem _stem;
    private HexVector _position;
    private HexVector _direction;
    private bool _hasEnded;
    public List<StemNode> Path { get; }

    private static readonly Action<StemBranch>[] PossibleActions =
    {
        branch => branch.MoveForward(),
        branch => branch.MoveLeft(),
        branch => branch.MoveRight(),
        branch => branch.SplitLeftAndMoveForward(),
        branch => branch.SplitRightAndMoveForward()
    };


    public StemBranch(Stem stem, HexVector startPosition, HexVector startDirection)
    {
        _stem = stem;
        Path = new List<StemNode>();
        _position = startPosition ?? HexVector.Zero;
        _direction = startDirection ?? HexVector.Up;
    }

    public void Tick()
    {
        if (_hasEnded)
            return;

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

    private void SplitLeftAndMoveForward()
    {
        var newBranch = new StemBranch(_stem, _position, _direction.RotateLeft());
        _stem.AddBranch(newBranch);

        MoveForward();
    }

    private void SplitRightAndMoveForward()
    {
        var newBranch = new StemBranch(_stem, _position, _direction.RotateRight());
        _stem.AddBranch(newBranch);

        MoveForward();
    }

    private void MoveBy(HexVector direction)
    {
        HexVector newPosition = _position + direction;

        if (_stem.Grid.HasNodeAtPosition(newPosition))
        {
            EndBranch();
            return;
        }

        var newStemNode = new StemNode(newPosition, _direction);
        _stem.Grid.AddNodeAtPosition(newStemNode, newPosition);
        Path.Add(newStemNode);
        _position = newPosition;
        _direction = direction;
    }

    private void EndBranch()
    {
        //_stem.RemoveBranch(this);
        _hasEnded = true;
    }
}
