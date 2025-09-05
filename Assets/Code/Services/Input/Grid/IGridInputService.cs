namespace Code.Services.Input.Grid
{
	public interface IGridInputService
	{
		bool IsEnabled { get; }
		void Enable();
		void Disable();
	}
}


