

namespace Left4Dots.DebugUtils.Draw
{
	[System.Flags]
	public enum EDebugDrawCategory
	{
		Invalid			= 0,
		QuadTree		= 1 << 0,
		Locomotion		= 1 << 2,

		AI				= 1 << 1,
		AI_Pathing		= 1 << 3,
		AI_Avoidance	= 1 << 4,
		AI_FlowField	= 1 << 5,
	}
}