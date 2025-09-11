using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Code.StaticData.Battle;
using Code.StaticData.Invocation;

namespace Code.Editor.Battle
{
    [CustomEditor(typeof(BattleStaticData))]
    public class BattleStaticDataEditor : UnityEditor.Editor
    {
        private BattleStaticData _target;
        private Vector2 _scrollPosition;
        private int _selectedBattleIndex = -1;
        private List<string> _availableInvocationIds = new();
        
        private void OnEnable()
        {
            _target = (BattleStaticData)target;
            LoadAvailableInvocationIds();
            
            // Генерируем ID для битв, у которых ID = 0, и инициализируем матрицы
            bool needsSave = false;
            foreach (var battleData in _target.BattleDataList)
            {
                if (battleData.BattleId == 0)
                {
                    battleData.GenerateBattleId();
                    needsSave = true;
                }
                
                // Проверяем и инициализируем матрицу если нужно
                if (battleData.BattleMatrix == null)
                {
                    battleData.InitializeMatrix();
                    needsSave = true;
                }
            }
            
            if (needsSave)
            {
                EditorUtility.SetDirty(_target);
            }
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Battle Static Data", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Manage battle data with matrix layouts for units and buildings", MessageType.Info);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New Battle")) 
                AddNewBattle();
            
            if (GUILayout.Button("Refresh Invocation IDs")) 
                LoadAvailableInvocationIds();
            
