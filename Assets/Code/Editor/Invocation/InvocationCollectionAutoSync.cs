using System.Collections.Generic;
using Code.StaticData.Cards;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Code.Editor.Invocation
{
    [InitializeOnLoad]
    public static class InvocationCollectionAutoSync
    {
        private static readonly Queue<string> _pendingCardUpdates = new Queue<string>();
        
        static InvocationCollectionAutoSync()
        {
            Debug.Log("InvocationCollectionAutoSync initialized");
        }
        
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Debug.Log("Scripts recompiled. Processing pending CardDefinition updates...");
            while (_pendingCardUpdates.Count > 0)
            {
                string cardName = _pendingCardUpdates.Dequeue();
                UpdateCardDefinitionAfterReload(cardName);
            }
        }
        
        public static void ScheduleCardDefinitionUpdate(string cardName)
        {
            if (_pendingCardUpdates.Contains(cardName)) 
                return;
            
            _pendingCardUpdates.Enqueue(cardName);
            Debug.Log($"Scheduled CardDefinition update for: {cardName} (Queue size: {_pendingCardUpdates.Count})");
        }
        
        private static void UpdateCardDefinitionAfterReload(string cardName)
        {
            try
            {
                Debug.Log($"Processing CardDefinition update for: {cardName}");
                if (!System.Enum.TryParse(cardName, out CardDefinitionType result))
                {
                    Debug.LogWarning($"CardDefinitionType {cardName} not found in enum after reload, retrying...");
                    EditorApplication.delayCall += () => {
                        if (System.Enum.TryParse(cardName, out CardDefinitionType retryResult))
                        {
                            ProcessCardDefinitionUpdate(cardName, retryResult);
                        }
                        else
                        {
                            Debug.LogError($"CardDefinitionType {cardName} still not found after retry");
                        }
                    };
                    return;
                }
                
                ProcessCardDefinitionUpdate(cardName, result);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to update CardDefinition after reload: {e.Message}");
            }
        }
        
        private static void ProcessCardDefinitionUpdate(string cardName, CardDefinitionType cardType)
        {
            string[] guids = AssetDatabase.FindAssets($"t:CardDefinitionStaticData");
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardDefinitionStaticData cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(path);
                
                if (cardDefinition != null && cardDefinition.Name == cardName)
                {
                    cardDefinition.Type = cardType;
                    EditorUtility.SetDirty(cardDefinition);
                    AssetDatabase.SaveAssets();
                    CollectionUpdater.AddToCardDefinitionCollection(cardDefinition);
                    
                    Debug.Log($"Successfully updated CardDefinitionType to {cardType} for {cardName} and added to collection");
                    return;
                }
            }
            
            Debug.LogWarning($"CardDefinitionStaticData with name '{cardName}' not found");
        }
    }
}