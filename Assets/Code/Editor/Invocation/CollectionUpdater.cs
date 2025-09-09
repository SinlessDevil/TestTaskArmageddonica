using System;
using System.Collections.Generic;
using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Invocation
{
    public static class CollectionUpdater
    {
        private const string InvocationCollectionPath = "Assets/Resources/StaticData/Invocation/InvocationCollectionStaticData.asset";
        private const string CardDefinitionCollectionPath = "Assets/Resources/StaticData/Cards/CardDefinitionCollectionStaticData.asset";
        
        public static void AddToInvocationCollection(InvocationStaticData invocationData)
        {
            InvocationCollectionStaticData collection = AssetDatabase.LoadAssetAtPath<InvocationCollectionStaticData>(InvocationCollectionPath);
            if (collection == null)
            {
                Debug.LogError($"InvocationCollectionStaticData not found at {InvocationCollectionPath}");
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
                case InvocationType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            if (cardDefinition.Type == CardDefinitionType.Unknown)
                return;
            
            CardDefinitionCollectionStaticData collection = AssetDatabase.LoadAssetAtPath<CardDefinitionCollectionStaticData>(CardDefinitionCollectionPath);
            if (collection == null)
            {
                collection = CreateCardDefinitionCollectionStaticData();
                if (collection == null)
                    return;
            }
            
            if (collection.CardDefinitionStaticData == null) 
                collection.CardDefinitionStaticData = new Dictionary<CardDefinitionType, CardDefinitionStaticData>();
            
            collection.CardDefinitionStaticData[cardDefinition.Type] = cardDefinition;
            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();
        }
        
        private static CardDefinitionCollectionStaticData CreateCardDefinitionCollectionStaticData()
        {
            try
            {
                CardDefinitionCollectionStaticData collection = ScriptableObject.CreateInstance<CardDefinitionCollectionStaticData>();
                collection.CardDefinitionStaticData = new Dictionary<CardDefinitionType, CardDefinitionStaticData>();
                string folderPath = "Assets/Resources/StaticData/Cards/";
                if (!System.IO.Directory.Exists(folderPath)) 
                    System.IO.Directory.CreateDirectory(folderPath);
                
                AssetDatabase.CreateAsset(collection, CardDefinitionCollectionPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                Debug.Log($"Created new CardDefinitionCollectionStaticData at {CardDefinitionCollectionPath}");
                return collection;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create CardDefinitionCollectionStaticData: {e.Message}");
                return null;
            }
        }
    }
}
