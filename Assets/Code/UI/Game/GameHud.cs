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
        [SerializeField] private Canvas _canvas;
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

        public CardHolder CardHolder => _cardHolder;
        public Canvas Canvas => _canvas;
        
        private void InitCardHolder()
        {
            _cardHolder.Initialize();
        }

        private void InitDebugObjects()
        {
            if (!_staticDataService.GameConfig.DebugMode) 
                return;
            
            foreach (var debugObject in _debugObjects)
                debugObject.SetActive(true);
        }

        private void TrySetUpEventSystem()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null) 
                return;
            
            GameObject gameObjectEventSystem = new GameObject("EventSystem");
            gameObjectEventSystem.AddComponent<EventSystem>();
            gameObjectEventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}