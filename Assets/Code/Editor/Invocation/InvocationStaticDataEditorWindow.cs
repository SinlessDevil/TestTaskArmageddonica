using System;
using System.IO;
using Code.StaticData.Cards;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Invocation
{
    public class InvocationStaticDataEditorWindow : EditorWindow
    {
        // Step 1: Invocation Type Selection
        private InvocationType _invocationType = InvocationType.Unit;
        private bool _invocationTypeSelected = false;
        
        // Step 2: Basic Data
        private string _id = "";
        private GameObject _prefab;
        private CardRankType _rank = CardRankType.Common;
        
        // Step 3: Card Definition Data
        private string _cardName = "";
        private string _cardDescription = "";
        private Sprite _cardIcon;
        
        // Step 4: Specific Parameters (will be shown based on InvocationType)
        private float _unitHealth = 100f;
        private float _unitDamage = 10f;
        private float _unitSpeed = 5f;
        
        private float _buildingHealth = 200f;
        private float _buildingDefense = 5f;
        private float _buildingRange = 10f;
        
        private float _skillCooldown = 5f;
        private float _skillManaCost = 10f;
        private float _skillRange = 8f;
        
        private Vector2 _scrollPosition;
        private int _currentStep = 1;
        
        [MenuItem("Tools/Invocation Static Data Creator")]
        public static void ShowWindow()
        {
            GetWindow<InvocationStaticDataEditorWindow>("Invocation Creator");
        }
        
        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            EditorGUILayout.LabelField("Invocation Static Data Creator", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            DrawStepIndicator();
            EditorGUILayout.Space();
            
            switch (_currentStep)
            {
                case 1:
                    DrawStep1_InvocationTypeSelection();
                    break;
                case 2:
                    DrawStep2_BasicData();
                    break;
                case 3:
                    DrawStep3_CardDefinition();
                    break;
                case 4:
                    DrawStep4_SpecificParameters();
                    break;
                case 5:
                    DrawStep5_ReviewAndCreate();
                    break;
            }
            
            EditorGUILayout.Space();
            DrawNavigationButtons();
            
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawStepIndicator()
        {
            EditorGUILayout.LabelField($"Step {_currentStep} of 5", EditorStyles.boldLabel);
            
            string[] stepNames = {
                "Select Invocation Type",
                "Basic Data",
                "Card Definition",
                "Specific Parameters",
                "Review & Create"
            };
            
            EditorGUILayout.LabelField(stepNames[_currentStep - 1], EditorStyles.miniLabel);
        }
        
        private void DrawStep1_InvocationTypeSelection()
        {
            EditorGUILayout.LabelField("Step 1: Select Invocation Type", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            EditorGUILayout.HelpBox("Choose the type of invocation you want to create. This will determine what specific parameters you can configure.", MessageType.Info);
            EditorGUILayout.Space();
            
            _invocationType = (InvocationType)EditorGUILayout.EnumPopup("Invocation Type", _invocationType);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Description:", EditorStyles.boldLabel);
            
            switch (_invocationType)
            {
                case InvocationType.Unit:
                    EditorGUILayout.LabelField("• Units are mobile entities that can move and attack");
                    EditorGUILayout.LabelField("• They have health, damage, and speed parameters");
                    break;
                case InvocationType.Build:
                    EditorGUILayout.LabelField("• Buildings are stationary structures");
                    EditorGUILayout.LabelField("• They have health, defense, and range parameters");
                    break;
                case InvocationType.Skill:
                    EditorGUILayout.LabelField("• Skills are abilities that can be cast");
                    EditorGUILayout.LabelField("• They have cooldown, mana cost, and range parameters");
                    break;
            }
        }
        
        private void DrawStep2_BasicData()
        {
            EditorGUILayout.LabelField("Step 2: Basic Data", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            _id = EditorGUILayout.TextField("ID", _id);
            _prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false);
            _rank = (CardRankType)EditorGUILayout.EnumPopup("Rank", _rank);
            
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("ID should be unique and descriptive (e.g., 'FireMage', 'StoneWall', 'HealSpell')", MessageType.Info);
        }
        
        private void DrawStep3_CardDefinition()
        {
            EditorGUILayout.LabelField("Step 3: Card Definition", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            _cardName = EditorGUILayout.TextField("Card Name", _cardName);
            _cardDescription = EditorGUILayout.TextArea(_cardDescription, GUILayout.Height(60));
            _cardIcon = (Sprite)EditorGUILayout.ObjectField("Card Icon", _cardIcon, typeof(Sprite), false);
            
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Card Name will be automatically added to the CardDefinitionType enum as a unique type for this specific card.", MessageType.Info);
        }
        
        private void DrawStep4_SpecificParameters()
        {
            EditorGUILayout.LabelField("Step 4: Specific Parameters", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            switch (_invocationType)
            {
                case InvocationType.Unit:
                    EditorGUILayout.LabelField("Unit Parameters", EditorStyles.boldLabel);
                    _unitHealth = EditorGUILayout.FloatField("Health", _unitHealth);
                    _unitDamage = EditorGUILayout.FloatField("Damage", _unitDamage);
                    _unitSpeed = EditorGUILayout.FloatField("Speed", _unitSpeed);
                    break;
                    
                case InvocationType.Build:
                    EditorGUILayout.LabelField("Building Parameters", EditorStyles.boldLabel);
                    _buildingHealth = EditorGUILayout.FloatField("Health", _buildingHealth);
                    _buildingDefense = EditorGUILayout.FloatField("Defense", _buildingDefense);
                    _buildingRange = EditorGUILayout.FloatField("Range", _buildingRange);
                    break;
                    
                case InvocationType.Skill:
                    EditorGUILayout.LabelField("Skill Parameters", EditorStyles.boldLabel);
                    _skillCooldown = EditorGUILayout.FloatField("Cooldown", _skillCooldown);
                    _skillManaCost = EditorGUILayout.FloatField("Mana Cost", _skillManaCost);
                    _skillRange = EditorGUILayout.FloatField("Range", _skillRange);
                    break;
                case InvocationType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DrawStep5_ReviewAndCreate()
        {
            EditorGUILayout.LabelField("Step 5: Review & Create", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Review your data:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"• ID: {_id}");
            EditorGUILayout.LabelField($"• Invocation Type: {_invocationType}");
            EditorGUILayout.LabelField($"• Card Name: {_cardName}");
            EditorGUILayout.LabelField($"• Rank: {_rank}");
            EditorGUILayout.LabelField($"• Prefab: {(_prefab != null ? _prefab.name : "None")}");
            
            EditorGUILayout.Space();
            
            bool isValid = ValidateAllData();
            if (!isValid)
            {
                EditorGUILayout.HelpBox("Please fill in all required fields", MessageType.Warning);
            }
            
            EditorGUI.BeginDisabledGroup(!isValid);
            
            if (GUILayout.Button("Create Invocation Static Data", GUILayout.Height(30)))
            {
                CreateInvocationStaticData();
            }
            
            EditorGUI.EndDisabledGroup();
        }
        
        private void DrawNavigationButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (_currentStep > 1)
            {
                if (GUILayout.Button("Previous"))
                {
                    _currentStep--;
                }
            }
            
            GUILayout.FlexibleSpace();
            
            if (_currentStep < 5)
            {
                bool canProceed = CanProceedToNextStep();
                EditorGUI.BeginDisabledGroup(!canProceed);
                
                if (GUILayout.Button("Next"))
                {
                    _currentStep++;
                }
                
                EditorGUI.EndDisabledGroup();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private bool CanProceedToNextStep()
        {
            return _currentStep switch
            {
                1 => true,
                2 => !string.IsNullOrEmpty(_id) && _prefab != null,
                3 => !string.IsNullOrEmpty(_cardName) && !string.IsNullOrEmpty(_cardDescription) && _cardIcon != null,
                4 => true,
                _ => false
            };
        }
        
        
        private bool ValidateAllData()
        {
            return !string.IsNullOrEmpty(_id) && 
                   _prefab != null && 
                   !string.IsNullOrEmpty(_cardName) && 
                   !string.IsNullOrEmpty(_cardDescription) && 
                   _cardIcon != null;
        }
        
        private void CreateInvocationStaticData()
        {
            if (!ValidateAllData())
            {
                EditorUtility.DisplayDialog("Validation Error", "Please fill in all required fields", "OK");
                return;
            }
            
            try
            {
                // 1. Создаем CardDefinitionStaticData
                CreateCardDefinitionStaticData();
                
                // 2. Создаем InvocationStaticData
                var invocationData = CreateInvocationStaticDataAsset();
                
                // 3. Добавляем в InvocationCollection
                if (invocationData != null)
                {
                    CollectionUpdater.AddToInvocationCollection(invocationData);
                }
                
                EditorUtility.DisplayDialog("Success", 
                    "Invocation Static Data and Card Definition created and added to collections successfully!", "OK");
                ClearForm();
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", $"Failed to create assets: {e.Message}", "OK");
            }
        }
        
        private void CreateCardDefinitionStaticData()
        {
            // Сначала добавляем в enum
            EnumUpdater.AddCardDefinitionType(_cardName);
            
            // Создаем CardDefinitionStaticData с Unknown типом пока
            CardDefinitionStaticData cardDefinition = CreateInstance<CardDefinitionStaticData>();
            cardDefinition.Name = _cardName;
            cardDefinition.Description = _cardDescription;
            cardDefinition.Icon = _cardIcon;
            cardDefinition.Type = CardDefinitionType.Unknown; // Временно Unknown
            
            string folderPath = "Assets/Resources/StaticData/Cards/Definition/";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            string fileName = $"{_cardName}_CardDefinition.asset";
            string fullPath = Path.Combine(folderPath, fileName);
            
            if (File.Exists(fullPath))
            {
                if (!EditorUtility.DisplayDialog("File Exists", 
                    $"Card Definition {fileName} already exists. Do you want to overwrite it?", 
                    "Overwrite", "Cancel"))
                {
                    DestroyImmediate(cardDefinition);
                    return;
                }
            }
            
            AssetDatabase.CreateAsset(cardDefinition, fullPath);
            AssetDatabase.SaveAssets();
            
            // Планируем обновление после компиляции
            InvocationCollectionAutoSync.ScheduleCardDefinitionUpdate(_cardName);
        }
        
        
        private CardDefinitionType GetCardDefinitionTypeFromName(string cardName)
        {
            // Сначала пытаемся найти значение в enum
            if (System.Enum.TryParse<CardDefinitionType>(cardName, out CardDefinitionType result))
                return result;
            
            // Если не найдено, добавляем в enum
            if (!EnumUpdater.IsCardDefinitionTypeExists(cardName))
            {
                EnumUpdater.AddCardDefinitionType(cardName);
            }
            
            // Пробуем снова после добавления
            if (System.Enum.TryParse<CardDefinitionType>(cardName, out result))
                return result;
            
            // Если все еще не найдено, возвращаем Unknown
            Debug.LogWarning($"Could not find CardDefinitionType for {cardName}, using Unknown");
            return CardDefinitionType.Unknown;
        }
        
        private InvocationStaticData CreateInvocationStaticDataAsset()
        {
            ScriptableObject asset = null;
            string folderPath = "";
            
            switch (_invocationType)
            {
                case InvocationType.Unit:
                    asset = CreateInstance<UnitStaticData>();
                    folderPath = "Assets/Resources/StaticData/Invocation/Units/";
                    break;
                case InvocationType.Build:
                    asset = CreateInstance<BuildStaticData>();
                    folderPath = "Assets/Resources/StaticData/Invocation/Builds/";
                    break;
                case InvocationType.Skill:
                    asset = CreateInstance<SkillStaticData>();
                    folderPath = "Assets/Resources/StaticData/Invocation/Skills/";
                    break;
            }
            
            if (asset == null)
            {
                throw new System.Exception("Failed to create InvocationStaticData asset");
            }
            
            var invocationData = asset as InvocationStaticData;
            invocationData.Id = _id;
            invocationData.Prefab = _prefab;
            invocationData.Rank = _rank;
            invocationData.CardDefinition = GetCardDefinitionTypeFromName(_cardName);
            invocationData.InvocationType = _invocationType;
            
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            string fileName = $"{_id}_{_invocationType}.asset";
            string fullPath = Path.Combine(folderPath, fileName);
            
            if (File.Exists(fullPath))
            {
                if (!EditorUtility.DisplayDialog("File Exists", 
                    $"Invocation Data {fileName} already exists. Do you want to overwrite it?", 
                    "Overwrite", "Cancel"))
                {
                    DestroyImmediate(asset);
                    return null;
                }
            }
            
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
            
            return invocationData;
        }
        
        private void ClearForm()
        {
            _currentStep = 1;
            _invocationType = InvocationType.Unit;
            _invocationTypeSelected = false;
            _id = "";
            _prefab = null;
            _rank = CardRankType.Common;
            _cardName = "";
            _cardDescription = "";
            _cardIcon = null;
            
            _unitHealth = 100f;
            _unitDamage = 10f;
            _unitSpeed = 5f;
            _buildingHealth = 200f;
            _buildingDefense = 5f;
            _buildingRange = 10f;
            _skillCooldown = 5f;
            _skillManaCost = 10f;
            _skillRange = 8f;
        }
        
    }
}
