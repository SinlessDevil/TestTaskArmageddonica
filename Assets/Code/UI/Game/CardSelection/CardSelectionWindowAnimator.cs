using System;
using UnityEngine;

namespace Code.UI.Game.CardSelection
{
    public class CardSelectionWindowAnimator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;

        public void Show()
        {
            SetVisible(true);
        }

        public void Hide()
        {
            SetVisible(false);
        }

        public void Close(Action onCompleted)
        {
            SetVisible(false);
            onCompleted?.Invoke();
        }

        private void SetVisible(bool visible)
        {
            if (_group == null) return;
            _group.alpha = visible ? 1f : 0f;
            _group.blocksRaycasts = visible;
            _group.interactable = visible;
        }
    }
}

