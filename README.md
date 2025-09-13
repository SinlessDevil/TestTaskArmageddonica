# WAVE BATTLE - TestTaskArmageddonica

![WAVE BATTLE](https://github.com/yourusername/TestTaskArmageddonica/assets/wave-battle-preview.png)

**WAVE BATTLE** - A strategic game with tactical elements, developed in Unity using modern architecture and advanced development tools.

## 🎮 Game Description

WAVE BATTLE is a tactical strategy game with card game elements where players control an army of diverse units and buildings on a 6x6 grid. The game includes an Invocation system for summoning various entities, including units, buildings, and skills.

### Key Features:
- **6x6 Tactical Grid** for unit and building placement
- **Card System** with various ranks (Common, Rare, Epic, Legendary)
- **Diverse Units** with unique characteristics
- **Buildings** with defensive abilities
- **Skills** for strategic advantage
- **Progression System** and save functionality

## 🏗️ Project Architecture

The project is built on **Clean Architecture** principles with clear separation of concerns and design pattern usage.

### Core Architecture Components:

#### 🎯 State Machine Architecture
- **GameStateMachine** - manages main game states
- **BattleStateMachine** - manages battle states
- **Game States:**
  - `BootstrapState` - system initialization
  - `LoadProgressState` - player progress loading
  - `LoadMenuState` - main menu state
  - `LoadLevelState` - level loading
  - `GamePlayState` - main gameplay

#### 🏭 Service Layer
- **StaticDataService** - static data management
- **PersistenceProgressService** - save system
- **InputService** - input handling (PC/Mobile)
- **AudioVibrationService** - sound and vibration
- **TimeService** - time management
- **CameraDirector** - camera control

#### 🎨 UI System
- **UIFactory** - UI element factory
- **Window System** - window system with settings
- **HUD System** - game user interface
- **Card System** - card system with drag&drop

## 🛠️ Technology Stack

### Unity Packages:
- **Unity 2022.3+** - main engine
- **Universal Render Pipeline (URP) 17.0.4** - modern rendering
- **Addressables 2.7.2** - asset system
- **Input System 1.14.2** - new input system
- **2D Animation 10.2.1** - 2D animation
- **Timeline 1.8.9** - timeline system
- **Visual Scripting 1.9.8** - visual programming

### Third-party Libraries:
- **Zenject** - Dependency Injection container
- **DOTween Pro** - animations and tweens
- **UniTask** - async programming
- **Odin Inspector** - enhanced Inspector
- **TextMeshPro** - improved typography
- **SRDebugger** - debugging tools
- **ParticleEffectForUGUI** - UI effects

### Development Tools:
- **Odin Inspector** - for creating custom editors
- **Unity Test Framework** - testing
- **Newtonsoft.Json** - JSON handling

## 🔧 RedTulZ - Development Tools

The project includes a set of specialized tools to accelerate development:

### 1. 🎯 Battle Matrix Editor
**Interactive Battle Matrix Editor**

Tool for creating and editing tactical battle layouts:
- **Interactive 6x6 Grid** with visual editor
- **Drag & Drop** unit and building placement
- **Automatic ID Generation** for battles
- **Preview** of available invocation IDs
- **Matrix Size Validation**
- **Export/Import** battle configurations

![Battle Matrix Editor](https://github.com/yourusername/TestTaskArmageddonica/assets/battle-matrix-editor.png)

### 2. 📋 Invocation Data Creator
**Step-by-step Invocation Data Creation Wizard**

Intuitive editor for creating game entities:
- **5-Step Creation Wizard**
- **Invocation Types:** Units, Buildings, Skills
- **Automatic Generation** of CardDefinitionType enum
- **Data Validation** at each step
- **Batch Editing** of existing data
- **Collection Integration**

**Wizard Steps:**
1. **Type Selection** - Unit/Building/Skill
2. **Basic Data** - ID, Prefab, Rank
3. **Card Definition** - Name, Description, Icon
4. **Specific Parameters** - Health, Damage, Speed, etc.
5. **Review & Create** - final validation

![Invocation Creator](https://github.com/yourusername/TestTaskArmageddonica/assets/invocation-creator.png)

### 3. 📝 Text to TextMeshPro Converter
**Bidirectional Text Converter**

Utility for migrating between text components:
- **Text ↔ TextMeshPro** bidirectional conversion
- **Settings Preservation** - fonts, sizes, colors, alignment
- **Batch Processing** of multiple components
- **Preview** before conversion
- **Automatic Font Matching**
- **Prefab and Regular Object Support**

![Text Converter](https://github.com/yourusername/TestTaskArmageddonica/assets/text-converter.png)

## 📁 Project Structure

```
Assets/
├── Code/
│   ├── Infrastructure/          # Core infrastructure
│   │   ├── StateMachine/        # State system
│   │   ├── Installers/          # Zenject installers
│   │   └── Services/            # Application services
│   ├── Logic/                   # Game logic
│   │   ├── Grid/               # Grid system
│   │   └── Points/             # Points system
│   ├── UI/                     # User interface
│   ├── StaticData/             # Static data
│   ├── Editor/                 # Editor tools
│   └── Window/                 # Window system
├── Resources/                   # Game resources
├── Scenes/                     # Unity scenes
└── Plugins/                    # External plugins
```

## 🎮 Game Systems

### 🃏 Card System
- **CardView** - visual card representation
- **CardSelection** - card selection system
- **CardDrag** - drag & drop functionality
- **CardRankType** - card ranks (Common, Rare, Epic, Legendary)

### ⚔️ Battle System
- **BattleMatrix** - 6x6 tactical grid
- **Invocation System** - unit summoning system
- **Power Calculation** - army strength calculation
- **Turn-based** game mechanics

### 🏰 Invocation Types
- **Units** - mobile combat units
- **Buildings** - stationary structures
- **Skills** - magical abilities

## 🚀 Running the Project

### Requirements:
- Unity 2022.3 LTS or newer
- Visual Studio 2022 or Rider
- Git LFS for large files

### Installation:
1. Clone the repository
2. Open the project in Unity
3. Unity will automatically import all packages
4. Run the `Initial` scene

### Build:
- **PC Build** - ready for Windows build
- **Mobile Build** - Android/iOS support
- **WebGL** - web version available

## 🎥 Demo

Watch the project demo video:
[![WAVE BATTLE Demo](https://img.youtube.com/vi/8jINENfk4yA/0.jpg)](https://www.youtube.com/watch?v=8jINENfk4yA)

## 👨‍💻 Development

### Architectural Principles:
- **SOLID** design principles
- **Dependency Injection** via Zenject
- **Separation of Concerns** - clear responsibility separation
- **State Machine Pattern** - for state management
- **Service Layer Pattern** - for business logic

### Code Style:
- **C# 9.0+** syntax
- **Async/Await** for async operations
- **UniTask** instead of coroutines
- **Nullable reference types**
- **Clean Code** principles

## 📊 Project Statistics

- **~50,000+** lines of code
- **100+** classes and interfaces
- **25+** units and buildings
- **6** different battles
- **3** specialized editors
- **Multi-platform** - PC/Mobile/Web

## 🤝 Project Contribution

The project demonstrates modern approaches to Unity game development:
- Modular architecture
- High-performance development tools
- Quality user code
- Intuitive editors for designers

## 📄 License

This project was created as part of a test assignment and demonstrates Unity game development skills.

---

**WAVE BATTLE** - where strategy meets tactics! 🎮⚔️