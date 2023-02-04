using System.Collections.Generic;
using HexGrid;
using Instructions;

public interface IInstructionMover
{
    IEnumerable<HexVector> NextMoves { get; }
    bool HasEnded { get; }
    void PreCalculateInstruction();
    void PerformInstruction(IReadOnlyCollection<HexVector> collisionPositions);
}
