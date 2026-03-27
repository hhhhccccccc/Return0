# Component Guidelines

> How Unity UI components (Panels, Views) are built in this project.

---

## Overview

This project uses a View/Panel system with:
- **AutoFind** - Automatic component discovery via attributes
- **Dependency Injection** - Zenject for dependencies
- **Message System** - Event-driven communication

---

## Panel Structure

### Base Class

Extend `Panel` for UI panels:
```csharp
public class MyPanel : Panel
{
    // Auto-find components
    [AutoFind]
    private Button ConfirmButton;
    
    [AutoFind]
    private Text TitleText;
    
    [AutoFind]
    private Image BackgroundImage;
    
    // Injected dependencies
    [Inject] private IMessageManager MessageManager { get; set; }
    
    // Register events
    protected override void RegisterEvent()
    {
        // Subscribe to messages
        Register<MyEventModel>(OnMyEvent);
        
        // UI events
        ConfirmButton.onClick.AddListener(OnConfirm);
    }
    
    // Event handlers
    private void OnMyEvent(MyEventModel msg)
    {
        TitleText.text = msg.Title;
    }
    
    private void OnConfirm()
    {
        // Handle click
    }
}
```

---

## AutoFind Attribute

### Usage

```csharp
[AutoFind]                          // Find by property name
private Button ConfirmButton;

[AutoFind("CustomName")]            // Find by custom path
private Image Background;

[AutoFind(GetOrAdd = true)]         // Add component if not found
private MyComponent CustomComponent;
```

### Supported Components

- Unity UI: `Button`, `Text`, `Image`, `RawImage`, `Slider`, `Toggle`, etc.
- Custom: Any `MonoBehaviour` or `Component`

---

## Panel Lifecycle

### Callbacks

```csharp
public class MyPanel : Panel
{
    // Called when panel is created
    protected override void OnAwake()
    {
        base.OnAwake();
        // Initialization
    }
    
    // Called in Start()
    protected override void OnStart()
    {
        // Post-initialization setup
    }
    
    // Called when panel is shown
    public override void OnShow()
    {
        base.OnShow();
        // Refresh UI
    }
    
    // Called when panel is hidden
    public override void OnHide()
    {
        // Save state, cleanup
        base.OnHide();
    }
    
    // Register events
    protected override void RegisterEvent()
    {
        // Subscribe to messages
    }
    
    // Cleanup
    protected override void OnDestroy()
    {
        // Unsubscribe from messages
        base.OnDestroy();
    }
}
```

---

## Message Communication

### Receiving Messages

```csharp
protected override void RegisterEvent()
{
    Register<BattleStartEventModel>(OnBattleStart);
    Register<UnitHPChangeEventModel>(OnHPChange);
}

private void OnBattleStart(BattleStartEventModel msg)
{
    // Update UI
}
```

### Sending Messages

```csharp
// Dispatch message
DispatchMsg(new MyEventModel 
{ 
    Param1 = value1,
    Param2 = value2 
});
```

---

## Common Mistakes

### 1. Not Calling Base Methods

**Wrong:**
```csharp
protected override void OnAwake()
{
    // Missing base.OnAwake()
    DoSomething();
}
```

**Correct:**
```csharp
protected override void OnAwake()
{
    base.OnAwake();
    DoSomething();
}
```

### 2. Memory Leaks - Not Unsubscribing

**Wrong:**
```csharp
// In OnDestroy, forgot to clear events
```

**Correct:**
```csharp
// Use base.OnDestroy() - it handles cleanup
protected override void OnDestroy()
{
    base.OnDestroy();
}
```

### 3. Using Unity Debug

**Wrong:**
```csharp
Debug.Log("message");
```

**Correct:**
```csharp
Debug("message");  // Uses LogManager
```

### 4. Hardcoding Strings

**Wrong:**
```csharp
TitleText.text = "Battle";
```

**Correct:**
```csharp
// Use localization or config
TitleText.text = Localization.Get("battle_title");
```

---

## Best Practices

1. **Use `[AutoFind]`** for all UI component references
2. **Register events** in `RegisterEvent()` not `OnAwake()`
3. **Use message system** for cross-panel communication
4. **Clean up** in `OnDestroy()` - message subscriptions auto-clean via base
5. **Use LogManager** via `Debug()` / `Error()` helpers
6. **Follow naming** - `UI<Name>Panel` for panels
