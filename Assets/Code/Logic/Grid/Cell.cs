using UnityEngine;

namespace Code.Logic.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellStateController stateController;

        public void Initialize()
        {
            stateController?.Initialize();
        }

        public TypeStateCell StateCell => stateController?.CurrentState ?? TypeStateCell.Empty;
        
        public void SetEmptyState()
        {
            stateController?.SetEmptyState();
        }
        
        public void SetFulledState()
        {
            stateController?.SetFulledState();
        }
        
        public void SetSelectedState()
        {
            stateController?.SetSelectedState();
        }
        
        public void SetState(TypeStateCell state)
        {
            stateController?.SetState(state);
        }
    }
}