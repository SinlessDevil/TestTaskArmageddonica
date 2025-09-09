using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;

namespace Code.Editor.Invocation
{
    [InitializeOnLoad]
    public static class InvocationCollectionAutoSync
    {
        private static bool _pendingCardDefinitionUpdate = false;
        private static string _pendingCardName = "";
        
        static InvocationCollectionAutoSync()
        {
            // Регистрируем callback для обновления после компиляции
            EditorApplication.delayCall += CheckPendingUpdates;
        }
        
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Debug.Log("Scripts recompiled. Checking for pending InvocationCollection updates...");
            
            if (_pendingCardDefinitionUpdate && !string.IsNullOrEmpty(_pendingCardName))
            {
                UpdateCardDefinitionAfterReload(_pendingCardName);
                _pendingCardDefinitionUpdate = false;
                _pendingCardName = "";
            }
        }
        
        private static void CheckPendingUpdates()
        {
            if (_pendingCardDefinitionUpdate && !string.IsNullOrEmpty(_pendingCardName))
            {
                UpdateCardDefinitionAfterReload(_pendingCardName);
                _pendingCardDefinitionUpdate = false;
                _pendingCardName = "";
            }
        }
        
        public static void ScheduleCardDefinitionUpdate(string cardName)
        {
            _pendingCardDefinitionUpdate = true;
            _pendingCardName = cardName;
            Debug.Log($"Scheduled CardDefinition update for: {cardName}");
        }
        
        private static void UpdateCardDefinitionAfterReload(string cardName)
        {
            try
            {
                // Находим CardDefinitionStaticData по имени
                string[] guids = AssetDatabase.FindAssets($"t:CardDefinitionStaticData {cardName}");
                
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(path);
                    
                    if (cardDefinition != null && cardDefinition.Name == cardName)
                    {
                        // Обновляем тип после компиляции enum
                        if (System.Enum.TryParse<CardDefinitionType>(cardName, out CardDefinitionType result))
                        {
                            cardDefinition.Type = result;
                            EditorUtility.SetDirty(cardDefinition);
                            AssetDatabase.SaveAssets();
                            
                            // Добавляем в коллекцию
                            CollectionUpdater.AddToCardDefinitionCollection(cardDefinition);
                            
                            Debug.Log($"Updated CardDefinitionType to {result} for {cardName} and added to collection");
                        }
                        else
                        {
                            Debug.LogWarning($"Could not find CardDefinitionType for {cardName} after reload");
                        }
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to update CardDefinition after reload: {e.Message}");
            }
        }
    }
}
