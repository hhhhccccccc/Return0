# Config Table Guidelines

> Luban configuration table patterns for this Unity project.

---

## Overview

This project uses **Luban** for configuration management instead of traditional databases. Configuration tables are:
- Stored as JSON files in `StreamingAssets/Luban/`
- Generated into C# classes via Luban tool
- Loaded via `ConfigManager` and accessed through `Tables.Instance`

---

## Config System Architecture

### File Locations

```
StreamingAssets/Luban/
├── .bin/                   # Binary config files
├── .json/                  # JSON config files
│   ├── TbSkill.json       # Skill configurations
│   ├── TbBuff.json        # Buff configurations
│   ├── TbTreasure.json    # Treasure configurations
│   └── ...
└── Config/                 # Source config files (editors)

Assets/Scripts/Config/
├── ConfigManager.cs        # Config system manager
└── Config/                 # Generated C# classes
    ├── Tables.cs           # Main tables class
    ├── TbSkill.cs          # Skill config class
    └── ...
```

### Usage

```csharp
// Access config via Tables.Instance
var skillConfig = Tables.Instance.TbSkill.Get(skillId);

if (skillConfig == null)
{
    LogManager.E($"Skill config not found: {skillId}");
    return;
}

// Use config properties
var damage = skillConfig.Damage;
var name = skillConfig.Name;
```

---

## Config Types

### Available Config Tables

| Table | Purpose | Example |
|-------|---------|---------|
| TbSkill | Skill definitions | Damage, cost, effects |
| TbBuff | Buff definitions | Duration, modifiers |
| TbTreasure | Treasure/equipment | Stats, skills |
| TbHeartMethod | Heart methods (passive skills) | Passive effects |
| TbBattleMoment | Battle moment conditions | Trigger timing |
| TbScene | Scene configurations | Enemies, rewards |
| TbWeather | Weather effects | Battle modifiers |

---

## Query Patterns

### Get by ID

```csharp
var config = Tables.Instance.TbSkill.Get(id);
if (config == null)
{
    // Handle missing config
}
```

### Get All

```csharp
var allSkills = Tables.Instance.TbSkill.DataList;
foreach (var skill in allSkills)
{
    // Process skill
}
```

### Get by Condition

```csharp
var fireSkills = Tables.Instance.TbSkill.DataList
    .Where(s => s.Element == ElementType.Fire)
    .ToList();
```

---

## Best Practices

### Always Check for Null

```csharp
// Wrong
var damage = config.Damage;  // May crash if config is null

// Correct
var config = Tables.Instance.TbSkill.Get(skillId);
if (config == null)
{
    LogManager.E($"Config not found: {skillId}");
    return;
}
var damage = config.Damage;
```

### Use ConfigManager for Runtime Config

```csharp
[Inject] private ConfigManager ConfigManager { get; set; }

// Access via DI
var tables = ConfigManager.Tables;
```

### Don't Modify Config at Runtime

Configs are read-only. For runtime modifications:
- Use BattleUnit properties
- Use Buff system
- Use modifiers

---

## Adding New Config

### 1. Create Config File

Add new table in Luban editor:
```
StreamingAssets/Luban/Config/
├── TbNewFeature.json
```

### 2. Generate C# Classes

Run Luban tool to generate:
```
Assets/Scripts/Config/TbNewFeature.cs
```

### 3. Use in Code

```csharp
var config = Tables.Instance.TbNewFeature.Get(id);
```

---

## Common Mistakes

### 1. Not Checking Null

**Wrong:**
```csharp
var name = Tables.Instance.TbSkill.Get(9999).Name;
```

**Correct:**
```csharp
var skill = Tables.Instance.TbSkill.Get(9999);
if (skill == null)
{
    LogManager.W($"Skill 9999 not found");
    return;
}
var name = skill.Name;
```

### 2. Hardcoding Config IDs

**Wrong:**
```csharp
if (skillId == 1001) { ... }
```

**Correct:**
```csharp
// Use config data for logic
var config = Tables.Instance.TbSkill.Get(skillId);
if (config.Type == SkillType.Attack) { ... }
```

### 3. Modifying Config Data

**Wrong:**
```csharp
var config = Tables.Instance.TbSkill.Get(id);
config.Damage = 100;  // Don't do this!
```

**Correct:**
```csharp
// Use separate data model for runtime modifications
unit.AddDamageModifier(100);
```

---

## Config Helper Utilities

### ConfigHelper

Use helper methods in `Model/Util/ConfigHelper.cs`:
```csharp
var skill = ConfigHelper.GetSkill(skillId);
var buff = ConfigHelper.GetBuff(buffId);
```
