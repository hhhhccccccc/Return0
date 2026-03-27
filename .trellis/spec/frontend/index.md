# Unity UI Development Guidelines

> Best practices for Unity UI development in this project.

---

## Overview

This directory contains guidelines for Unity UI development (Panels, Views, Components).

**Note**: This project uses Unity C# for both game logic and UI. The "frontend" in this context refers to Unity UI development.

---

## Guidelines Index

| Guide | Description | Status |
|-------|-------------|--------|
| [Directory Structure](./directory-structure.md) | UI folder organization | Required |
| [Component Guidelines](./component-guidelines.md) | Panel/View patterns | Required |
| [Quality Guidelines](./quality-guidelines.md) | UI code quality | Required |

---

## UI Architecture

This project uses:
- **View** base class - Auto-injection, message subscription
- **Panel** - UI panels with show/hide lifecycle
- **AutoFind** - Automatic component discovery
- **Message system** - UI communicates via messages

---

## Quick Start

```bash
# Read UI directory structure
cat .trellis/spec/frontend/directory-structure.md

# Read component patterns
cat .trellis/spec/frontend/component-guidelines.md

# Read code quality
cat .trellis/spec/frontend/quality-guidelines.md
```

---

**Language**: All documentation should be written in **English**.
