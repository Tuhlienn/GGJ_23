﻿using System;
using System.Collections.Generic;
using System.Linq;
using HexGrid;
using Instructions;
using Tree;

public class TreeBranch : IInstructionMover
{
    private static readonly Dictionary<InstructionType, Action<TreeBranch>> PossibleActions = new()
    {
        { InstructionType.MoveForward, branch => branch.MoveForward() },
        { InstructionType.MoveLeft, branch => branch.MoveLeft() },
        { InstructionType.MoveRight, branch => branch.MoveRight() },
        { InstructionType.SplitLeftAndMoveForward, branch => branch.SplitLeftAndMoveForward() },
        { InstructionType.SplitRightAndMoveForward, branch => branch.SplitRightAndMoveForward() }
    };

    private readonly TreeGrowthManager _treeGrowthManager;
    private HexVector _position;
    private HexVector _direction;

    private InstructionType _nextInstruction;
    private readonly List<TreeBranch> _newBranches;

    public bool HasEnded { get; private set; }
    public List<BranchNode> Path { get; }

    public IEnumerable<HexVector> NextMoves => _newBranches
        .Select(branch => branch._position)
        .Concat(new[] { _position })
        .ToList();

    public TreeBranch(TreeGrowthManager treeGrowthManager, HexVector startPosition, HexVector startDirection)
    {
        _treeGrowthManager = treeGrowthManager;
        _position = startPosition ?? HexVector.Zero;
        _direction = startDirection ?? HexVector.Up;
        _newBranches = new List<TreeBranch>();
        Path = new List<BranchNode>();
        PlaceNodeAtCurrentPosition();
        MoveForward();
    }

    public void PreCalculateInstruction(InstructionType instruction)
    {
        _newBranches.Clear();
        _nextInstruction = instruction;
        PossibleActions[_nextInstruction].Invoke(this);
    }

    public void PerformInstruction(IReadOnlyCollection<HexVector> collisions)
    {
        if (!collisions.Contains(_position))
            PlaceNodeAtCurrentPosition();
        else
            EndBranch();

        foreach (TreeBranch branch in _newBranches.Where(branch => !collisions.Contains(branch._position)))
        {
            branch.PlaceNodeAtCurrentPosition();
            _treeGrowthManager.RegisterMover(branch);
        }
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
        _position += direction;
        _direction = direction;
    }

    private void SplitLeftAndMoveForward()
    {
        _newBranches.Add(new TreeBranch(_treeGrowthManager, _position, _direction.RotateLeft()));
        MoveForward();
    }

    private void SplitRightAndMoveForward()
    {
        _newBranches.Add(new TreeBranch(_treeGrowthManager, _position, _direction.RotateRight()));
        MoveForward();
    }

    private void PlaceNodeAtCurrentPosition()
    {
        var newNode = new BranchNode(_position, _direction);
        _treeGrowthManager.Grid.AddNodeAtPosition(newNode, _position);
        Path.Add(newNode);
    }

    private void EndBranch()
    {
        HasEnded = true;
    }
}