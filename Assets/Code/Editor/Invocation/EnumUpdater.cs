using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Invocation
{
    public static class EnumUpdater
    {
        private const string CardDefinitionTypePath = "Assets/Code/StaticData/Cards/Definition/CardDefinitionType.cs";
        
        public static void AddCardDefinitionType(string cardName)
        {
            if (string.IsNullOrEmpty(cardName))
                return;
            
            if (CardDefinitionTypeExists(cardName))
                return;
                
            string enumFilePath = CardDefinitionTypePath;
            
            if (!File.Exists(enumFilePath))
            {
                Debug.LogError($"CardDefinitionType.cs not found at {enumFilePath}");
                return;
            }
            
            try
            {
                string content = File.ReadAllText(enumFilePath);
                string newEnumValue = $"        {cardName},";
                
                int lastCommaIndex = content.LastIndexOf(',');
                if (lastCommaIndex != -1)
                {
                    content = content.Insert(lastCommaIndex + 1, $"\n{newEnumValue}");
                }
                else
                {
                    int openBraceIndex = content.IndexOf('{');
                    if (openBraceIndex != -1)
                    {
                        content = content.Insert(openBraceIndex + 1, $"\n{newEnumValue}");
                    }
                }
                
                File.WriteAllText(enumFilePath, content);
                
                AssetDatabase.ImportAsset(enumFilePath);
                AssetDatabase.Refresh();
                EditorUtility.RequestScriptReload();
                
                Debug.Log($"Added {cardName} to CardDefinitionType enum and requested script reload");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to update CardDefinitionType enum: {e.Message}");
            }
        }
        
        private static bool CardDefinitionTypeExists(string cardName)
        {
            string enumFilePath = CardDefinitionTypePath;
            
            if (!File.Exists(enumFilePath))
                return false;
                
            try
            {
                string content = File.ReadAllText(enumFilePath);
                return content.Contains(cardName);
            }
            catch
            {
                return false;
            }
        }
        
        public static bool IsCardDefinitionTypeExists(string cardName)
        {
            try
            {
                Type enumType = typeof(StaticData.Cards.CardDefinitionType);
                string[] enumValues = Enum.GetNames(enumType);
                return Array.Exists(enumValues, name => name == cardName);
            }
            catch
            {
                return false;
            }
        }
    }
}
