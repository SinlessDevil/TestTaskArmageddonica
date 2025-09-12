using UnityEngine;

namespace Code.Logic.Grid.Extensions
{
    public static class CellLayoutExtensions
    {
        public static Vector3[] CalculateGridPositions(Vector3 cellCenter, int objectCount, float objectSize,
            float spacing, int maxColumns = 3)
        {
            if (objectCount <= 0)
                return new Vector3[0];

            Vector3[] positions = new Vector3[objectCount];
            
            int columns = Mathf.Min(objectCount, maxColumns);
            int rows = Mathf.CeilToInt((float)objectCount / columns);
            
            float totalWidth = (columns - 1) * spacing + columns * objectSize;
            float totalDepth = (rows - 1) * spacing + rows * objectSize;
            
            float startX = cellCenter.x - totalWidth * 0.5f + objectSize * 0.5f;
            float startZ = cellCenter.z + totalDepth * 0.5f - objectSize * 0.5f;
            
            for (int i = 0; i < objectCount; i++)
            {
                int row = i / columns;
                int col = i % columns;
                
                float x = startX + col * (objectSize + spacing);
                float z = startZ - row * (objectSize + spacing);
                
                positions[i] = new Vector3(x, cellCenter.y, z);
            }
            
            return positions;
        }
        
        public static Vector3[] CalculateCircularPositions(Vector3 cellCenter, int objectCount, float radius)
        {
            if (objectCount <= 0)
                return new Vector3[0];
                
            Vector3[] positions = new Vector3[objectCount];
            
            if (objectCount == 1)
            {
                positions[0] = cellCenter;
                return positions;
            }
            
            float angleStep = 360f / objectCount;
            
            for (int i = 0; i < objectCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = cellCenter.x + Mathf.Cos(angle) * radius;
                float z = cellCenter.z + Mathf.Sin(angle) * radius;
                
                positions[i] = new Vector3(x, cellCenter.y, z);
            }
            
            return positions;
        }
    }
}
