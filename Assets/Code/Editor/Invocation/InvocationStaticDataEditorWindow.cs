using System;
using System.Collections.Generic;
using System.IO;
using Code.StaticData.Cards;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.Data.Skill;
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
        private int _unitHealth = 100;
        private int _unitDamage = 10;
        private int _unitSpeed = 5;
        
        private float _buildingDefense = 5f;
        private float _buildingDamage = 10f;
        private SkillData _buildingSkill = new SkillData();
        
        private SkillData _skillData = new SkillData();
        
        private Vector2 _scrollPosition;
        private int _currentStep = 1;
        
        private int _selectedTab = 0;
        private string[] _tabNames = { "Create New", "Edit Existing" };
        
        private List<InvocationStaticData> _allInvocationData = new();
        private List<UnitStaticData> _unitData = new();
        private List<BuildStaticData> _buildData = new();
        private List<SkillStaticData> _skillDataList = new();
        private Vector2 _existingObjectsScrollPosition;
        private bool _dataLoaded = false;
        
        // Foldout states for sections
        private bool _unitsSectionFolded = false;
        private bool _buildsSectionFolded = false;
        private bool _skillsSectionFolded = false;
        
        // Foldout states for individual items
        private Dictionary<string, bool> _itemFoldoutStates = new();
        
        [MenuItem("Tools/Invocation Static Data Window Editor", false, 2002)]
        public static void ShowWindow()
        {
            InvocationStaticDataEditorWindow window = GetWindow<InvocationStaticDataEditorWindow>("Invocation Creator");
            window.LoadExistingData();
        }
        
        private void LoadExistingData()
        {
            _allInvocationData.Clear();
            _unitData.Clear();
            _buildData.Clear();
            _skillDataList.Clear();
            
            string[] guids = AssetDatabase.FindAssets("t:InvocationStaticData");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                InvocationStaticData data = AssetDatabase.LoadAssetAtPath<InvocationStaticData>(path);
                if (data != null)
                {
                    _allInvocationData.Add(data);

                    switch (data)
                    {
                        case UnitStaticData unitData:
                            _unitData.Add(unitData);
                            break;
                        case BuildStaticData buildData:
                            _buildData.Add(buildData);
                            break;
                        case SkillStaticData skillData:
                            _skillDataList.Add(skillData);
                            break;
                    }
                }
            }
            
            _dataLoaded = true;
        }
        
        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            EditorGUILayout.LabelField("Invocation Static Data Creator", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            _selectedTab = GUILayout.Toolbar(_selectedTab, _tabNames);
            EditorGUILayout.Space();
            
            if (_selectedTab == 0)
            {
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
            }
            else
            {
                DrawExistingObjectsTab();
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawExistingObjectsTab()
        {
            EditorGUILayout.LabelField("Edit Existing Invocation Data", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            // Control buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh Data", GUILayout.Height(25)))
                LoadExistingData();
            
            if (GUILayout.Button("Expand All", GUILayout.Height(25)))
            {
                _unitsSectionFolded = false;
                _buildsSectionFolded = false;
                _skillsSectionFolded = false;
                _itemFoldoutStates.Clear();
            }
            
            if (GUILayout.Button("Collapse All", GUILayout.Height(25)))
            {
                _unitsSectionFolded = true;
                _buildsSectionFolded = true;
                _skillsSectionFolded = true;
                _itemFoldoutStates.Clear();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            if (!_dataLoaded)
                LoadExistingData();
            
            _existingObjectsScrollPosition = EditorGUILayout.BeginScrollView(_existingObjectsScrollPosition);
            
            DrawCollectionSection("Units", _unitData);
            DrawCollectionSection("Builds", _buildData);
            DrawCollectionSection("Skills", _skillDataList);
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawCollectionSection<T>(string title, List<T> collection) where T : InvocationStaticData
        {
            bool isFolded = GetSectionFoldoutState(title);
            
            EditorGUILayout.BeginHorizontal();
            isFolded = EditorGUILayout.Foldout(!isFolded, $"{title} ({collection.Count} items)", true, EditorStyles.foldoutHeader);
            SetSectionFoldoutState(title, !isFolded);
            EditorGUILayout.EndHorizontal();
            
            if (isFolded)
            {
                if (collection.Count == 0)
                {
                    EditorGUILayout.HelpBox($"No {title.ToLower()} found.", MessageType.Info);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    foreach (var item in collection)
                        DrawInvocationDataItem(item);
                    EditorGUI.indentLevel--;
                }
            }
            
            EditorGUILayout.Space();
        }
        
        private bool GetSectionFoldoutState(string sectionName)
        {
            return sectionName switch
            {
                "Units" => _unitsSectionFolded,
                "Builds" => _buildsSectionFolded,
                "Skills" => _skillsSectionFolded,
                _ => false
            };
        }
        
        private void SetSectionFoldoutState(string sectionName, bool folded)
        {
            switch (sectionName)
            {
                case "Units":
                    _unitsSectionFolded = folded;
                    break;
                case "Builds":
                    _buildsSectionFolded = folded;
                    break;
                case "Skills":
                    _skillsSectionFolded = folded;
                    break;
            }
        }
        
        private void DrawInvocationDataItem(InvocationStaticData data)
        {
            string itemKey = $"{data.Id}_{data.InvocationType}";
            bool isItemFolded = GetItemFoldoutState(itemKey);
            
            EditorGUILayout.BeginVertical("box");
            
            // Header with foldout
            EditorGUILayout.BeginHorizontal();
            isItemFolded = EditorGUILayout.Foldout(!isItemFolded, $"{data.Id} ({data.InvocationType})", true, EditorStyles.foldoutHeader);
            SetItemFoldoutState(itemKey, !isItemFolded);
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                Selection.activeObject = data;
                EditorGUIUtility.PingObject(data);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (isItemFolded)
            {
                EditorGUI.indentLevel++;
                DrawItemContent(data);
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        
        private void DrawItemContent(InvocationStaticData data)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID:", GUILayout.Width(50));
            data.Id = EditorGUILayout.TextField(data.Id);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rank:", GUILayout.Width(50));
            data.Rank = (CardRankType)EditorGUILayout.EnumPopup(data.Rank);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab:", GUILayout.Width(50));
            data.Prefab = (GameObject)EditorGUILayout.ObjectField(data.Prefab, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();
 
            switch (data)
            {
                case UnitStaticData unitData:
                    EditorGUILayout.LabelField("Unit Stats:", EditorStyles.miniBoldLabel);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Health:", GUILayout.Width(50));
                    unitData.Health = EditorGUILayout.IntField(unitData.Health);
                    EditorGUILayout.LabelField("Damage:", GUILayout.Width(50));
                    unitData.Damage = EditorGUILayout.IntField(unitData.Damage);
                    EditorGUILayout.LabelField("Speed:", GUILayout.Width(50));
                    unitData.Speed = EditorGUILayout.IntField(unitData.Speed);
                    EditorGUILayout.EndHorizontal();
                    break;
                case BuildStaticData buildData:
                    EditorGUILayout.LabelField("Build Stats:", EditorStyles.miniBoldLabel);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Defense:", GUILayout.Width(60));
                    buildData.Defense = EditorGUILayout.FloatField(buildData.Defense);
                    EditorGUILayout.LabelField("Damage:", GUILayout.Width(50));
                    buildData.Damage = EditorGUILayout.FloatField(buildData.Damage);
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Skill:", GUILayout.Width(50));
                    if (buildData.Skill == null)
                        buildData.Skill = new SkillData();
                    EditorGUILayout.BeginVertical();
                    buildData.Skill.Value = EditorGUILayout.FloatField("Value:", buildData.Skill.Value);
                    buildData.Skill.SkillType = (SkillType)EditorGUILayout.EnumPopup("Type:", buildData.Skill.SkillType);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    break;
                case SkillStaticData skillData:
                    EditorGUILayout.LabelField("Skill Stats:", EditorStyles.miniBoldLabel);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Skill:", GUILayout.Width(50));
                    if (skillData.Skill == null)
                        skillData.Skill = new SkillData();
                    EditorGUILayout.BeginVertical();
                    skillData.Skill.Value = EditorGUILayout.FloatField("Value:", skillData.Skill.Value);
                    skillData.Skill.SkillType = (SkillType)EditorGUILayout.EnumPopup("Type:", skillData.Skill.SkillType);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    break;
            }
            
            if (GUILayout.Button("Save Changes", GUILayout.Height(20)))
            {
                EditorUtility.SetDirty(data);
                AssetDatabase.SaveAssets();
                Debug.Log($"Saved changes to {data.Id}");
            }
        }
        
        private bool GetItemFoldoutState(string itemKey)
        {
            if (!_itemFoldoutStates.ContainsKey(itemKey))
                _itemFoldoutStates[itemKey] = true;
            return _itemFoldoutStates[itemKey];
        }
        
        private void SetItemFoldoutState(string itemKey, bool folded)
        {
            _itemFoldoutStates[itemKey] = folded;
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
                case InvocationType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                    _unitHealth = EditorGUILayout.IntField("Health", _unitHealth);
                    _unitDamage = EditorGUILayout.IntField("Damage", _unitDamage);
                    _unitSpeed = EditorGUILayout.IntField("Speed", _unitSpeed);
                    break;
                case InvocationType.Build:
                    EditorGUILayout.LabelField("Building Parameters", EditorStyles.boldLabel);
                    _buildingDefense = EditorGUILayout.FloatField("Defense", _buildingDefense);
                    _buildingDamage = EditorGUILayout.FloatField("Damage", _buildingDamage);
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Skill:", EditorStyles.boldLabel);
                    if (_buildingSkill == null)
                        _buildingSkill = new SkillData();
                    _buildingSkill.Value = EditorGUILayout.FloatField("Skill Value:", _buildingSkill.Value);
                    _buildingSkill.SkillType = (SkillType)EditorGUILayout.EnumPopup("Skill Type:", _buildingSkill.SkillType);
                    break;
                case InvocationType.Skill:
                    EditorGUILayout.LabelField("Skill Parameters", EditorStyles.boldLabel);
                    if (_skillData == null)
                        _skillData = new SkillData();
                    _skillData.Value = EditorGUILayout.FloatField("Skill Value:", _skillData.Value);
                    _skillData.SkillType = (SkillType)EditorGUILayout.EnumPopup("Skill Type:", _skillData.SkillType);
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
                EditorGUILayout.HelpBox("Please fill in all required fields", MessageType.Warning);
            
            EditorGUI.BeginDisabledGroup(!isValid);
            
            if (GUILayout.Button("Create Invocation Static Data", GUILayout.Height(30))) 
                CreateInvocationStaticData();
            
            EditorGUI.EndDisabledGroup();
        }
        
        private void DrawNavigationButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (_currentStep > 1)
                if (GUILayout.Button("Previous")) 
                    _currentStep--;
            
            GUILayout.FlexibleSpace();
            
            if (_currentStep < 5)
            {
                bool canProceed = CanProceedToNextStep();
                EditorGUI.BeginDisabledGroup(!canProceed);
                
                if (GUILayout.Button("Next")) 
                    _currentStep++;
                
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
                CreateCardDefinitionStaticData();
                InvocationStaticData invocationData = CreateInvocationStaticDataAsset();
                if (invocationData != null)
                    CollectionUpdater.AddToInvocationCollection(invocationData);
                
                EditorUtility.DisplayDialog("Success", 
                    "Invocation Static Data and Card Definition created and added to collections successfully!", "OK");
                ClearForm();
                LoadExistingData();
                _selectedTab = 1;
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Error", $"Failed to create assets: {e.Message}", "OK");
            }
        }
        
        private void CreateCardDefinitionStaticData()
        {
            EnumUpdater.AddCardDefinitionType(_cardName);
            CardDefinitionStaticData cardDefinition = CreateInstance<CardDefinitionStaticData>();
            cardDefinition.Name = _cardName;
            cardDefinition.Description = _cardDescription;
            cardDefinition.Icon = _cardIcon;
            cardDefinition.Type = CardDefinitionType.Unknown;
            
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
        }
        
        private CardDefinitionType GetCardDefinitionTypeFromName(string cardName)
        {
            if (string.IsNullOrEmpty(cardName))
                return CardDefinitionType.Unknown;
            
            if (Enum.TryParse(cardName, out CardDefinitionType result))
                return result;
            
            if (!EnumUpdater.IsCardDefinitionTypeExists(cardName)) 
                EnumUpdater.AddCardDefinitionType(cardName);
            
            return Enum.TryParse(cardName, out result) ? result : CardDefinitionType.Unknown;
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
                case InvocationType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (asset == null)
                throw new Exception("Failed to create InvocationStaticData asset");
            
            InvocationStaticData invocationData = asset as InvocationStaticData;
            invocationData.Id = _id;
            invocationData.Prefab = _prefab;
            invocationData.Rank = _rank;
            invocationData.CardDefinition = GetCardDefinitionTypeFromName(_cardName);
            invocationData.InvocationType = _invocationType;
            
            SetSpecificStats(invocationData);
            
            if (!Directory.Exists(folderPath)) 
                Directory.CreateDirectory(folderPath);
            
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
        
        private void SetSpecificStats(InvocationStaticData invocationData)
        {
            switch (_invocationType)
            {
                case InvocationType.Unit:
                    if (invocationData is UnitStaticData unitData)
                    {
                        unitData.Health = _unitHealth;
                        unitData.Damage = _unitDamage;
                        unitData.Speed = _unitSpeed;
                    }
                    break;
                case InvocationType.Build:
                    if (invocationData is BuildStaticData buildData)
                    {
                        buildData.Defense = _buildingDefense;
                        buildData.Damage = _buildingDamage;
                        buildData.Skill = _buildingSkill ?? new SkillData();
                    }
                    break;
                case InvocationType.Skill:
                    if (invocationData is SkillStaticData skillData)
                    {
                        skillData.Skill = _skillData ?? new SkillData();
                    }
                    break;
                case InvocationType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            
            _unitHealth = 100;
            _unitDamage = 10;
            _unitSpeed = 5;
            _buildingDefense = 5f;
            _buildingDamage = 10f;
            _buildingSkill = new SkillData();
            _skillData = new SkillData();
        }
        
        public void UpdateCardDefinitionsAfterReload()
        {
            string[] cardGuids = AssetDatabase.FindAssets($"t:CardDefinitionStaticData");
            foreach (string guid in cardGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardDefinitionStaticData cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(path);

                if (cardDefinition == null || string.IsNullOrEmpty(cardDefinition.Name)) 
                    continue;
                
                if (!Enum.TryParse(cardDefinition.Name, out CardDefinitionType result))
                    continue;
                
                if (cardDefinition.Type != result)
                {
                    cardDefinition.Type = result;
                    EditorUtility.SetDirty(cardDefinition);
                }
                        
                CollectionUpdater.AddToCardDefinitionCollection(cardDefinition);
            }
            
            string[] invocationGuids = AssetDatabase.FindAssets($"t:InvocationStaticData");
            foreach (string guid in invocationGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                InvocationStaticData invocationData = AssetDatabase.LoadAssetAtPath<InvocationStaticData>(path);

                if (invocationData == null || invocationData.CardDefinition != CardDefinitionType.Unknown) 
                    continue;
                
                string[] cardGuids2 = AssetDatabase.FindAssets($"t:CardDefinitionStaticData");
                foreach (string cardGuid in cardGuids2)
                {
                    string cardPath = AssetDatabase.GUIDToAssetPath(cardGuid);
                    CardDefinitionStaticData cardDefinition = AssetDatabase.LoadAssetAtPath<CardDefinitionStaticData>(cardPath);

                    if (cardDefinition == null || cardDefinition.Name != invocationData.Id) 
                        continue;
                    
                    if (!Enum.TryParse(cardDefinition.Name, out CardDefinitionType result)) 
                        continue;
                    
                    invocationData.CardDefinition = result;
                    EditorUtility.SetDirty(invocationData);
                    break;
                }
            }
            
            AssetDatabase.SaveAssets();
        }
    }
}