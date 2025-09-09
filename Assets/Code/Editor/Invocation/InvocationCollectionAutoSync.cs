using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Code.StaticData.Cards;
using Code.StaticData.Invocation.Data;

namespace Code.Editor.Invocation
{
    [InitializeOnLoad]
    public static class InvocationCollectionAutoSync
    {
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            InvocationStaticDataEditorWindow editorWindow = Resources.FindObjectsOfTypeAll<InvocationStaticDataEditorWindow>().FirstOrDefault();
            if (editorWindow == null) 
            {
                UpdateAllCardDefinitionsAfterReload();
                return;
            }
            
            editorWindow.UpdateCardDefinitionsAfterReload();
        }
        
        private static void UpdateAllCardDefinitionsAfterReload()
        {
            string[] cardGuids = AssetDatabase.FindAssets($"t:CardDefinitionStaticData");
            foreach (string guid in cardGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardDefinitionStaticData cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(path);
                
                if (cardDefinition != null && !string.IsNullOrEmpty(cardDefinition.Name))
                {
                    if (System.Enum.TryParse(cardDefinition.Name, out CardDefinitionType result))
                    {
                        if (cardDefinition.Type != result)
                        {
                            cardDefinition.Type = result;
                            EditorUtility.SetDirty(cardDefinition);
                        }
                        
                        CollectionUpdater.AddToCardDefinitionCollection(cardDefinition);
                    }
                }
            }
            
            string[] invocationGuids = AssetDatabase.FindAssets($"t:InvocationStaticData");
            foreach (string guid in invocationGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                InvocationStaticData invocationData = AssetDatabase.LoadAssetAtPath<InvocationStaticData>(path);
                
                if (invocationData != null && invocationData.CardDefinition == CardDefinitionType.Unknown)
                {
                    string[] cardGuids2 = AssetDatabase.FindAssets($"t:CardDefinitionStaticData");
                    foreach (string cardGuid in cardGuids2)
                    {
                        string cardPath = AssetDatabase.GUIDToAssetPath(cardGuid);
                        CardDefinitionStaticData cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(cardPath);
                        
                        if (cardDefinition != null && cardDefinition.Name == invocationData.Id)
                        {
                            if (System.Enum.TryParse<CardDefinitionType>(cardDefinition.Name, out CardDefinitionType result))
                            {
                                invocationData.CardDefinition = result;
                                EditorUtility.SetDirty(invocationData);
                                break;
                            }
                        }
                    }
                }
            }
            AssetDatabase.SaveAssets();
        }
    }
}