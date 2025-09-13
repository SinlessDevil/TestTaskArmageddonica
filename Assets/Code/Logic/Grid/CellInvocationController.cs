using System.Collections.Generic;
using Code.StaticData.Invocation;
using Code.Logic.Grid.Extensions;
using UnityEngine;

namespace Code.Logic.Grid
{
    public class CellInvocationController
    {
        private Vector3 _cellCenter;
        private float _objectSize = 1f;
        private float _spacing = 0.2f;
        private int _maxColumns = 3;
        private bool _useCircularLayout = false;
        private float _circularRadius = 0.5f;
        
        public void Initialize(Vector3 cellCenter, float objectSize = 1f, float spacing = 0.2f, 
            int maxColumns = 3)
        {
            _cellCenter = cellCenter;
            _objectSize = objectSize;
            _spacing = spacing;
            _maxColumns = maxColumns;
            
            UpdateInvocationPositions();
        }
        
        public List<Invocation.Invocation> Invocations { get; private set; } = new();
        
        public InvocationType TargetInvocationType { get; private set; } = InvocationType.Unknown;
        
        public string Id { get; private set; } = string.Empty;
        
        public string UniqueId { get; private set; } = string.Empty;
        
        public void SetLayoutMode(bool useCircularLayout, float circularRadius = 0.5f)
        {
            _useCircularLayout = useCircularLayout;
            _circularRadius = circularRadius;
            
            UpdateInvocationPositions();
        }
        
        public void UpdateLayoutSettings(float objectSize, float spacing, int maxColumns)
        {
            _objectSize = objectSize;
            _spacing = spacing;
            _maxColumns = maxColumns;
            
            UpdateInvocationPositions();
        }
        
        public void AddInvocation(Invocation.Invocation invocation, InvocationType targetInvocationType, 
            string id, string uniqueId)
        {
            if (HasFreeCell())
            {
                Id = id;
                UniqueId = uniqueId;
                TargetInvocationType = targetInvocationType;   
            }
            
            Invocations.Add(invocation);
            UpdateInvocationPositions();
        }

        public void ClearInvocations()
        {
            foreach (Invocation.Invocation invocation in Invocations) 
                Object.Destroy(invocation.gameObject);
            
            Invocations.Clear();
            
            Id = string.Empty;
            TargetInvocationType = InvocationType.Unknown;
        }
        
        public bool HasFreeCell() => Invocations.Count == 0;

        public bool HasAddedAdditionalInvocation(string id)
        {
            if (TargetInvocationType != InvocationType.Unit)
                return false;

            return Id == id;
        }
        
        public Vector3[] GetInvocationPositions()
        {
            if (_useCircularLayout)
                return CellLayoutExtensions.CalculateCircularPositions(_cellCenter, Invocations.Count, _circularRadius);

            return CellLayoutExtensions.CalculateGridPositions(_cellCenter, Invocations.Count, _objectSize,
                _spacing, _maxColumns);
        }
        
        private void UpdateInvocationPositions()
        {
            Vector3[] positions = GetInvocationPositions();
            for (int i = 0; i < Invocations.Count && i < positions.Length; i++)
                if (Invocations[i] != null) 
                    Invocations[i].transform.position = positions[i];
        }
    }
}