            if (GUILayout.Button("Force Save")) 
            {
                Debug.Log($"Before save - BattleDataList count: {_target.BattleDataList.Count}");
                foreach (var battle in _target.BattleDataList)
                {
                    Debug.Log($"Battle: {battle.BattleName}, ID: {battle.BattleId}, Matrix: {battle.MatrixWidth}x{battle.MatrixHeight}");
                }
                
                EditorUtility.SetDirty(_target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                Debug.Log("BattleStaticData saved and refreshed!");
                
                // Проверяем после сохранения
                Debug.Log($"After save - BattleDataList count: {_target.BattleDataList.Count}");
            }
            
            if (GUILayout.Button("Debug Data")) 
            {
                Debug.Log("=== DEBUG BATTLE DATA ===");
                Debug.Log($"Target: {_target.name}");
                Debug.Log($"BattleDataList Count: {_target.BattleDataList.Count}");
                Debug.Log($"BattleDataList is null: {_target.BattleDataList == null}");
                
                for (int i = 0; i < _target.BattleDataList.Count; i++)
                {
                    var battle = _target.BattleDataList[i];
                    Debug.Log($"Battle {i}: {battle?.BattleName ?? "NULL"}, ID: {battle?.BattleId ?? -1}");
                }
                Debug.Log("=== END DEBUG ===");
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            if (_target.BattleDataList.Count > 0)
            {
                EditorGUILayout.LabelField($"Battle Data List ({_target.BattleDataList.Count}):", EditorStyles.boldLabel);
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(200));
                
                for (int i = 0; i < _target.BattleDataList.Count; i++)
                {
                    BattleData battleData = _target.BattleDataList[i];
                    
                    EditorGUILayout.BeginHorizontal();
                    
                    if (GUILayout.Button(_selectedBattleIndex == i ? "●" : "○", GUILayout.Width(20)))
                        _selectedBattleIndex = i;
                    
                    string newName = EditorGUILayout.TextField(battleData.BattleName, GUILayout.Width(120));
                    if (newName != battleData.BattleName)
                    {
                        battleData._battleName = newName;
                        EditorUtility.SetDirty(_target);
                    }
                    
                    EditorGUILayout.LabelField($"{battleData.MatrixWidth}x{battleData.MatrixHeight}", EditorStyles.miniLabel, GUILayout.Width(40));
                    
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("✕", GUILayout.Width(25), GUILayout.Height(20)))
                    {
                        if (EditorUtility.DisplayDialog("Delete Battle", 
                            $"Are you sure you want to delete '{battleData.BattleName}'?", "Yes", "No"))
                        {
                            _target.RemoveBattleData(battleData);
                            
                            if (_selectedBattleIndex == i)
                                _selectedBattleIndex = -1;
                            else if (_selectedBattleIndex > i)
                                _selectedBattleIndex--;
                            
                            // Принудительно сохраняем изменения
                            EditorUtility.SetDirty(_target);
                            AssetDatabase.SaveAssets();
                        }
                    }
                    GUI.backgroundColor = Color.white;
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }
                
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Space();
                
                if (_selectedBattleIndex >= 0 && _selectedBattleIndex < _target.BattleDataList.Count)
                    DisplaySelectedBattle(_target.BattleDataList[_selectedBattleIndex]);
            }
            else
            {
                EditorGUILayout.HelpBox("No battle data found. Click 'Add New Battle' to create one.", MessageType.Info);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void AddNewBattle()
        {
            Debug.Log($"Adding new battle. Current count: {_target.BattleDataList.Count}");
            
            // Создаем новую битву напрямую через SerializedObject
            serializedObject.Update();
            SerializedProperty battleListProperty = serializedObject.FindProperty("_battleDataList");
            battleListProperty.arraySize++;
            SerializedProperty newBattleProperty = battleListProperty.GetArrayElementAtIndex(battleListProperty.arraySize - 1);
            
            // Устанавливаем все свойства через SerializedProperty
            newBattleProperty.FindPropertyRelative("_battleName").stringValue = $"Battle_{battleListProperty.arraySize}";
            newBattleProperty.FindPropertyRelative("_battleId").intValue = UnityEngine.Random.Range(1000, 9999);
            newBattleProperty.FindPropertyRelative("_matrixWidth").intValue = 5;
            newBattleProperty.FindPropertyRelative("_matrixHeight").intValue = 5;
            
            // Инициализируем матрицу как одномерный массив
            SerializedProperty matrixProperty = newBattleProperty.FindPropertyRelative("_battleMatrix");
            matrixProperty.arraySize = 5 * 5; // 5x5 матрица = 25 элементов
            
            for (int i = 0; i < matrixProperty.arraySize; i++)
            {
                SerializedProperty cellProperty = matrixProperty.GetArrayElementAtIndex(i);
                cellProperty.FindPropertyRelative("_invocationId").stringValue = "";
                cellProperty.FindPropertyRelative("_isOccupied").boolValue = false;
            }
            
            serializedObject.ApplyModifiedProperties();
            
            _selectedBattleIndex = _target.BattleDataList.Count - 1;
            
            Debug.Log($"After adding. New count: {_target.BattleDataList.Count}");
            
            // Принудительно помечаем как измененный и сохраняем
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
            
            Debug.Log("New battle added and saved!");
        }
        
        private void LoadAvailableInvocationIds()
        {
            _availableInvocationIds.Clear();
            
            string[] guids = AssetDatabase.FindAssets("t:InvocationCollectionStaticData");
            foreach (string guid in guids)
            {
                InvocationCollectionStaticData collectionData = AssetDatabase
                    .LoadAssetAtPath<InvocationCollectionStaticData>(AssetDatabase.GUIDToAssetPath(guid));
                
                if (collectionData == null) 
                    continue;
                
                if (collectionData.BuildCollectionData != null)
                    foreach (var build in collectionData.BuildCollectionData.Where(build => !string.IsNullOrEmpty(build.Id) && !_availableInvocationIds.Contains(build.Id)))
                        _availableInvocationIds.Add(build.Id);
                    
                if (collectionData.SkillCollectionData != null)
                    foreach (var skill in collectionData.SkillCollectionData.Where(skill => !string.IsNullOrEmpty(skill.Id) && !_availableInvocationIds.Contains(skill.Id)))
                        _availableInvocationIds.Add(skill.Id);

                if (collectionData.UnitCollectionData == null) 
                    continue;
                
                foreach (var unit in collectionData.UnitCollectionData
                             .Where(unit => !string.IsNullOrEmpty(unit.Id) && !_availableInvocationIds.Contains(unit.Id)))
                {
                    _availableInvocationIds.Add(unit.Id);
                }
            }
            
            _availableInvocationIds.Sort();
        }
        
        private void DisplaySelectedBattle(BattleData battleData)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Selected Battle: {battleData.BattleName}", EditorStyles.boldLabel);
            
            if (battleData.BattleMatrix == null)
            {
                EditorGUILayout.HelpBox("Matrix is null, initializing...", MessageType.Warning);
                battleData.InitializeMatrix();
                EditorUtility.SetDirty(_target);
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Matrix Size:", GUILayout.Width(80));
            
            int newWidth = EditorGUILayout.IntField(battleData.MatrixWidth, GUILayout.Width(50));
            EditorGUILayout.LabelField("x", GUILayout.Width(10));
            int newHeight = EditorGUILayout.IntField(battleData.MatrixHeight, GUILayout.Width(50));
            
            if (newWidth != battleData.MatrixWidth || newHeight != battleData.MatrixHeight)
            {
                if (newWidth > 0 && newHeight > 0)
                {
                    battleData.ResizeMatrix(newWidth, newHeight);
                    EditorUtility.SetDirty(_target);
                    AssetDatabase.SaveAssets();
                }
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Battle Matrix:", EditorStyles.boldLabel);
            
            BattleMatrixCell[] matrix = battleData.BattleMatrix;
            
            if (matrix != null)
            {
                // Проверяем соответствие размеров матрицы
                int expectedSize = battleData.MatrixWidth * battleData.MatrixHeight;
                if (matrix.Length != expectedSize)
                {
                    EditorGUILayout.HelpBox($"Matrix size mismatch! Expected: {expectedSize} cells ({battleData.MatrixWidth}x{battleData.MatrixHeight}), Actual: {matrix.Length} cells. Reinitializing...", MessageType.Warning);
                    battleData.InitializeMatrix();
                    EditorUtility.SetDirty(_target);
                    matrix = battleData.BattleMatrix;
                }
                
                EditorGUILayout.BeginVertical("box");
                
                for (int y = battleData.MatrixHeight - 1; y >= 0; y--)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    for (int x = 0; x < battleData.MatrixWidth; x++)
                    {
                        int index = y * battleData.MatrixWidth + x;
                        
                        // Проверяем границы массива
                        if (index < matrix.Length)
                        {
                            var cell = matrix[index];
                            if (cell == null)
                            {
                                cell = new BattleMatrixCell();
                                matrix[index] = cell;
                            }
                            DisplayMatrixCell(x, y, cell, battleData);
                        }
                        else
                        {
                            // Если выходим за границы, создаем новую ячейку
                            var cell = new BattleMatrixCell();
                            DisplayMatrixCell(x, y, cell, battleData);
                        }
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
                
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("Matrix is still null after initialization attempt", MessageType.Error);
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField($"Available Invocation IDs ({_availableInvocationIds.Count}):", EditorStyles.boldLabel);
            
            if (_availableInvocationIds.Count > 0)
                EditorGUILayout.HelpBox($"IDs: {string.Join(", ", _availableInvocationIds)}", MessageType.Info);
            else
                EditorGUILayout.HelpBox("No InvocationStaticData found. Create InvocationStaticData assets first.",
                    MessageType.Warning);
        }
        
        private void DisplayMatrixCell(int x, int y, BattleMatrixCell cell, BattleData battleData)
        {
            Color originalColor = GUI.backgroundColor;

            GUI.backgroundColor = cell.IsOccupied ? Color.green : Color.white;

            string buttonText = cell.IsOccupied ? cell.InvocationId : "";
            
            if (GUILayout.Button(buttonText, GUILayout.Width(90), GUILayout.Height(90))) 
                ShowInvocationSelectionMenu(x, y, cell, battleData);
            
            GUI.backgroundColor = originalColor;
        }
        
        private void ShowInvocationSelectionMenu(int x, int y, BattleMatrixCell cell, BattleData battleData)
        {
            GenericMenu menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Clear"), false, () => {
                cell.Clear();
                EditorUtility.SetDirty(_target);
                AssetDatabase.SaveAssets();
            });
            
            menu.AddSeparator("");
            
            if (_availableInvocationIds.Count > 0)
            {
                foreach (string id in _availableInvocationIds)
                {
                    string capturedId = id;
                    menu.AddItem(new GUIContent($"ID: {capturedId}"), cell.InvocationId == capturedId, () => {
                        cell.SetInvocation(capturedId);
                        EditorUtility.SetDirty(_target);
                        AssetDatabase.SaveAssets();
                    });
                }
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("No IDs available"));
            }
            
            menu.ShowAsContext();
        }
    }
}
