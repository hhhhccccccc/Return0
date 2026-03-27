# UI Quality Guidelines

> Code quality standards for Unity UI development.

---

## Overview

UI code follows the same standards as backend code, with additional UI-specific guidelines.

---

## Required Patterns

### 1. AutoFind for Components

**Always use `[AutoFind]` for UI components:**
```csharp
[AutoFind]
private Button ConfirmButton;

[AutoFind]
private Text InfoText;
```

### 2. RegisterEvent Pattern

**Register all events in `RegisterEvent()`:**
```csharp
protected override void RegisterEvent()
{
    Register<MyEventModel>(OnEvent);
    ConfirmButton.onClick.AddListener(OnClick);
}
```

### 3. Message-Driven Updates

**Update UI via messages, not direct calls:**
```csharp
// Good - via message
Register<HPChangeEventModel>(OnHPChange);

private void OnHPChange(HPChangeEventModel msg)
{
    HPText.text = msg.CurrentHP.ToString();
}
```

---

## Forbidden Patterns

### 1. Direct Component Access

**Wrong:**
```csharp
// Finding components manually
var btn = transform.Find("Button").GetComponent<Button>();
```

**Correct:**
```csharp
[AutoFind]
private Button MyButton;
```

### 2. Hardcoded Strings

**Wrong:**
```csharp
TitleText.text = "Start Battle";
```

**Correct:**
```csharp
// Use constants or localization
TitleText.text = GameConst.BattleStartTitle;
```

### 3. Using Unity Debug

**Wrong:**
```csharp
Debug.Log("Clicked");
```

**Correct:**
```csharp
Debug("Clicked");  // Uses LogManager
```

### 4. Memory Leaks

**Always unsubscribe or use base methods:**
```csharp
protected override void OnDestroy()
{
    base.OnDestroy();  // Handles cleanup
}
```

---

## Code Review Checklist

### UI-Specific Checks

- [ ] All UI components use `[AutoFind]`
- [ ] Events registered in `RegisterEvent()`
- [ ] No hardcoded strings (use constants/localization)
- [ ] Uses `Debug()` not `Debug.Log()`
- [ ] Calls `base.OnAwake()`, `base.OnDestroy()`
- [ ] Proper null checks for components

### General Checks

- [ ] Uses dependency injection (`[Inject]`)
- [ ] Proper namespace (`App`)
- [ ] No memory leaks
- [ ] Follows naming conventions

---

## Performance Considerations

### Do

- Cache component references with `[AutoFind]`
- Use message system for updates
- Hide unused panels (don't destroy)

### Don't

- Find components in Update
- Use `GetComponent` in loops
- Create/destroy panels frequently (use pool)
