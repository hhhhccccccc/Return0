# Quality Guidelines

> Code quality standards for Unity C# development.

---

## Overview

This project follows Unity best practices with Zenject dependency injection. All code should be:
- **Testable** - Use dependency injection for easy mocking
- **Maintainable** - Clear naming and structure
- **Performant** - Optimize for game performance

---

## Required Patterns

### Dependency Injection

**Always use `[Inject]` for dependencies:**
```csharp
public class MyController : ControllerBase<MyEventModel>
{
    [Inject] private MyManager MyManager { get; set; }
    [Inject] private IMessageManager MessageManager { get; set; }
}
```

**Never use `new` for Manager/Service classes** - Always inject via constructor or property.

### Message System

**Register messages in constructor or RegisterEvent:**
```csharp
// In SingleModel or View
protected IDisposable Register<T>(Action<T> action) where T : MessageModel
{
    return MessageManager.Register<T>(action);
}

// In Controller
public class MyController : ControllerBase<MyEventModel>
{
    public override void Handle(MyEventModel msg) { }
}
```

**Dispatch messages:**
```csharp
MessageManager.DispatchMsg(new MyEventModel { Param = value });
```

### Object Pooling

**Implement `IRecycle` for frequently created objects:**
```csharp
public class MyData : IRecycle
{
    public void OnRecycle()
    {
        // Reset state for reuse
    }
}
```

**Use PoolManager:**
```csharp
// Get from pool
var data = PoolManager.GetClass<MyData>();
// Return to pool
PoolManager.RecycleClass(data);
```

---

## Forbidden Patterns

### 1. Avoid UnityEngine.Debug directly

**Wrong:**
```csharp
Debug.Log("message");
Debug.LogError("error");
```

**Correct:**
```csharp
// In Manager/Controller/View
LogManager.D("message");  // Debug
LogManager.E("error");    // Error

// Or use protected helpers in View/Controller
Debug("message");
Error("error");
```

### 2. Avoid hardcoded paths

**Wrong:**
```csharp
var prefab = Resources.Load("Prefabs/MyPrefab");
```

**Correct:**
```csharp
// Use ResourceManager
ResourceManager.Load<GameObject>("Assets/GameResource/Prefab/MyPrefab");
```

### 3. Avoid direct instantiation

**Wrong:**
```csharp
var obj = new GameObject("Name");
var component = gameObject.AddComponent<MyComponent>();
```

**Wrong (for Managers):**
```csharp
var manager = new MyManager();  // No!
```

**Correct:**
```csharp
// For GameObjects in scene - use PoolManager
PoolManager.GetGameObject(path, go => { ... });

// For Managers - use DI
[Inject] private MyManager MyManager;
```

### 4. Avoid public fields

**Wrong:**
```csharp
public class MyClass
{
    public int Value;
}
```

**Correct:**
```csharp
public class MyClass
{
    public int Value { get; set; }
}
```

### 5. Avoid string literals for identification

**Wrong:**
```csharp
if (type == "battle") { }
```

**Correct:**
```csharp
// Use enums
if (type == MyType.Battle) { }
```

---

## Unity-Specific Guidelines

### Coroutines

**Use JobManager for coroutines:**
```csharp
// In Manager with yield return
protected override IEnumerator OnInit()
{
    var operation = ResourceManager.LoadAsync<GameObject>(path);
    yield return operation;
}
```

### Update Loop

**Implement IUpdate interface:**
```csharp
public class MyManager : ManagerBase, IUpdate
{
    public void OnUpdate(float dt)
    {
        // Per-frame logic
    }
}
```

### MonoBehaviour

**Use MonoSingleton for scene-persistent objects:**
```csharp
public class MySingleton : MonoSingleton<MySingleton>
{
    // Automatically persists across scenes
}
```

---

## Code Review Checklist

### Must Check

- [ ] Uses `[Inject]` for all dependencies (no `new` for Managers)
- [ ] Uses `LogManager` instead of `UnityEngine.Debug`
- [ ] No hardcoded strings for paths/types
- [ ] Implements `IRecycle` for frequently created objects
- [ ] Proper namespace usage (`App` namespace)
- [ ] Proper disposal of registered messages (`IDisposable`)
- [ ] Uses `yield break` for empty coroutines

### Recommended

- [ ] XML documentation for public APIs
- [ ] Proper null checks
- [ ] Constants instead of magic numbers
- [ ] LINQ usage is performant (avoid in Update loops)

---

## Performance Considerations

### Avoid in Update/Frame Logic

- LINQ queries (`Where`, `Select`, etc.)
- String concatenation in loops
- Garbage allocation (use pools)
- Heavy computations (cache results)

### Recommended

- Use structs for small data
- Cache component references
- Use object pools for frequent instantiation
- Profile with Unity Profiler
