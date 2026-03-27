# Error Handling

> How errors are handled in this Unity C# project.

---

## Overview

This project uses a message-driven architecture with centralized error handling through:
- **LogManager** - Centralized logging
- **Message system** - Error event propagation
- **Unity exception handling** - Scene-level error catching

---

## Error Types

### 1. Logic Errors

Errors in game logic (invalid state, unexpected values):
```csharp
// Use Debug assertion in development
Debug.Assert(condition, "Invalid state");

// Use LogManager for errors
LogManager.E($"Invalid skill ID: {skillId}");
```

### 2. Resource Errors

Failed to load resources (prefabs, configs, etc.):
```csharp
// ResourceManager handles loading errors
var operation = ResourceManager.LoadAsync<GameObject>(path);
if (operation.Status == EOperationStatus.Failed)
{
    LogManager.E($"Failed to load: {path}, Error: {operation.Error}");
    yield break;
}
```

### 3. Config Errors

Missing or invalid configuration data:
```csharp
var config = Tables.Instance.TbSkill.Get(skillId);
if (config == null)
{
    LogManager.E($"Config not found for skill: {skillId}");
    return;
}
```

### 4. Runtime Errors

Unexpected exceptions during gameplay:
```csharp
try
{
    // Risky operation
}
catch (Exception e)
{
    LogManager.E(e);  // Use LogManager for exceptions
}
```

---

## Error Handling Patterns

### In Managers

Use try-catch and proper error reporting:
```csharp
protected override IEnumerator OnInit()
{
    try
    {
        // Initialization logic
        yield return LoadData();
    }
    catch (Exception e)
    {
        LogManager.E($"Init failed: {e}");
        // Handle gracefully - don't crash the game
    }
}
```

### In Controllers

Handle errors without breaking game flow:
```csharp
public override void Handle(MyEventModel msg)
{
    try
    {
        if (msg == null)
        {
            LogManager.E("Received null message");
            return;
        }
        
        // Process message
    }
    catch (Exception e)
    {
        LogManager.E($"Handle failed: {e}");
    }
}
```

### In Views

Show error UI and log:
```csharp
protected override void RegisterEvent()
{
    Register<ErrorEventModel>(OnError);
}

private void OnError(ErrorEventModel msg)
{
    LogManager.E($"UI Error: {msg.Message}");
    // Optionally show error panel
}
```

---

## Logging Errors

### Use LogManager

**Correct:**
```csharp
LogManager.D("Debug message");
LogManager.I("Info message");
LogManager.W("Warning message");
LogManager.E("Error message");
LogManager.E(exception);
```

**Wrong:**
```csharp
Debug.Log("message");      // Don't use Unity Debug
Debug.LogError("error");   // Don't use Unity Debug
```

### Error Context

Always include meaningful context:
```csharp
// Good
LogManager.E($"Failed to use skill {skillId} on unit {unitId}: {e.Message}");

// Bad
LogManager.E("Error occurred");
```

---

## Common Mistakes

### 1. Swallowing Exceptions

**Wrong:**
```csharp
try { }
catch (Exception) { }  // Silent swallow - very bad!
```

**Correct:**
```csharp
try { }
catch (Exception e)
{
    LogManager.E(e);
    // Handle gracefully
}
```

### 2. Not Checking null

**Wrong:**
```csharp
var config = Tables.Instance.TbSkill.Get(id);
var value = config.Value;  // May be null!
```

**Correct:**
```csharp
var config = Tables.Instance.TbSkill.Get(id);
if (config == null)
{
    LogManager.E($"Config not found: {id}");
    return;
}
var value = config.Value;
```

### 3. Using Debug.Log for Errors

**Wrong:**
```csharp
Debug.LogError("This is an error");
```

**Correct:**
```csharp
LogManager.E("This is an error");
```

### 4. Not Handling Load Failures

**Wrong:**
```csharp
var op = ResourceManager.LoadAsync<GameObject>(path);
yield return op;
// Continue without checking status
```

**Correct:**
```csharp
var op = ResourceManager.LoadAsync<GameObject>(path);
yield return op;
if (op.Status == EOperationStatus.Failed)
{
    LogManager.E($"Load failed: {path}");
    yield break;
}
```

---

## Best Practices

1. **Always use LogManager** - Not Unity's Debug class
2. **Include context** - What failed, with which parameters
3. **Don't crash silently** - Log before returning
4. **Graceful degradation** - Don't let one error crash the game
5. **Catch specific exceptions** - Don't catch all exceptions unless necessary
