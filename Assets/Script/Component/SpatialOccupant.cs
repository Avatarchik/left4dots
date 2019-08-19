using Unity.Entities;
using Unity.Mathematics;

public struct SpatialOccupant : IComponentData
{
	public int m_gridIndex; // index into our list of all grids representing the world's accessible space
}