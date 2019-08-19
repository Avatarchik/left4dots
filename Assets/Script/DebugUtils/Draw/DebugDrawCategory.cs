

namespace Left4Dots.DebugUtils.Draw
{
	[System.Flags]
	public enum EDebugDrawCategory
	{
		Invalid			= 0,
		Test			= 1 << 0,
		Locomotion		= 1 << 1,
	}
}