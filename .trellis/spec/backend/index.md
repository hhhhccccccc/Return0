# Backend Development Guidelines

> Best practices for Unity C# backend development in this project.

---

## Overview

This directory contains guidelines for Unity C# backend development. This is a wuxia strategy game using:
- **Unity** - Game engine
- **Zenject** - Dependency injection
- **YooAsset** - Resource management
- **Luban** - Configuration table generation

---

## Guidelines Index

| Guide | Description | Status |
|-------|-------------|--------|
| [Directory Structure](./directory-structure.md) | Module organization and file layout | Required |
| [Config Guidelines](./database-guidelines.md) | Luban config table patterns | Required |
| [Error Handling](./error-handling.md) | Error types, handling strategies | Required |
| [Quality Guidelines](./quality-guidelines.md) | Code standards, forbidden patterns | Required |
| [Logging Guidelines](./logging-guidelines.md) | LogManager usage, log levels | Required |

---

## Core Architecture

### Layers

```
┌─────────────────────────────────────┐
│         Entry Layer                │
│   (AppManager / GameManager)       │
├─────────────────────────────────────┤
│         Manager Layer              │
│  (Message/Controller/Model/View/   │
│   UI/Resource/Pool/Archive/...)    │
├─────────────────────────────────────┤
│        Controller Layer            │
│    (Business Logic via Messages)   │
├─────────────────────────────────────┤
│          Model Layer                │
│   (SingleModel / BattleUnit / ...)  │
├─────────────────────────────────────┤
│          View Layer                 │
│        (UI Panels / Scenes)         │
└─────────────────────────────────────┘
```

### Key Patterns

1. **Dependency Injection** - Use `[Inject]` attribute with Zenject
2. **Message Driven** - Controller/View communicate via messages
3. **Object Pool** - Implement `IRecycle` for poolable objects
4. **Message Model** - Extend `MessageModel` for events

---

## How to Use These Guidelines

For each guideline file:

1. Read the conventions for your task type
2. Follow the patterns when writing code
3. Reference code examples from existing modules

---

**Language**: All documentation should be written in **English**.
