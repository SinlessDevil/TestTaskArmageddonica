using UnityEngine;

namespace Code.Services.UniqueId
{
    /// <summary>
    /// Пример использования UniqueIdService
    /// </summary>
    public class UniqueIdServiceExample : MonoBehaviour
    {
        [SerializeField] private bool _generateOnStart = true;
        
        private IUniqueIdService _uniqueIdService;
        
        private void Start()
        {
            if (_generateOnStart)
            {
                GenerateExampleIds();
            }
        }
        
        [ContextMenu("Generate Example IDs")]
        public void GenerateExampleIds()
        {
            _uniqueIdService = new UniqueIdService();
            
            // Генерация простого ID
            string simpleId = _uniqueIdService.GenerateUniqueId();
            Debug.Log($"Simple ID: {simpleId}");
            
            // Генерация ID с префиксом
            string unitId = _uniqueIdService.GenerateUniqueId("Unit");
            Debug.Log($"Unit ID: {unitId}");
            
            string buildingId = _uniqueIdService.GenerateUniqueId("Building");
            Debug.Log($"Building ID: {buildingId}");
            
            string skillId = _uniqueIdService.GenerateUniqueId("Skill");
            Debug.Log($"Skill ID: {skillId}");
            
            // Генерация ID с префиксом и длиной
            string shortId = _uniqueIdService.GenerateUniqueId("Short", 4);
            Debug.Log($"Short ID: {shortId}");
            
            string longId = _uniqueIdService.GenerateUniqueId("Long", 16);
            Debug.Log($"Long ID: {longId}");
            
            // Генерация нескольких ID для демонстрации уникальности
            Debug.Log("=== Multiple IDs ===");
            for (int i = 0; i < 5; i++)
            {
                string id = _uniqueIdService.GenerateUniqueId("Test");
                Debug.Log($"Test ID {i + 1}: {id}");
            }
        }
    }
}
