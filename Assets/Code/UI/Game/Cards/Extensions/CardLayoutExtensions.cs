using UnityEngine;

namespace Code.UI.Game.Cards.Extensions
{
    public static class CardLayoutExtensions
    {
        public static Vector2[] CalculateCenteredRowPositions(
            RectTransform root,
            int cardCount,
            float cardWidth,
            float spacingPreferred,
            float verticalOffset)
        {
            Vector2[] positions = new Vector2[cardCount];
            if (root == null || cardCount <= 0)
                return positions;

            float available = root.rect.width;
            float spacing = spacingPreferred;

            if (cardCount > 1)
            {
                float maxSpacingByWidth = (available - cardWidth) / (cardCount - 1);
                spacing = Mathf.Min(spacing, maxSpacingByWidth);
            }

            float totalWidth = (cardCount - 1) * spacing + cardWidth;
            float startX = -totalWidth * 0.5f + cardWidth * 0.5f;

            for (int i = 0; i < cardCount; i++)
                positions[i] = new Vector2(startX + i * spacing, verticalOffset);

            return positions;
        }
    }
}

