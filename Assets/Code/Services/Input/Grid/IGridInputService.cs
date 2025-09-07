using Code.Logic.Grid;

namespace Code.Services.Input.Grid
{
	public interface IGridInputService
	{
		bool IsEnabled { get; }
		Cell HoverCell { get; }
		void Enable();
		void Disable();
	}
}


