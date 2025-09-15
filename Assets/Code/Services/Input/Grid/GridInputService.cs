using System;
using Code.Logic.Cell;
using Code.Logic.Grid;
using Code.Logic.Invocations;
using Code.Services.Context;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.Services.Input.Grid
{
	public class GridInputService : IGridInputService
	{
		private const float MaxRayDistance = 100f;
		
		private Camera _camera;
		private bool _isEnabled;
		
		private Cell _hoverCell;
		private InvocationDTO _invocationDto;
		private bool _isDragActive;
		
		private readonly Color _rayColorNoHit = Color.red;
		private readonly Color _rayColorHit = Color.green;
		private readonly Color _hitNormalColor = Color.yellow;
		
		private readonly IInputService _inputService;
		private readonly IGameContext _gameContext;

		public GridInputService(
			IInputService inputService,
			IGameContext gameContext)
		{
			_inputService = inputService;
			_gameContext = gameContext;
		}
		
		public event Action<InvocationDTO, Cell> DroppedInvocationInCellEvent;
		public event Action СancelledDropInvocationInCellEvent;

		public void Enable()
		{
			if (_isEnabled)
				return;

			_camera = Camera.main;
			_isEnabled = true;
			Subscribe();
		}

		public void Disable()
		{
			if (!_isEnabled)
				return;

			Unsubscribe();
			ClearHover();
			_isEnabled = false;
		}

		public void SetInvocationDTO(InvocationDTO invocationDTO) 
		{
			_invocationDto = invocationDTO;
			_isDragActive = invocationDTO != null;
		}
		
		public void CancelDrag()
		{
			if (_isDragActive)
			{
				СancelledDropInvocationInCellEvent?.Invoke();
				
				_invocationDto = null;
				_hoverCell = null;
				_isDragActive = false;
			}
		}
		
		private void Subscribe()
		{
			_inputService.InputUpdateEvent += OnUpdate;
			_inputService.PointerDownEvent += OnPointerDown;
			_inputService.PointerUpEvent += OnPointerUp;
		}

		private void Unsubscribe()
		{
			_inputService.InputUpdateEvent -= OnUpdate;
			_inputService.PointerDownEvent -= OnPointerDown;
			_inputService.PointerUpEvent -= OnPointerUp;
		}

		private void OnUpdate()
		{
			if (!_isEnabled)
				return;

			Ray ray = _camera.ScreenPointToRay(_inputService.TouchPosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				DrawRayGizmos(ray, hit);
				Cell cell = hit.collider.GetComponent<Cell>();
				if (cell != null || _invocationDto != null)
				{
					HandleHover(cell);
					return;
				}
			}
			else
			{
				DrawRayGizmos(ray, null);
			}
			
			ClearHover();
		}

		private void OnPointerDown()
		{
			if (!_isEnabled || _hoverCell == null)
				return;

			if (_hoverCell.VisualController.StateCell != TypeStateCell.Fulled)
				_hoverCell.VisualController.SetFilledState();
		}

		private void OnPointerUp()
		{
			if (!_isDragActive)
				return;

			if (CanDropInvocation())
			{
				if (_hoverCell.InvocationController.HasAddedAdditionalInvocation(_invocationDto.Id))
				{
					_invocationDto.UniqueId = _hoverCell.InvocationController.UniqueId;
				}
				DroppedInvocationInCellEvent?.Invoke(_invocationDto, _hoverCell);
				UpdateAllCellVisual();
			}
			else
			{
				СancelledDropInvocationInCellEvent?.Invoke();
			}

			ResetDragState();
		}

		private void HandleHover(Cell cell)
		{
			if (_hoverCell == cell)
				return;
			
			ClearPreviousHover();
			_hoverCell = cell;
			UpdateCellVisualState();
		}

		private void ClearHover()
		{
			if (_hoverCell == null)
				return;
			
			if (_hoverCell.InvocationController.Invocations.Count > 0)
			{
				_hoverCell.VisualController.SetFilledState();
			}
			else
			{
				_hoverCell.VisualController.SetEmptyState();
			}

			_hoverCell = null;
		}

		private void DrawRayGizmos(Ray ray, RaycastHit? hit)
		{
			if (hit.HasValue)
			{
				RaycastHit h = hit.Value;
				Debug.DrawLine(ray.origin, h.point, _rayColorHit, 0f, false);
				Debug.DrawRay(h.point, h.normal * 0.25f, _hitNormalColor, 0f, false);
			}
			else
			{
				Debug.DrawRay(ray.origin, ray.direction * MaxRayDistance, _rayColorNoHit, 0f, false);
			}
		}

		private bool CanDropInvocation()
		{
			if (_hoverCell == null || _invocationDto == null)
				return false;
			
			if (_invocationDto is SkillDTO)
			{
				return HasUnitInCell(_hoverCell);
			}

			return _hoverCell.InvocationController.HasFreeCell() || 
			       _hoverCell.InvocationController.HasAddedAdditionalInvocation(_invocationDto.Id);
		}

		private bool HasUnitInCell(Cell cell)
		{
			if (cell?.InvocationController?.Invocations == null)
				return false;

			if (cell.InvocationController.Invocations.Count > 0 && 
			    cell.InvocationController.TargetInvocationType == InvocationType.Unit)
				return true;

			return false;
		}

		private void ResetDragState()
		{
			_invocationDto = null;
			_hoverCell = null;
			_isDragActive = false;
		}

		private void ClearPreviousHover()
		{
			if(_hoverCell == null)
				return;
			
			if (_hoverCell.InvocationController.HasFreeCell())
				_hoverCell.VisualController.SetEmptyState();
			else if(!_hoverCell.InvocationController.HasFreeCell())
				_hoverCell.VisualController.SetFilledState();
		}

		private void UpdateCellVisualState()
		{
			if (_hoverCell == null)
				return;
			
			if (_isDragActive && _invocationDto is SkillDTO)
			{
				if (HasUnitInCell(_hoverCell))
				{
					_hoverCell.VisualController.SetSelectedState();
				}
				else
				{
					_hoverCell.VisualController.SetNotSelectedState();
				}
				return;
			}

			if (_hoverCell.InvocationController.HasFreeCell())
			{
				_hoverCell.VisualController.SetSelectedState();
				return;
			}
			
			if (_isDragActive && _invocationDto != null)
			{
				if (_hoverCell.InvocationController.HasAddedAdditionalInvocation(_invocationDto.Id))
				{
					_hoverCell.VisualController.SetSelectedState();
				}
				else
				{
					_hoverCell.VisualController.SetNotSelectedState();
				}
			}
			else
			{
				_hoverCell.VisualController.SetNotSelectedState();
			}
		}

		private void UpdateAllCellVisual()
		{
			foreach (var cell in _gameContext.PlayerGrid.Cells)
			{
				if (cell.InvocationController.HasFreeCell())
					cell.VisualController.SetEmptyState();
				else if(!cell.InvocationController.HasFreeCell())
					cell.VisualController.SetFilledState();
			}
		}
	}
}