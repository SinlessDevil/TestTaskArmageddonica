using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;

namespace Code.Editor.Invocation
{
    public static class CollectionUpdater
    {
        private const string INVOCATION_COLLECTION_PATH = "Assets/Resources/StaticData/Invocation/InvocationCollectionStaticData.asset";
        private const string CARD_DEFINITION_COLLECTION_PATH = "Assets/Resources/StaticData/Cards/CardDefinitionCollectionStaticData.asset";
        
        public static void AddToInvocationCollection(InvocationStaticData invocationData)
        {
            var collection = AssetDatabase.LoadAssetAtPath<InvocationCollectionStaticData>(INVOCATION_COLLECTION_PATH);
            if (collection == null)
            {
                Debug.LogError($"InvocationCollectionStaticData not found at {INVOCATION_COLLECTION_PATH}");
                return;
            }
            
            bool wasModified = false;
            
            switch (invocationData.InvocationType)
            {
                case InvocationType.Unit:
                    if (invocationData is UnitStaticData unitData)
                    {
                        if (!collection.UnitCollectionData.Contains(unitData))
                        {
                            collection.UnitCollectionData.Add(unitData);
                            wasModified = true;
                        }
                    }
                    break;
                    
                case InvocationType.Build:
                    if (invocationData is BuildStaticData buildData)
                    {
                        if (!collection.BuildCollectionData.Contains(buildData))
                        {
                            collection.BuildCollectionData.Add(buildData);
                            wasModified = true;
                        }
                    }
                    break;
                    
                case InvocationType.Skill:
                    if (invocationData is SkillStaticData skillData)
                    {
                        if (!collection.SkillCollectionData.Contains(skillData))
                        {
                            collection.SkillCollectionData.Add(skillData);
                            wasModified = true;
                        }
                    }
                    break;
            }
            
            if (wasModified)
            {
                EditorUtility.SetDirty(collection);
                AssetDatabase.SaveAssets();
                Debug.Log($"Added {invocationData.Id} to InvocationCollectionStaticData");
            }
        }
        
        public static void AddToCardDefinitionCollection(CardDefinitionStaticData cardDefinition)
        {
            Debug.Log($"Attempting to add {cardDefinition.Name} (Type: {cardDefinition.Type}) to CardDefinitionCollectionStaticData");
            
            var collection = AssetDatabase.LoadAssetAtPath<CardDefinitionCollectionStaticData>(CARD_DEFINITION_COLLECTION_PATH);
            if (collection == null)
            {
                Debug.LogWarning($"CardDefinitionCollectionStaticData not found at {CARD_DEFINITION_COLLECTION_PATH}, creating new one...");
                collection = CreateCardDefinitionCollectionStaticData();
                if (collection == null)
                {
                    Debug.LogError("Failed to create CardDefinitionCollectionStaticData");
                    return;
                }
            }
            
            if (collection.CardDefinitionStaticData == null)
            {
                collection.CardDefinitionStaticData = new Dictionary<CardDefinitionType, CardDefinitionStaticData>();
                Debug.Log("Initialized CardDefinitionStaticData dictionary");
            }
            
            if (!collection.CardDefinitionStaticData.ContainsKey(cardDefinition.Type))
            {
                collection.CardDefinitionStaticData[cardDefinition.Type] = cardDefinition;
                EditorUtility.SetDirty(collection);
                AssetDatabase.SaveAssets();
                Debug.Log($"Added {cardDefinition.Name} (Type: {cardDefinition.Type}) to CardDefinitionCollectionStaticData");
            }
            else
            {
                Debug.Log($"CardDefinitionType {cardDefinition.Type} already exists in collection, updating...");
                collection.CardDefinitionStaticData[cardDefinition.Type] = cardDefinition;
                EditorUtility.SetDirty(collection);
                AssetDatabase.SaveAssets();
            }
        }
        
        public static void UpdateAllCollections()
        {
            // Обновляем InvocationCollection
            UpdateInvocationCollection();
            
            // Обновляем CardDefinitionCollection
            UpdateCardDefinitionCollection();
        }
        
        private static CardDefinitionCollectionStaticData CreateCardDefinitionCollectionStaticData()
        {
            try
            {
                var collection = ScriptableObject.CreateInstance<CardDefinitionCollectionStaticData>();
                collection.CardDefinitionStaticData = new Dictionary<CardDefinitionType, CardDefinitionStaticData>();
                
                // Создаем папку если не существует
                string folderPath = "Assets/Resources/StaticData/Cards/";
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                
                AssetDatabase.CreateAsset(collection, CARD_DEFINITION_COLLECTION_PATH);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                Debug.Log($"Created new CardDefinitionCollectionStaticData at {CARD_DEFINITION_COLLECTION_PATH}");
                return collection;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to create CardDefinitionCollectionStaticData: {e.Message}");
                return null;
            }
        }
        
        private static void UpdateInvocationCollection()
        {
            var collection = AssetDatabase.LoadAssetAtPath<InvocationCollectionStaticData>(INVOCATION_COLLECTION_PATH);
            if (collection == null) return;
            
            // Находим все InvocationStaticData файлы
            string[] guids = AssetDatabase.FindAssets("t:InvocationStaticData");
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var invocationData = AssetDatabase.LoadAssetAtPath<InvocationStaticData>(path);
                
                if (invocationData != null)
                {
                    AddToInvocationCollection(invocationData);
                }
            }
        }
        
        private static void UpdateCardDefinitionCollection()
        {
            var collection = AssetDatabase.LoadAssetAtPath<CardDefinitionCollectionStaticData>(CARD_DEFINITION_COLLECTION_PATH);
            if (collection == null) return;
            
            if (collection.CardDefinitionStaticData == null)
            {
                collection.CardDefinitionStaticData = new Dictionary<CardDefinitionType, CardDefinitionStaticData>();
            }
            
            // Находим все CardDefinitionStaticData файлы
            string[] guids = AssetDatabase.FindAssets("t:CardDefinitionStaticData");
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(path);
                
                if (cardDefinition != null && cardDefinition.Type != CardDefinitionType.Unknown)
                {
                    collection.CardDefinitionStaticData[cardDefinition.Type] = cardDefinition;
                }
            }
            
            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();
        }
    }
}
