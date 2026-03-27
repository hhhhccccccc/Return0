# Logging Guidelines

> How logging is done in this Unity C# project.

---

## Overview

This project uses **LogManager** for centralized logging. Never use Unity's `Debug.Log` directly.

---

## LogManager Usage

### Log Levels

| Level | Method | Usage |
|-------|--------|-------|
| Debug | `LogManager.D(msg)` | Development debug info |
| Info | `LogManager.I(msg)` | General information |
| Warning | `LogManager.W(msg)` | Potential issues |
| Error | `LogManager.E(msg)` | Errors that need attention |

### In View/Controller

Use protected helpers:
```csharp
public class MyView : View
{
    private void SomeMethod()
    {
        Debug("Debug message");    // Shortcut to LogManager.D
        Error("Error message");    // Shortcut to LogManager.E
    }
}

public class MyController : ControllerBase<MyEventModel>
{
    public override void Handle(MyEventModel msg)
    {
        Debug("Handling event");
        Error("Something wrong");
    }
}
```

---

## What to Log

### Important Events

- **Game state changes**: Battle start/end, level transitions
- **Configuration loading**: Config files loaded, settings applied
- **User actions**: Button clicks, input events
- **Performance metrics**: Load times, frame rate drops
- **Errors**: Exceptions, failed operations, invalid data

### Example Logging

```csharp
// Battle events
LogManager.I($"Battle started: {battleId}");
LogManager.I($"Unit {unitId} used skill {skillId}");

// Resource loading
LogManager.D($"Loading config: {configPath}");
LogManager.I($"All configs loaded: {configCount} items");

// Errors
LogManager.W($"Missing skill config: {skillId}");
LogManager.E($"Failed to load prefab: {path}");
```

---

## What NOT to Log

### Sensitive Data

- **Player personal information**
- **Network request/response bodies** (may contain sensitive data)
- **File paths with user names**
- **Debug build information** in release builds

### Performance-Critical Code

Avoid logging in:
- **Update loops** (OnUpdate)
- **Every frame rendering**
- **Hot paths** (frequently called code)

```csharp
// Don't do this!
public void OnUpdate(float dt)
{
    Debug($"Position: {transform.position}");  // Too much logging!
}
```

---

## Structured Logging

### Include Context

Always include relevant context:
```csharp
// Good
LogManager.E($"Skill {skillId} failed: not enough qi, unit {unitId}");
LogManager.I($"Battle won: {battleId}, turns: {turnCount}");

// Bad
LogManager.E("Skill failed");
LogManager.I("Battle won");
```

### Use Format Strings

```csharp
// Good - easy to parse
LogManager.I($"BattleStart: battleId={battleId}, player={playerId}");

// Acceptable - readable
LogManager.I($"Starting battle {battleId} for player {playerId}");
```

---

## Best Practices

### DO

1. **Use LogManager** - Never use Unity's Debug class
2. **Include context** - What, where, why
3. **Use appropriate levels** - D for debug, I for info, W for warnings, E for errors
4. **Log errors with exceptions** - `LogManager.E(exception)`
5. **Be concise** - Don't log entire objects

### DON'T

1. **Don't use `Debug.Log`** - Use LogManager.D/I/W/E
2. **Don't log in Update loops** - Unless debugging
3. **Don't log sensitive data** - Player info, credentials
4. **Don't log verbose debug in production** - Use #if UNITY_EDITOR
5. **Don't forget to log errors** - Always log failures

---

## Log in Different Layers

### Manager Layer

```csharp
protected override IEnumerator OnInit()
{
    LogManager.D("MyManager initializing...");
    
    // ... initialization code ...
    
    LogManager.I("MyManager initialized");
}
```

### Controller Layer

```csharp
public override void Handle(BattleStartEventModel msg)
{
    LogManager.D($"BattleStart: {msg.BattleId}");
    
    // Handle event
}
```

### View Layer

```csharp
protected override void RegisterEvent()
{
    Register<BattleEndEventModel>(OnBattleEnd);
}

private void OnBattleEnd(BattleEndEventModel msg)
{
    Debug($"Battle ended: {msg.Result}");
}
```

### Model Layer

```csharp
public void TakeDamage(int damage)
{
    if (damage <= 0)
    {
        LogManager.W($"Invalid damage value: {damage}");
        return;
    }
    
    Health -= damage;
    LogManager.D($"Unit {UnitId} took {damage} damage, remaining: {Health}");
}
```
