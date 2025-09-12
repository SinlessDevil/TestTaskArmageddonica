using UnityEngine;

namespace Code.UI.Game.Finish.InvocationIcon.Extensions
{
    public static class InvocationLayoutExtensions
    {
        public static Vector2[] CalculateCenteredRowPositions(
            RectTransform root,
            int iconCount,
            float iconWidth,
            float spacingPreferred,
            float verticalOffset)
        {
            Vector2[] positions = new Vector2[iconCount];
            if (root == null || iconCount <= 0)
                return positions;
            
            float availableWidth = root.rect.width;
            float spacing = spacingPreferred;
            if (iconCount > 1)
            {
                float maxSpacingByWidth = (availableWidth - iconWidth) / (iconCount - 1);
                spacing = Mathf.Min(spacing, maxSpacingByWidth);
            }
            
            float totalWidth = (iconCount - 1) * spacing + iconWidth;
            float startX = -totalWidth * 0.5f + iconWidth * 0.5f;
            for (int i = 0; i < iconCount; i++)
            {
                float x = startX + i * spacing;
                positions[i] = new Vector2(x, verticalOffset);
            }

            return positions;
        }
    }
}
