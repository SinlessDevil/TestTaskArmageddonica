using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Invocation
{
    public static class EnumUpdater
    {
        private const string CARD_DEFINITION_TYPE_PATH = "Assets/Code/StaticData/Cards/Definition/CardDefinitionType.cs";
        
        public static void AddCardDefinitionType(string cardName)
        {
            if (string.IsNullOrEmpty(cardName))
                return;
                
            // Проверяем, существует ли уже такое значение в enum
            if (CardDefinitionTypeExists(cardName))
                return;
                
            string enumFilePath = CARD_DEFINITION_TYPE_PATH;
            
            if (!File.Exists(enumFilePath))
            {
                Debug.LogError($"CardDefinitionType.cs not found at {enumFilePath}");
                return;
            }
            
            try
            {
                string content = File.ReadAllText(enumFilePath);
                string newEnumValue = $"        {cardName},";
                
                // Находим последнее значение enum и добавляем новое
                int lastCommaIndex = content.LastIndexOf(',');
                if (lastCommaIndex != -1)
                {
                    content = content.Insert(lastCommaIndex + 1, $"\n{newEnumValue}");
                }
                else
                {
                    // Если нет запятых, добавляем после открывающей скобки
                    int openBraceIndex = content.IndexOf('{');
                    if (openBraceIndex != -1)
                    {
                        content = content.Insert(openBraceIndex + 1, $"\n{newEnumValue}");
                    }
                }
                
                File.WriteAllText(enumFilePath, content);
                AssetDatabase.Refresh();
                
                // Принудительно обновляем компиляцию
                AssetDatabase.ImportAsset(enumFilePath);
                EditorUtility.RequestScriptReload();
                
                Debug.Log($"Added {cardName} to CardDefinitionType enum");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to update CardDefinitionType enum: {e.Message}");
            }
        }
        
        private static bool CardDefinitionTypeExists(string cardName)
        {
            string enumFilePath = CARD_DEFINITION_TYPE_PATH;
            
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
        
        public static void UpdateCardDefinitionTypeEnum()
        {
            // Находим все CardDefinitionStaticData файлы
            string[] guids = AssetDatabase.FindAssets("t:CardDefinitionStaticData");
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var cardDefinition = AssetDatabase.LoadAssetAtPath<Code.StaticData.Cards.CardDefinitionStaticData>(path);
                
                if (cardDefinition != null && !string.IsNullOrEmpty(cardDefinition.Name))
                {
                    AddCardDefinitionType(cardDefinition.Name);
                }
            }
        }
        
        public static bool IsCardDefinitionTypeExists(string cardName)
        {
            try
            {
                // Пытаемся найти значение через рефлексию
                var enumType = typeof(Code.StaticData.Cards.CardDefinitionType);
                var enumValues = System.Enum.GetNames(enumType);
                return System.Array.Exists(enumValues, name => name == cardName);
            }
            catch
            {
                return false;
            }
        }
    }
}
