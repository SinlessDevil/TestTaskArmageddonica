using Code.Logic.Grid;
using UnityEngine;

namespace Code.Services.Input.Grid
{
	public class GridInputService : IGridInputService
	{
		private Camera _camera;
		private bool _isEnabled;
		private Cell _hoverCell;
		
		// Debug visualization
		private readonly Color _rayColorNoHit = Color.red;
		private readonly Color _rayColorHit = Color.green;
		private readonly Color _hitNormalColor = Color.yellow;
		private const float _maxRayDistance = 100f;
		
		private readonly IInputService _inputService;

		public GridInputService(IInputService inputService)
		{
			_inputService = inputService;
		}

		public bool IsEnabled => _isEnabled;

		public void Enable()
		{
			if (_isEnabled)
				return;

			_camera = Camera.main;
			_isEnabled = true;
			Subscribe();
			
			Debug.Log("GridInputService Enabled");
		}

		public void Disable()
		{
			if (!_isEnabled)
				return;

			Unsubscribe();
			ClearHover();
			_isEnabled = false;
			
			Debug.Log("GridInputService Disabled");
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
			Debug.Log("Update GridInputService");
			
			if (!_isEnabled || _camera == null)
				return;

			Ray ray = _camera.ScreenPointToRay(_inputService.TouchPosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				DrawRayGizmos(ray, hit);
				Cell cell = hit.collider.GetComponent<Cell>();
				if (cell != null)
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

			if (_hoverCell.StateCell != TypeStateCell.Fulled)
			{
				_hoverCell.SetFulledState();
			}
		}

		private void OnPointerUp() { }

		private void HandleHover(Cell cell)
		{
			if (_hoverCell == cell)
				return;
			
			if (_hoverCell != null && _hoverCell.StateCell != TypeStateCell.Fulled)
			{
				_hoverCell.SetEmptyState();
			}

			_hoverCell = cell;

			if (_hoverCell.StateCell != TypeStateCell.Fulled)
			{
				_hoverCell.SetSelectedState();
			}
		}

		private void ClearHover()
		{
			if (_hoverCell == null)
				return;

			if (_hoverCell.StateCell != TypeStateCell.Fulled)
			{
				_hoverCell.SetEmptyState();
			}

			_hoverCell = null;
		}

		private void DrawRayGizmos(Ray ray, RaycastHit? hit)
		{
			if (hit.HasValue)
			{
				var h = hit.Value;
				Debug.DrawLine(ray.origin, h.point, _rayColorHit, 0f, false);
				Debug.DrawRay(h.point, h.normal * 0.25f, _hitNormalColor, 0f, false);
			}
			else
			{
				Debug.DrawRay(ray.origin, ray.direction * _maxRayDistance, _rayColorNoHit, 0f, false);
			}
		}
	}
}


