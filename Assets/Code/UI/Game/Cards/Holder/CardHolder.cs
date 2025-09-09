using Code.UI.Game.Cards.View;
using UnityEngine;

namespace Code.UI.Game.Cards.Holder
{
    public class CardHolder : MonoBehaviour
    {
        [SerializeField] private CardLayoutEngine _layout;
        [SerializeField] private CardHolderAnimator _handAnim;

        public void Initialize()
        {
            _layout.Initialize();
            _handAnim.Initialize();
        }
        
        public void AddCard(CardView view)
        {
            _layout.AddCard(view);
        }

        public void RemoveCard(CardView view)
        {
            _layout.RemoveCard(view);
        }

        public void Show()
        {
            _handAnim.Show();
        }

        public void Hide()
        {
            _handAnim.Hide();
        }
    }
}