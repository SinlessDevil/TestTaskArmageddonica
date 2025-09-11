using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace Code.Editor
{
    public class TMPTextReplacer : MonoBehaviour
    {
        [Header("TMP Text Replacer Tool")]
        [SerializeField] public string _newText = "";
        [SerializeField] public bool _copyRectAndSize = true;
        [SerializeField] public bool _showPreview = true;
        [SerializeField] public bool _includeInactive = true;
        
        [Header("Found TMP Texts")]
        [SerializeField] public List<TMPTextInfo> _foundTexts = new List<TMPTextInfo>();
        
        [System.Serializable]
        public class TMPTextInfo
        {
            public TMP_Text textComponent;
            public string originalText;
            public RectTransform rectTransform;
            public Vector2 originalSize;
            public Vector2 originalAnchoredPosition;
            public Vector2 originalSizeDelta;
            public bool isSelected = true;
        }
    }

    [CustomEditor(typeof(TMPTextReplacer))]
    public class TMPTextReplacerEditor : UnityEditor.Editor
    {
        private TMPTextReplacer _target;
        private Vector2 _scrollPosition;

        private void OnEnable()
        {
            _target = (TMPTextReplacer)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("TMP Text Replacer Tool", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Select objects in scene or prefab to replace TMP_Text components", MessageType.Info);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Settings:", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_newText"), new GUIContent("New Text:"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_copyRectAndSize"), new GUIContent("Copy Rect & Size"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_showPreview"), new GUIContent("Show Preview"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_includeInactive"), new GUIContent("Include Inactive Objects"));
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Find TMP Texts in Selection")) 
                FindTMPTextsInSelection();
            
            if (GUILayout.Button("Find TMP Texts in Children")) 
                FindTMPTextsInChildren();
            
            if (GUILayout.Button("Clear List"))
            {
                _target._foundTexts.Clear();
                EditorUtility.SetDirty(_target);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            if (_target._foundTexts.Count > 0)
            {
                EditorGUILayout.LabelField($"Found {_target._foundTexts.Count} TMP_Text components:", EditorStyles.boldLabel);
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(200));
                
                foreach (var textInfo in _target._foundTexts)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    textInfo.isSelected = EditorGUILayout.Toggle(textInfo.isSelected, GUILayout.Width(20));
                    
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField($"Object: {textInfo.textComponent.gameObject.name}", EditorStyles.miniLabel);
                    
                    if (_target._showPreview)
                    {
                        EditorGUILayout.LabelField($"Current: \"{textInfo.originalText}\"", EditorStyles.miniLabel);
                        EditorGUILayout.LabelField($"New: \"{_target._newText}\"", EditorStyles.miniLabel);
                        
                        if (_target._copyRectAndSize)
                        {
                            EditorGUILayout.LabelField($"Size: {textInfo.originalSize}", EditorStyles.miniLabel);
                        }
                    }
                    
                    EditorGUILayout.EndVertical();
                    
                    if (GUILayout.Button("Select", GUILayout.Width(50)))
                    {
                        Selection.activeGameObject = textInfo.textComponent.gameObject;
                        EditorGUIUtility.PingObject(textInfo.textComponent.gameObject);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }
                
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Space();
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Select All"))
                {
                    foreach (var textInfo in _target._foundTexts)
                        textInfo.isSelected = true;
                    
                    EditorUtility.SetDirty(_target);
                }
                
                if (GUILayout.Button("Deselect All"))
                {
                    foreach (var textInfo in _target._foundTexts)
                        textInfo.isSelected = false;
                    
                    EditorUtility.SetDirty(_target);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space();
                
                GUI.enabled = !string.IsNullOrEmpty(_target._newText);
                if (GUILayout.Button("Replace Selected Texts", GUILayout.Height(30))) 
                    ReplaceSelectedTexts();
                
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.HelpBox("No TMP_Text components found. Select objects and click 'Find TMP Texts' to search.", MessageType.Info);
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void FindTMPTextsInSelection()
        {
            _target._foundTexts.Clear();
            
            if (Selection.gameObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select objects in the scene first!", "OK");
                return;
            }
            
            foreach (var selectedObject in Selection.gameObjects)
            {
                TMP_Text[] textComponents = selectedObject.GetComponentsInChildren<TMP_Text>(_target._includeInactive);
                
                foreach (var textComponent in textComponents)
                {
                    var textInfo = new TMPTextReplacer.TMPTextInfo
                    {
                        textComponent = textComponent,
                        originalText = textComponent.text,
                        rectTransform = textComponent.GetComponent<RectTransform>(),
                        originalSize = textComponent.GetComponent<RectTransform>().sizeDelta,
                        originalAnchoredPosition = textComponent.GetComponent<RectTransform>().anchoredPosition,
                        originalSizeDelta = textComponent.GetComponent<RectTransform>().sizeDelta,
                        isSelected = true
                    };
                    
                    _target._foundTexts.Add(textInfo);
                }
            }
            
            EditorUtility.SetDirty(_target);
            Debug.Log($"Found {_target._foundTexts.Count} TMP_Text components in selected objects");
        }
        
        private void FindTMPTextsInChildren()
        {
            _target._foundTexts.Clear();
            
            if (Selection.gameObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select objects in the scene first!", "OK");
                return;
            }
            
            foreach (var selectedObject in Selection.gameObjects)
            {
                TMP_Text[] textComponents = selectedObject.GetComponentsInChildren<TMP_Text>(_target._includeInactive);
                
                foreach (var textComponent in textComponents)
                {
                    var textInfo = new TMPTextReplacer.TMPTextInfo
                    {
                        textComponent = textComponent,
                        originalText = textComponent.text,
                        rectTransform = textComponent.GetComponent<RectTransform>(),
                        originalSize = textComponent.GetComponent<RectTransform>().sizeDelta,
                        originalAnchoredPosition = textComponent.GetComponent<RectTransform>().anchoredPosition,
                        originalSizeDelta = textComponent.GetComponent<RectTransform>().sizeDelta,
                        isSelected = true
                    };
                    
                    _target._foundTexts.Add(textInfo);
                }
            }
            
            EditorUtility.SetDirty(_target);
            Debug.Log($"Found {_target._foundTexts.Count} TMP_Text components in children of selected objects");
        }

        private void ReplaceSelectedTexts()
        {
            if (string.IsNullOrEmpty(_target._newText))
            {
                EditorUtility.DisplayDialog("Error", "Please enter new text!", "OK");
                return;
            }

            int replacedCount = 0;
            List<Object> modifiedObjects = new List<Object>();

            foreach (var textInfo in _target._foundTexts)
            {
                if (!textInfo.isSelected) 
                    continue;
                
                textInfo.textComponent.text = _target._newText;
                
                if (_target._copyRectAndSize && textInfo.rectTransform != null)
                {
                    textInfo.rectTransform.sizeDelta = textInfo.originalSizeDelta;
                    textInfo.rectTransform.anchoredPosition = textInfo.originalAnchoredPosition;
                }
                
                if (!modifiedObjects.Contains(textInfo.textComponent.gameObject)) 
                    modifiedObjects.Add(textInfo.textComponent.gameObject);
                
                replacedCount++;
            }
            
            foreach (Object obj in modifiedObjects)
            {
                EditorUtility.SetDirty(obj);
                
                if (PrefabUtility.IsPartOfPrefabAsset(obj)) 
                {
                    GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(obj as GameObject);
                    if (prefabRoot != null)
                        PrefabUtility.SavePrefabAsset(prefabRoot);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Successfully replaced {replacedCount} TMP_Text components with text: \"{_target._newText}\"");
            EditorUtility.DisplayDialog("Success", $"Replaced {replacedCount} TMP_Text components!", "OK");
        }
    }
}
