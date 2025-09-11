using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace Code.Editor
{
    public class TextToTMPConverterWindow : EditorWindow
    {
        private GameObject _targetPrefab;
        private Vector2 _scrollPosition;
        private List<TextConversionInfo> _foundTexts = new();
        private List<TMPConversionInfo> _foundTMPTexts = new();
        private bool _copySettings = true;
        private bool _showPreview = true;
        private bool _includeInactive = true;
        private TMP_FontAsset _defaultTMPFont;
        private Font _defaultFont;
        private ConversionMode _conversionMode = ConversionMode.TextToTMP;

        private enum ConversionMode
        {
            TextToTMP,
            TMPToText
        }
        
        [System.Serializable]
        private class TextConversionInfo
        {
            public Text originalText;
            public GameObject gameObject;
            public string originalTextContent;
            public Font originalFont;
            public int originalFontSize;
            public Color originalColor;
            public TextAnchor originalAlignment;
            public bool isSelected = true;
        }
        
        [System.Serializable]
        private class TMPConversionInfo
        {
            public TMP_Text originalTMPText;
            public GameObject gameObject;
            public string originalTextContent;
            public TMP_FontAsset originalFont;
            public float originalFontSize;
            public Color originalColor;
            public TextAlignmentOptions originalAlignment;
            public bool isSelected = true;
        }

        [MenuItem("Tools/Text to TMP Converter")]
        public static void ShowWindow()
        {
            GetWindow<TextToTMPConverterWindow>("Text to TMP Converter");
        }

        private void OnGUI()
        {
            GUILayout.Label("Text ↔ TextMeshPro Converter", EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.Label("Target UI Prefab:", EditorStyles.label);
            _targetPrefab = (GameObject)EditorGUILayout.ObjectField(_targetPrefab, typeof(GameObject), false);
            
            if (_targetPrefab == null)
            {
                EditorGUILayout.HelpBox("Please select a UI prefab to work with.", MessageType.Warning);
                return;
            }

            GUILayout.Space(10);

            GUILayout.Label("Conversion Mode:", EditorStyles.boldLabel);
            _conversionMode = (ConversionMode)EditorGUILayout.EnumPopup("Convert:", _conversionMode);
            
            GUILayout.Space(10);

            GUILayout.Label("Conversion Settings:", EditorStyles.boldLabel);
            _copySettings = EditorGUILayout.Toggle("Copy Settings", _copySettings);
            _showPreview = EditorGUILayout.Toggle("Show Preview", _showPreview);
            _includeInactive = EditorGUILayout.Toggle("Include Inactive Objects", _includeInactive);
            
            GUILayout.Space(5);
            
            if (_conversionMode == ConversionMode.TextToTMP)
            {
                GUILayout.Label("Default TMP Font (if no font found):", EditorStyles.miniLabel);
                _defaultTMPFont = (TMP_FontAsset)EditorGUILayout.ObjectField(_defaultTMPFont, typeof(TMP_FontAsset), false);
            }
            else
            {
                GUILayout.Label("Default Font (if no font found):", EditorStyles.miniLabel);
                _defaultFont = (Font)EditorGUILayout.ObjectField(_defaultFont, typeof(Font), false);
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(_conversionMode == ConversionMode.TextToTMP ? "Find Text Components" : "Find TMP Components"))
            {
                if (_conversionMode == ConversionMode.TextToTMP)
                    FindTextComponents();
                else
                    FindTMPComponents();
            }
            
            if (GUILayout.Button("Clear List"))
            {
                _foundTexts.Clear();
                _foundTMPTexts.Clear();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (_conversionMode == ConversionMode.TextToTMP && _foundTexts.Count > 0)
            {
                GUILayout.Label($"Found {_foundTexts.Count} Text components:", EditorStyles.boldLabel);
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(250));
                
                foreach (var textInfo in _foundTexts)
                {
                    if (textInfo.gameObject == null) continue;
                    
                    EditorGUILayout.BeginHorizontal();
                    
                    textInfo.isSelected = EditorGUILayout.Toggle(textInfo.isSelected, GUILayout.Width(20));
                    
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField($"Object: {textInfo.gameObject.name}", EditorStyles.miniLabel);
                    
                    if (_showPreview)
                    {
                        EditorGUILayout.LabelField($"Text: \"{textInfo.originalTextContent}\"", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"Font: {textInfo.originalFont?.name ?? "None"}", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"Size: {textInfo.originalFontSize}", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"Color: {textInfo.originalColor}", EditorStyles.miniLabel);
                    }
                    
                    EditorGUILayout.EndVertical();
                    
                    if (GUILayout.Button("Select", GUILayout.Width(50)))
                    {
                        Selection.activeGameObject = textInfo.gameObject;
                        EditorGUIUtility.PingObject(textInfo.gameObject);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }
                
                EditorGUILayout.EndScrollView();
                
                GUILayout.Space(10);
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Select All"))
                    foreach (var textInfo in _foundTexts)
                        textInfo.isSelected = true;
                
                if (GUILayout.Button("Deselect All"))
                    foreach (var textInfo in _foundTexts)
                        textInfo.isSelected = false;
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(10);
                
                if (GUILayout.Button("Convert Selected to TextMeshPro", GUILayout.Height(30))) 
                    ConvertSelectedTexts();
            }
            else if (_conversionMode == ConversionMode.TMPToText && _foundTMPTexts.Count > 0)
            {
                GUILayout.Label($"Found {_foundTMPTexts.Count} TMP_Text components:", EditorStyles.boldLabel);
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(250));
                
                foreach (var tmpInfo in _foundTMPTexts)
                {
                    if (tmpInfo.gameObject == null) continue;
                    
                    EditorGUILayout.BeginHorizontal();
                    
                    tmpInfo.isSelected = EditorGUILayout.Toggle(tmpInfo.isSelected, GUILayout.Width(20));
                    
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField($"Object: {tmpInfo.gameObject.name}", EditorStyles.miniLabel);
                    
                    if (_showPreview)
                    {
                        EditorGUILayout.LabelField($"Text: \"{tmpInfo.originalTextContent}\"", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"Font: {tmpInfo.originalFont?.name ?? "None"}", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"Size: {tmpInfo.originalFontSize}", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"Color: {tmpInfo.originalColor}", EditorStyles.miniLabel);
                    }
                    
                    EditorGUILayout.EndVertical();
                    
                    if (GUILayout.Button("Select", GUILayout.Width(50)))
                    {
                        Selection.activeGameObject = tmpInfo.gameObject;
                        EditorGUIUtility.PingObject(tmpInfo.gameObject);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }
                
                EditorGUILayout.EndScrollView();
                
                GUILayout.Space(10);
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Select All"))
                    foreach (var tmpInfo in _foundTMPTexts)
                        tmpInfo.isSelected = true;
                
                if (GUILayout.Button("Deselect All"))
                    foreach (var tmpInfo in _foundTMPTexts)
                        tmpInfo.isSelected = false;
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(10);
                
                if (GUILayout.Button("Convert Selected to Text", GUILayout.Height(30)))
                {
                    ConvertSelectedTMPTexts();
                }
            }
            else
            {
                string componentType = _conversionMode == ConversionMode.TextToTMP ? "Text" : "TMP_Text";
                EditorGUILayout.HelpBox($"No {componentType} components found. Click 'Find {componentType} Components' to search.", MessageType.Info);
            }
        }

        private void FindTextComponents()
        {
            _foundTexts.Clear();
            
            if (_targetPrefab == null) 
                return;

            Text[] textComponents = _targetPrefab.GetComponentsInChildren<Text>(_includeInactive);
            
            foreach (var textComponent in textComponents)
            {
                var textInfo = new TextConversionInfo
                {
                    originalText = textComponent,
                    gameObject = textComponent.gameObject,
                    originalTextContent = textComponent.text,
                    originalFont = textComponent.font,
                    originalFontSize = textComponent.fontSize,
                    originalColor = textComponent.color,
                    originalAlignment = textComponent.alignment,
                    isSelected = true
                };
                
                _foundTexts.Add(textInfo);
            }
            
            Debug.Log($"Found {_foundTexts.Count} Text components in {_targetPrefab.name}");
        }
        
        private void FindTMPComponents()
        {
            _foundTMPTexts.Clear();
            
            if (_targetPrefab == null) 
                return;

            TMP_Text[] tmpComponents = _targetPrefab.GetComponentsInChildren<TMP_Text>(_includeInactive);
            
            foreach (var tmpComponent in tmpComponents)
            {
                var tmpInfo = new TMPConversionInfo
                {
                    originalTMPText = tmpComponent,
                    gameObject = tmpComponent.gameObject,
                    originalTextContent = tmpComponent.text,
                    originalFont = tmpComponent.font,
                    originalFontSize = tmpComponent.fontSize,
                    originalColor = tmpComponent.color,
                    originalAlignment = tmpComponent.alignment,
                    isSelected = true
                };
                
                _foundTMPTexts.Add(tmpInfo);
            }
            
            Debug.Log($"Found {_foundTMPTexts.Count} TMP_Text components in {_targetPrefab.name}");
        }

        private void ConvertSelectedTexts()
        {
            int convertedCount = 0;
            List<Object> modifiedObjects = new List<Object>();

            // Создаем копию списка для безопасной итерации
            var textsToConvert = _foundTexts.Where(t => t.isSelected && t.originalText != null && t.gameObject != null).ToList();

            foreach (var textInfo in textsToConvert)
            {
                // Сохраняем данные перед уничтожением
                string textContent = textInfo.originalTextContent;
                int fontSize = textInfo.originalFontSize;
                Color color = textInfo.originalColor;
                TextAnchor alignment = textInfo.originalAlignment;
                Font originalFont = textInfo.originalFont;
                GameObject gameObject = textInfo.gameObject;

                // Уничтожаем старый компонент
                if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
                    DestroyImmediate(textInfo.originalText, true);
                else
                    DestroyImmediate(textInfo.originalText);

                // Добавляем новый компонент
                TextMeshProUGUI tmpText = gameObject.AddComponent<TextMeshProUGUI>();
                
                if (_copySettings)
                {
                    tmpText.text = textContent;
                    tmpText.fontSize = fontSize;
                    tmpText.color = color;
                    tmpText.alignment = ConvertTextAnchorToTextAlignmentOptions(alignment);
                    
                    TMP_FontAsset tmpFont = FindTMPFontForFont(originalFont);
                    if (tmpFont != null)
                        tmpText.font = tmpFont;
                    else if (_defaultTMPFont != null) 
                        tmpText.font = _defaultTMPFont;
                }
                else
                {
                    tmpText.text = textContent;
                    if (_defaultTMPFont != null) 
                        tmpText.font = _defaultTMPFont;
                }

                if (!modifiedObjects.Contains(gameObject)) 
                    modifiedObjects.Add(gameObject);

                convertedCount++;
            }

            // Очищаем список после конвертации
            _foundTexts.Clear();

            foreach (Object obj in modifiedObjects)
            {
                EditorUtility.SetDirty(obj);

                if (!PrefabUtility.IsPartOfPrefabAsset(obj)) 
                    continue;
                
                GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(obj as GameObject);
                if (prefabRoot != null)
                    PrefabUtility.SavePrefabAsset(prefabRoot);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully converted {convertedCount} Text components to TextMeshPro");
            EditorUtility.DisplayDialog("Success", $"Converted {convertedCount} Text components to TextMeshPro!", "OK");
        }
        
        private void ConvertSelectedTMPTexts()
        {
            int convertedCount = 0;
            List<Object> modifiedObjects = new List<Object>();

            // Создаем копию списка для безопасной итерации
            var tmpTextsToConvert = _foundTMPTexts.Where(t => t.isSelected && t.originalTMPText != null && t.gameObject != null).ToList();

            foreach (var tmpInfo in tmpTextsToConvert)
            {
                // Сохраняем данные перед уничтожением
                string textContent = tmpInfo.originalTextContent;
                float fontSize = tmpInfo.originalFontSize;
                Color color = tmpInfo.originalColor;
                TextAlignmentOptions alignment = tmpInfo.originalAlignment;
                TMP_FontAsset originalFont = tmpInfo.originalFont;
                GameObject gameObject = tmpInfo.gameObject;

                // Уничтожаем старый компонент
                if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
                    DestroyImmediate(tmpInfo.originalTMPText, true);
                else
                    DestroyImmediate(tmpInfo.originalTMPText);

                // Добавляем новый компонент
                Text text = gameObject.AddComponent<Text>();
                
                if (_copySettings)
                {
                    text.text = textContent;
                    text.fontSize = Mathf.RoundToInt(fontSize);
                    text.color = color;
                    text.alignment = ConvertTextAlignmentOptionsToTextAnchor(alignment);
                    
                    Font regularFont = FindFontForTMPFont(originalFont);
                    if (regularFont != null)
                        text.font = regularFont;
                    else if (_defaultFont != null) 
                        text.font = _defaultFont;
                }
                else
                {
                    text.text = textContent;
                    if (_defaultFont != null) 
                        text.font = _defaultFont;
                }

                if (!modifiedObjects.Contains(gameObject)) 
                    modifiedObjects.Add(gameObject);

                convertedCount++;
            }

            // Очищаем список после конвертации
            _foundTMPTexts.Clear();

            foreach (Object obj in modifiedObjects)
            {
                EditorUtility.SetDirty(obj);

                if (!PrefabUtility.IsPartOfPrefabAsset(obj)) 
                    continue;
                
                GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(obj as GameObject);
                if (prefabRoot != null)
                    PrefabUtility.SavePrefabAsset(prefabRoot);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully converted {convertedCount} TMP_Text components to Text");
            EditorUtility.DisplayDialog("Success", $"Converted {convertedCount} TMP_Text components to Text!", "OK");
        }

        private TMP_FontAsset FindTMPFontForFont(Font originalFont)
        {
            if (originalFont == null) 
                return null;

            string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
            foreach (string guid in guids)
            {
                TMP_FontAsset tmpFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(AssetDatabase.GUIDToAssetPath(guid));
                if (tmpFont != null && tmpFont.name.ToLower().Contains(originalFont.name.ToLower()))
                    return tmpFont;
            }

            return null;
        }

        private TextAlignmentOptions ConvertTextAnchorToTextAlignmentOptions(TextAnchor textAnchor)
        {
            switch (textAnchor)
            {
                case TextAnchor.UpperLeft: return TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperCenter: return TextAlignmentOptions.Top;
                case TextAnchor.UpperRight: return TextAlignmentOptions.TopRight;
                case TextAnchor.MiddleLeft: return TextAlignmentOptions.Left;
                case TextAnchor.MiddleCenter: return TextAlignmentOptions.Center;
                case TextAnchor.MiddleRight: return TextAlignmentOptions.Right;
                case TextAnchor.LowerLeft: return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerCenter: return TextAlignmentOptions.Bottom;
                case TextAnchor.LowerRight: return TextAlignmentOptions.BottomRight;
                default: return TextAlignmentOptions.Center;
            }
        }
        
        private TextAnchor ConvertTextAlignmentOptionsToTextAnchor(TextAlignmentOptions alignment)
        {
            switch (alignment)
            {
                case TextAlignmentOptions.TopLeft: return TextAnchor.UpperLeft;
                case TextAlignmentOptions.Top: return TextAnchor.UpperCenter;
                case TextAlignmentOptions.TopRight: return TextAnchor.UpperRight;
                case TextAlignmentOptions.Left: return TextAnchor.MiddleLeft;
                case TextAlignmentOptions.Center: return TextAnchor.MiddleCenter;
                case TextAlignmentOptions.Right: return TextAnchor.MiddleRight;
                case TextAlignmentOptions.BottomLeft: return TextAnchor.LowerLeft;
                case TextAlignmentOptions.Bottom: return TextAnchor.LowerCenter;
                case TextAlignmentOptions.BottomRight: return TextAnchor.LowerRight;
                default: return TextAnchor.MiddleCenter;
            }
        }
        
        private Font FindFontForTMPFont(TMP_FontAsset tmpFont)
        {
            if (tmpFont == null) 
                return null;
            
            string[] guids = AssetDatabase.FindAssets("t:Font");
            return guids.Select(guid => AssetDatabase
                .LoadAssetAtPath<Font>(AssetDatabase
                    .GUIDToAssetPath(guid)))
                .FirstOrDefault(font => font != null && font.name.ToLower()
                    .Contains(tmpFont.name.ToLower()));
        }
    }
}
