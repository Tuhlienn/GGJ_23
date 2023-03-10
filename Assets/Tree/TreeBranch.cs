using System;
using System.Collections.Generic;
using System.Linq;
using Hex;
using Instructions;
using Tree;
using UnityEngine;

public class TreeBranch
{
    private static readonly Dictionary<InstructionType, Action<TreeBranch>> PossibleActions = new()
    {
        { InstructionType.Empty, branch => branch.SkipCurrentInstruction() },
        { InstructionType.MoveForward, branch => branch.MoveForward() },
        { InstructionType.MoveLeft, branch => branch.MoveLeft() },
        { InstructionType.MoveRight, branch => branch.MoveRight() },
        { InstructionType.SplitLeftAndMoveForward, branch => branch.SplitLeftAndMoveForward() },
        { InstructionType.SplitRightAndMoveForward, branch => branch.SplitRightAndMoveForward() },
        { InstructionType.SplitLeftAndRight, branch => branch.SplitLeftAndRight() },
        { InstructionType.MoveToSun, branch => branch.MoveToSun() },
        { InstructionType.CreateFlower, branch => branch.SpawnFlower() }
    };

    private readonly TreeGrowthManager _treeGrowthManager;
    private HexVector _position;
    private HexVector _direction;

    private readonly Node _startInstructionNode;
    private Node _currentInstructionNode;
    private Node _startInstructionOfCurrentTick;
    private readonly List<TreeBranch> _splitBranches;

    public bool HasEnded { get; private set; }
    public List<BranchNode> Path { get; }

    public HexVector Position => _position;

    public IEnumerable<HexVector> NextMoves => _splitBranches
        .Select(branch => branch._position)
        .Concat(new[] { _position })
        .ToList();

    public (BranchNode current, BranchNode next) LastAdded => Path.Count <= 1
        ? (Path[^1], null)
        : (Path[^2], Path[^1]);

    public TreeBranch(TreeGrowthManager treeGrowthManager, BranchNode startNode, HexVector startDirection, Node startInstruction)
    {
        _startInstructionNode = startInstruction;
        _currentInstructionNode = startInstruction;

        _treeGrowthManager = treeGrowthManager;
        _position = startNode.Position;
        _direction = startDirection;
        _splitBranches = new List<TreeBranch>();
        Path = new List<BranchNode> { startNode };
        _treeGrowthManager.Grid.AddNodeAtPosition(startNode, startNode.Position);
    }

    public void PreCalculateInstruction()
    {
        _startInstructionOfCurrentTick = _currentInstructionNode;
        _splitBranches.Clear();
        PossibleActions[_currentInstructionNode.Instruction](this);
    }

    public void PerformInstruction(IReadOnlyCollection<HexVector> collisions)
    {
        if (_currentInstructionNode.Instruction == InstructionType.Empty
             || _currentInstructionNode.Instruction == InstructionType.CreateFlower)
        {
            GoToNextInstruction();
            return;
        }

        PlaceNodeAtCurrentPosition();

        if (collisions.Contains(_position))
            EndBranch();

        foreach (TreeBranch splitBranch in _splitBranches)
        {
            splitBranch.PlaceNodeAtCurrentPosition();

            if (collisions.Contains(splitBranch._position))
            {
                splitBranch.EndBranch();
            }

            _treeGrowthManager.RegisterBranch(splitBranch);
        }

        GoToNextInstruction();
    }

    private void GoToNextInstruction()
    {
        _currentInstructionNode = _currentInstructionNode.GetNextNode();
        if (_currentInstructionNode == null)
            _currentInstructionNode = _startInstructionNode;
    }

    private void SkipCurrentInstruction()
    {
        var prevNode = _currentInstructionNode;
        GoToNextInstruction();
        if(_currentInstructionNode == _startInstructionOfCurrentTick)
            return; //endless loop since we are back at the same instruction
        PossibleActions[_currentInstructionNode.Instruction]?.Invoke(this);
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
        AddNewBranchInDirection(_direction.RotateLeft());
        MoveForward();
    }

    private void SplitRightAndMoveForward()
    {
        AddNewBranchInDirection(_direction.RotateRight());
        MoveForward();
    }

    private void SplitLeftAndRight()
    {
        AddNewBranchInDirection(_direction.RotateLeft());
        AddNewBranchInDirection(_direction.RotateRight());
        EndBranch();
    }

    private void MoveToSun()
    {
        HexVector newDirection;
        if (_direction == HexVector.Down)
            newDirection = _position.IsOnLeftScreenHalf ? _direction.RotateRight() : _direction.RotateLeft();
        else if (_direction == HexVector.UpLeft || _direction == HexVector.DownLeft)
            newDirection = _direction.RotateRight();
        else if (_direction == HexVector.UpRight || _direction == HexVector.DownRight)
            newDirection = _direction.RotateLeft();
        else
            newDirection = _direction;

        MoveBy(newDirection);
    }

    public void SpawnFlower()
    {
        _treeGrowthManager.SendFlowerEvent(LastAdded.next);
        SkipCurrentInstruction();
    }

    private void AddNewBranchInDirection(HexVector newDirection)
    {
        var newBranch = new TreeBranch(
            _treeGrowthManager,
            new BranchNode(_position, _direction, BranchNode.NodeType.Root),
            newDirection,
            _startInstructionNode);

        newBranch.MoveForward();
        _splitBranches.Add(newBranch);
    }

    private void PlaceNodeAtCurrentPosition()
    {
        var newNode = new BranchNode(_position, _direction);
        _treeGrowthManager.Grid.AddNodeAtPosition(newNode, _position);
        Path.Add(newNode);
    }

    public void EndBranch()
    {
        HasEnded = true;
    }
}
