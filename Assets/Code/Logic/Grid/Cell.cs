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
        
        public Invocation.Invocation Invocation { get; private set; }
        
        public void SetInvocation(Invocation.Invocation invocation)
        {
            Invocation = invocation;
        }
        
        public bool HasFreeCell() => Invocation == null;
        
        public bool HasAddedAdditionalInvocation(string uniqueId) => Invocation.UniqueId == uniqueId;
        
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

        public void SetNotSelectedState()
        {
            stateController?.SetNotSelectedState();
        }
        
        public void SetState(TypeStateCell state)
        {
            stateController?.SetState(state);
        }
    }
}