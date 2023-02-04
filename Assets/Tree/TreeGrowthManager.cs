﻿using System;
using System.Collections.Generic;
using System.Linq;
using HexGrid;
using Instructions;
using UnityEngine;

namespace Tree
{
    public class TreeGrowthManager : MonoBehaviour
    {
        [SerializeField] private float tickTime = 1;
        [SerializeField] private Node startInstructionNode;

        private bool _isRunning;
        private float _timer;
        private List<HexVector> _collisions;
        public HexGrid<BranchNode> Grid { get; } = new();
        public List<TreeBranch> Branches { get; } = new();
        public event Action<List<(BranchNode current, BranchNode next)>> OnNodesUpdate;
        public event Action OnNodesReset;

        public void StartNewTree()
        {
            Grid.Clear();
            Branches.Clear();
            Branches.Add(new TreeBranch(this, new BranchNode(HexVector.Zero, HexVector.Up), HexVector.Up, startInstructionNode));
            OnNodesReset?.Invoke();
        }

        public void SetRunning(bool running)
        {
            _isRunning = running;
        }

        public void RegisterBranch(TreeBranch newBranch) => Branches.Add(newBranch);

        private void Update()
        {
            if (!_isRunning)
                return;

            _timer += Time.deltaTime;
            if (_timer < tickTime)
                return;

            Tick();
            _timer -= tickTime;
        }

        private void Tick()
        {
            RemoveStoppedBranches();

            List<TreeBranch> runningBranches = Branches.ToList();
            PrecalculateNextStep(runningBranches);
            PerformNextStep(runningBranches);
        }

        private void RemoveStoppedBranches()
        {
            for (int i = Branches.Count - 1; i >= 0; i--)
            {
                if (Branches[i].HasEnded)
                    Branches.Remove(Branches[i]);
            }
        }

        private void PrecalculateNextStep(List<TreeBranch> branches)
        {
            IEnumerable<HexVector> nextNodes = CalculateNextPositions(branches);
            _collisions = CalculateCollisions(nextNodes);
        }

        private void PerformNextStep(List<TreeBranch> branches)
        {
            foreach (TreeBranch branch in branches)
            {
                branch.PerformInstruction(_collisions);
            }

            List<(BranchNode, BranchNode)> updatedNodes = Branches
                .Where(branch => branch.Path.Count >= 2)
                .Select(branch => branch.LastAdded).ToList();
            OnNodesUpdate?.Invoke(updatedNodes);
        }

        private static IEnumerable<HexVector> CalculateNextPositions(List<TreeBranch> branches)
        {
            var nextAddedNodes = new List<HexVector>();
            foreach (TreeBranch branch in branches)
            {
                branch.PreCalculateInstruction();
                nextAddedNodes.AddRange(branch.NextMoves);
            }

            return nextAddedNodes;
        }

        private List<HexVector> CalculateCollisions(IEnumerable<HexVector> positions)
        {
            return positions
                .GroupBy(position => position)
                .Where(grouping => grouping.Key.IsBelowGround
                    || Grid.HasNodeAtPosition(grouping.Key)
                    || grouping.Count() > 1)
                .Select(grouping => grouping.Key)
                .ToList();
        }
    }
}
