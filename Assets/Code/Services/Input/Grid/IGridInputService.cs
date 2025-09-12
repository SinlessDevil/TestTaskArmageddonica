using System;
using Code.Logic.Grid;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.Input.Grid
{
	public interface IGridInputService
	{
		event Action<InvocationDTO, Cell> DroppedInvocationInCellEvent;
		event Action СancelledDropInvocationInCellEvent;
		void Enable();
		void Disable();
		void SetInvocationDTO(InvocationDTO invocationDTO);
	}
}