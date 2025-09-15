using Code.Logic.Grid.Extensions;
using UnityEngine;

namespace Code.Logic.Cell
{
    public class CellInvocationGizmos : MonoBehaviour
    {
        [Header("Layout Settings")]
        [SerializeField] private float _objectSize = 1f;
        [SerializeField] private float _spacing = 0.2f;
        [SerializeField] private int _maxColumns = 3;
        [SerializeField] private bool _useCircularLayout = false;
        [SerializeField] private float _circularRadius = 0.5f;
        [Header("Gizmos Settings")]
        [SerializeField] private bool _showGizmos = true;
        [SerializeField] private Color _cellCenterColor = Color.yellow;
        [SerializeField] private Color _cellBoundsColor = Color.white;
        [SerializeField] private Color _positionColor = Color.green;
        [SerializeField] private Color _connectionColor = Color.gray;
        [SerializeField] private Color _circleColor = Color.cyan;
        [Header("Test Settings")]
        [SerializeField] private int _testObjectCount = 4;
        [SerializeField] private bool _showTestPositions = true;
        
        private CellInvocationController _cellInvocationController;
        private Vector3 _cellCenter;
        
        private void Awake()
        {
            _cellCenter = transform.position;
            _cellInvocationController = GetComponent<Logic.Cell.Cell>()?.InvocationController;
        }
        
        private void OnDrawGizmos()
        {
            if (!_showGizmos)
                return;
                
            _cellCenter = transform.position;
            
            DrawCellCenter();
            DrawCellBounds();

            if (_showTestPositions)
                DrawTestPositions();
            else if (_cellInvocationController != null) 
                DrawInvocationPositions();
        }
        
        private void DrawCellCenter()
        {
            Gizmos.color = _cellCenterColor;
            Gizmos.DrawWireSphere(_cellCenter, 0.1f);
            
            float crossSize = 0.2f;
            Gizmos.DrawLine(
                _cellCenter + Vector3.left * crossSize,
                _cellCenter + Vector3.right * crossSize
            );
            Gizmos.DrawLine(
                _cellCenter + Vector3.forward * crossSize,
                _cellCenter + Vector3.back * crossSize
            );
        }
        
        private void DrawCellBounds()
        {
            Gizmos.color = _cellBoundsColor;
            float cellSize = _objectSize + _spacing;
            Gizmos.DrawWireCube(_cellCenter, new Vector3(cellSize * 2, 0.1f, cellSize * 2));
        }
        
        private void DrawTestPositions()
        {
            Vector3[] positions;
            
            if (_useCircularLayout)
            {
                positions = CellLayoutExtensions.CalculateCircularPositions(_cellCenter, _testObjectCount, _circularRadius);
                DrawCircle();
            }
            else
            {
                positions = CellLayoutExtensions.CalculateGridPositions(_cellCenter, _testObjectCount, _objectSize, _spacing, _maxColumns);
            }
            
            DrawPositions(positions);
            DrawConnections(positions);
        }
        
        private void DrawInvocationPositions()
        {
            if (_cellInvocationController == null)
                return;
                
            Vector3[] positions = _cellInvocationController.GetInvocationPositions();
            
            if (_useCircularLayout) 
                DrawCircle();
            
            DrawPositions(positions);
            DrawConnections(positions);
        }
        
        private void DrawCircle()
        {
            Gizmos.color = _circleColor;
            
            int segments = 32;
            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * 360f / segments * Mathf.Deg2Rad;
                float angle2 = (i + 1) * 360f / segments * Mathf.Deg2Rad;
                
                Vector3 pos1 = _cellCenter + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * _circularRadius;
                Vector3 pos2 = _cellCenter + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * _circularRadius;
                
                Gizmos.DrawLine(pos1, pos2);
            }
        }
        
        private void DrawPositions(Vector3[] positions)
        {
            Gizmos.color = _positionColor;
            
            foreach (var pos in positions)
            {
                Gizmos.DrawWireCube(pos, Vector3.one * _objectSize);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pos, 0.05f);
                Gizmos.color = _positionColor;
            }
        }
        
        private void DrawConnections(Vector3[] positions)
        {
            Gizmos.color = _connectionColor;
            
            foreach (var pos in positions) 
                Gizmos.DrawLine(_cellCenter, pos);
        }
        
        private void OnValidate()
        {
            if (_cellInvocationController == null)
                return;
            
            _cellInvocationController.UpdateLayoutSettings(_objectSize, _spacing, _maxColumns);
            _cellInvocationController.SetLayoutMode(_useCircularLayout, _circularRadius);
        }
    }
}
