


namespace Left4Dots.DebugUtils.Draw
{
	public interface IDebugDrawable
	{
		EDebugDrawCategory DebugDrawCategory { get; }

		void DebugDraw();
	}
}