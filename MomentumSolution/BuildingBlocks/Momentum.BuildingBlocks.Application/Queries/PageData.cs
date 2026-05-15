namespace Momentum.BuildingBlocks.Application.Queries;

public struct PageData(int offset, int next)
{
    public int Offset { get; } = offset;

    public int Next { get; } = next;
}