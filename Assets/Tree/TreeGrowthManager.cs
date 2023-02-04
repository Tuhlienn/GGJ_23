using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using HexGrid;
using Instructions;
using UnityEngine;

namespace Tree
{
    public class TreeGrowthManager : MonoBehaviour
    {
        [SerializeField] private float tickTime = 1;

        private bool _isRunning;
        private float _timer;
        private List<HexVector> _collisions;
        public HexGrid<BranchNode> Grid { get; } = new();
        public List<IInstructionMover> InstructionMovers { get; } = new();

        public void StartNewTree()
        {
            Grid.Clear();
            InstructionMovers.Clear();
            InstructionMovers.Add(new TreeBranch(this, HexVector.Zero, HexVector.Up));
        }

        public void SetRunning(bool running)
        {
            _isRunning = running;
        }

        public void RegisterMover(IInstructionMover newMover) => InstructionMovers.Add(newMover);

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
            List<IInstructionMover> runningMovers = InstructionMovers.Where(mover => !mover.HasEnded).ToList();
            PrecalculateNextStep(runningMovers);
            PerformNextStep(runningMovers);
        }

        private void PrecalculateNextStep(List<IInstructionMover> instructionMovers)
        {
            IEnumerable<HexVector> nextNodes = CalculateNextPositions(instructionMovers);
            _collisions = CalculateCollidingNodes(nextNodes);
        }

        private void PerformNextStep(List<IInstructionMover> instructionMovers)
        {
            Debug.Log($"Updating {instructionMovers.Count} Branches");
            foreach (IInstructionMover mover in instructionMovers)
            {
                mover.PerformInstruction(_collisions);
            }
        }

        private static IEnumerable<HexVector> CalculateNextPositions(List<IInstructionMover> instructionMovers)
        {
            var nextAddedNodes = new List<HexVector>();
            foreach (IInstructionMover mover in instructionMovers)
            {
                mover.PreCalculateInstruction(((InstructionType[]) Enum.GetValues(typeof(InstructionType))).Random());
                nextAddedNodes.AddRange(mover.NextMoves);
            }

            return nextAddedNodes;
        }

        private List<HexVector> CalculateCollidingNodes(IEnumerable<HexVector> positions)
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
