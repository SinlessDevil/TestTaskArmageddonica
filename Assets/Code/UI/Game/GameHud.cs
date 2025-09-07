using System.Collections.Generic;
using Code.Services.StaticData;
using Code.UI.Game.Cards.Holder;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Game
{
    public class GameHud : MonoBehaviour
    {
        [Space(10)] [Header("Components")]
        [SerializeField] private CardHolder _cardHolder;
        [Space(10)] [Header("Other")]
        [SerializeField] private List<GameObject> _debugObjects;
        
        private IStaticDataService _staticDataService; 
        
        [Inject]
        public void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Initialize()
        {
            InitDebugObjects();
            
            TrySetUpEventSystem();
            
            InitCardHolder();
        }

        private void InitCardHolder()
        {
            _cardHolder.Initialize();
        }

        private void InitDebugObjects()
        {
            if (_staticDataService.GameConfig.DebugMode)
            {
                foreach (var debugObject in _debugObjects)
                {
                    debugObject.SetActive(true);
                }
            }
        }

        private static void TrySetUpEventSystem()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var gameObjectEventSystem = new GameObject("EventSystem");
                gameObjectEventSystem.AddComponent<EventSystem>();
                gameObjectEventSystem.AddComponent<StandaloneInputModule>();
            }
        }
    }
}