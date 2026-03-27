# Directory Structure

> How Unity C# code is organized in this project.

---

## Overview

This project is a Unity-based wuxia strategy game with modular architecture:

- **Manager Layer** - Core system management with dependency injection
- **Controller Layer** - Business logic driven by messages
- **Model Layer** - Data models (SingleModel, BattleUnit, etc.)
- **View Layer** - UI panels and scene views
- **Message Layer** - Event system for inter-layer communication
- **Config Layer** - Luban-generated configuration tables

---

## Directory Layout

```
Assets/Scripts/
├── AppManager.cs           # Application manager base class
├── GameManager.cs          # Game entry point
│
├── Config/                 # Configuration table system
│   ├── ConfigManager.cs    # Config table manager
│   └── Config/             # Luban-generated config classes
│
├── Const/                  # Constants
│   ├── GameConst.cs
│   ├── GameEnumArray.cs
│   └── GameResource.cs
│
├── Controller/             # Controllers (business logic)
│   └── Impl/
│       ├── Battle/         # Battle-related controllers
│       ├── DebugController.cs
│       └── GameStartController.cs
│
├── Debug/                  # Debug utilities
│
├── Interface/              # Interface definitions
│   ├── Base/               # Base interfaces (IManager, IModel, etc.)
│   └── Mono/               # Mono-related interfaces
│
├── Manager/                # Core managers
│   ├── ArchiveManager/     # Save/load system
│   ├── ConditionManager/   # Condition evaluation
│   ├── ControllerManager/  # Controller registration
│   ├── InputManager/      # Input handling
│   ├── JobManager/        # Coroutine/Task management
│   ├── LogManager/        # Logging
│   ├── MessageManager/    # Message dispatch
│   ├── ModelManager/      # Model lifecycle
│   ├── PoolManager/       # Object pooling
│   ├── ResourceManager/   # Resource loading (YooAsset)
│   ├── UIManager/         # UI panel management
│   ├── ViewManager/       # View management
│   └── ...
│
├── Message/                # Message system
│   ├── Base/
│   │   └── MessageModel.cs # Base message class
│   └── Impl/
│       ├── Battle/        # Battle events
│       ├── Business/      # Business events
│       └── InputEventModel.cs
│
├── Model/                  # Data models
│   ├── Archive/           # Archive models (ISingleArchiveModel)
│   ├── Battle/            # Battle system
│   │   ├── Logic/          # Battle logic
│   │   │   ├── BattleUnit.cs
│   │   │   ├── BattleProperty.cs
│   │   │   ├── BattleKey.cs
│   │   │   ├── BattleSkill/   # Skill implementations
│   │   │   ├── BattleMomentCondition/  # Moment conditions
│   │   │   └── BattleTreasure/ # Treasure items
│   │   └── Manager/       # Battle managers
│   └── Util/              # Utilities
│
├── MonoEx/                 # MonoBehaviour extensions
│
└── View/                   # UI views
    ├── Battle/             # Battle UI
    ├── Panel/
    │   ├── Gen/            # Auto-generated panel code
    │   └── UILogic/       # Panel logic implementations
    └── Scene/             # Scene views
```

---

## Module Organization

### Adding New Manager

Create in `Assets/Scripts/Manager/<ManagerName>/`:
```
Manager/<ManagerName>/
├── <ManagerName>.cs       # Main manager class
└── <ManagerInterface>.cs # Interface (optional)
```

**Base Class**: Extend `ManagerBase`
```csharp
public class MyManager : ManagerBase, IMyManager
{
    protected override IEnumerator OnInit()
    {
        // Initialization code
        yield break;
    }
}
```

### Adding New Controller

Create in `Assets/Scripts/Controller/Impl/<Feature>/`:
```
Controller/Impl/<Feature>/
└── <Feature>Controller.cs
```

**Base Class**: Extend `ControllerBase<TMsg>`
```csharp
public class MyController : ControllerBase<MyEventModel>
{
    [Inject] private MyManager MyManager;
    
    public override void Handle(MyEventModel msg)
    {
        // Handle message
    }
}
```

### Adding New Model

Create in `Assets/Scripts/Model/<Category>/`:
```
Model/<Category>/
└── MyModel.cs
```

**Types**:
- `SingleModel` - Singleton model, message-driven lifecycle
- `SingleArchiveModel` - Persisted model with auto-save/load
- `BattleUnit` - Battle unit with properties, skills, buffs
- Regular class - For simple data structures

### Adding New UI Panel

Create in `Assets/Scripts/View/Panel/UILogic/<PanelName>/`:
```
View/Panel/UILogic/<PanelName>/
└── <PanelName>.cs
```

**Base Class**: Extend `Panel`
```csharp
public class MyPanel : Panel
{
    [AutoFind]
    private Button ConfirmButton;
    
    protected override void RegisterEvent()
    {
        Register<MyEventModel>(OnMyEvent);
        ConfirmButton.onClick.AddListener(OnConfirm);
    }
    
    private void OnConfirm() { }
}
```

### Adding New Message

Create in `Assets/Scripts/Message/Impl/<Category>/`:
```
Message/Impl/<Category>/
└── MyEventModel.cs
```

**Base Class**: Extend `MessageModel`
```csharp
public class MyEventModel : MessageModel
{
    public int Param1;
    public string Param2;
}
```

---

## Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Manager | `<Name>Manager` | `MessageManager` |
| Controller | `<Feature>Controller` | `BattleStartController` |
| Model | `<Feature>Model` / `<Entity>Model` | `SingleModel`, `BattleUnit` |
| View/Panel | `UI<Name>Panel` | `UIBattlePanel` |
| Message | `<Event>Model` / `<Event>EventModel` | `BattleStartEventModel` |
| Interface | `I<Name>` | `IManager`, `IController<T>` |
| Battle Skill | `Skill<ID>` | `Skill1001` |
| Battle Treasure | `BattleTreasure<ID>` | `BattleTreasure10001` |

---

## Examples

See these well-organized modules:

| Module | Location | Purpose |
|--------|----------|---------|
| MessageManager | `Manager/MessageManager/` | Event dispatch system |
| BattleUnit | `Model/Battle/Logic/BattleUnit.cs` | Battle unit entity |
| UIBattlePanel | `View/Panel/UILogic/UIBattlePanel/` | Battle UI |
| BattleSkill | `Model/Battle/Logic/BattleSkill/` | Skill implementations |
