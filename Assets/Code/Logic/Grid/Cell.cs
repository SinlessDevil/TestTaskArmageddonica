using UnityEngine;

namespace Code.Logic.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellVisualController _visualController;
        private CellInvocationController _invocationController;
        
        public void Initialize()
        {
            _visualController?.Initialize();
            _invocationController = new CellInvocationController();
        }

        public CellInvocationController CellInvocationController => _invocationController;

        public CellVisualController VisualController  => _visualController;
    }
}