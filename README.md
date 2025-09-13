# 🎮 Clone 9 Kings — TestTaskArmageddonica
<p align="center"> <a href="https://www.youtube.com/watch?v=8jINENfk4yA" target="_blank"> <img src="https://img.youtube.com/vi/8jINENfk4yA/0.jpg" alt="Watch the demo" /> </a> </p> <p align="center"> <a href="https://www.youtube.com/watch?v=8jINENfk4yA" target="_blank"> <img src="https://img.shields.io/badge/Watch%20on-YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white" alt="Watch on YouTube" /> </a> </p>

🏗️ Architecture
🎯 Game State Machine (high-level flow)

BootstrapState
Sets up DI, loads static data, registers services.

- LoadProgressState
Loads player progress from persistence (creates defaults if none).
- LoadMenuState
Brings up the main menu / meta UI.
- LoadLevelState
Loads scene, initializes grid and battle context, wires UI/HUD.
- GamePlayState
Runs gameplay loop and hosts Battle State Machine. Handles pause/end, saves progress on exit.

Transitions (simplified)
Bootstrap → LoadProgress → LoadMenu → LoadLevel → GamePlay → (Menu|Next Level|Exit)

⚔️ Battle State Machine (combat loop)
PrepareBattleState
Reads selected BattleData (matrix), spawns grid cells, places predefined invocations (units/buildings) from matrix.
PlayerTurnState
Card selection, placement, skill usage, resource checks, UI hints.
EnemyTurnState (if enabled)
Simple AI/sequence processing, counter-actions.
ResolveCombatState
Movement/attacks resolution, damage, deaths, win/lose conditions.

VictoryState / DefeatState
Rewards/summary or retry/back to menu.

#### 🎯 State Machine Architecture
- **GameStateMachine** - manages main game states
- **BattleStateMachine** - manages battle states
- **Game States (Execution Order):**
  - `BootstrapState` - system initialization and initial scene loading
  - `LoadProgressState` - player progress loading and data initialization
  - `BootstrapAnalyticState` - analytics system bootstrap
  - `PreLoadGameState` - determines first scene based on tutorial completion
  - `LoadMenuState` - main menu state loading
  - `LoadLevelState` - game level loading and setup
  - `GamePlayState` - main gameplay state

🎨 UI Pattern
MVPM (Model–View–Presenter–Model):
Views stay dumb; Presenters handle input & state; Models hold data.


🔧 Editor Tools

1) Battle Matrix Editor — build battle layouts
Interactive editor for creating and editing BattleData with a visual grid.
What it does
Displays a 6×6 (or custom) grid to place invocations on specific cells
Assigns any defined Invocation ID (Unit/Building/Skill) to a cell
Validates matrix size; prevents out-of-range edits
Persists changes to static data assets
Controls (from the window)
Add New Battle — creates a new BattleData entry
Refresh Invocation IDs — refreshes the ID catalog shown below the grid
Force Save — forces asset save
Debug Data — quick inspection for validation/debug
<img width="757" height="1055" alt="image" src="https://github.com/user-attachments/assets/5b6ee5f3-b3f6-4b67-b53e-4f17f27cb970" />

2) Invocation Data Creator — guided data wizard
A 5-step wizard to create Units, Buildings, and Skills consistently.
Steps
Type Selection — Unit / Building / Skill
Basics — ID, prefab, rank
Card Definition — name, description, icon
Type-specific parameters
Unit: Health, Damage, Speed (and other combat stats)
Building: Attack, Defense, special traits
Skill: effect type, value, duration, targets
Review & Create — validation + asset generation
<img width="758" height="836" alt="image" src="https://github.com/user-attachments/assets/4a88a998-43e2-49c4-a843-d720073810ab" />

3) Text ↔ TextMeshPro Converter — migrate UI safely
Bidirectional converter between UnityEngine.UI.Text and TMP components.
<img width="587" height="894" alt="image" src="https://github.com/user-attachments/assets/4eb9429d-98e1-4170-aab1-14d25006859c" />

### Additional Development Tools:
- **Audio Vibration Window/Sound Library** - Audio system management
- **Tests Tool Window** - Automated testing interface
- **Save Window** - Save system management tools

## 📁 Project Structure

```
Assets/
├── Code/
│   ├── Infrastructure/    # State machines, installers, services
│   ├── Logic/             # Gameplay (Grid, Battle, Points, etc.)
│   ├── UI/                # Views/Presenters (MVPM)
│   ├── StaticData/        # ScriptableObjects (Invocations, Battles)
│   ├── Editor/            # RedTulZ tools
│   └── Window/            # Window system
├── Scenes/
└── Plugins/
```

🧰 Tech Stack (essentials only)
Unity 6 / 2022.3 LTS compatible
Zenject — dependency injection
UniTask — async/await for Unity
DOTween — tweening/animation
Odin Inspector — editor tooling
TextMeshPro — typography
(Optional) Addressables — asset management
