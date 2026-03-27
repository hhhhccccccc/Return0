# Workspace Index

> Records of all AI Agent work records across all developers

---

## Overview

This directory tracks records for all developers working with AI Agents on this Unity game project.

### File Structure

```
workspace/
|-- index.md              # This file - main index
+-- {developer}/          # Per-developer directory
    |-- index.md          # Personal index with session history
    |-- tasks/            # Task files
    |   |-- *.json        # Active tasks
    |   +-- archive/      # Archived tasks by month
    +-- journal-N.md     # Journal files (sequential: 1, 2, 3...)
```

---

## Active Developers

| Developer | Last Active | Sessions | Active File |
|-----------|-------------|----------|-------------|
| (none yet) | - | - | - |

---

## Getting Started

### For New Developers

Run the initialization script:

```bash
python3 ./.trellis/scripts/init_developer.py <your-name>
```

This will:
1. Create your identity file (gitignored)
2. Create your progress directory
3. Create your personal index
4. Create initial journal file

### For Returning Developers

1. Get your developer name:
   ```bash
   python3 ./.trellis/scripts/get_developer.py
   ```

2. Read your personal index:
   ```bash
   cat .trellis/workspace/$(python3 ./.trellis/scripts/get_developer.py)/index.md
   ```

---

## Project Context

This is a **Unity + Zenject** wuxia strategy game with:
- **Manager Layer** - Core systems (Message, Controller, Model, View, UI, Resource, Pool, Archive)
- **Controller Layer** - Business logic via message-driven patterns
- **Model Layer** - Data models (SingleModel, SingleArchiveModel, BattleUnit)
- **View Layer** - UI panels and scene views
- **Config Layer** - Luban-generated configuration tables

---

## Guidelines

### Journal File Rules

- **Max 2000 lines** per journal file
- When limit is reached, create `journal-{N+1}.md`
- Update your personal `index.md` when creating new files

### Session Record Format

Each session should include:
- Summary: One-line description
- Main Changes: What was modified
- Git Commits: Commit hashes and messages
- Next Steps: What to do next

---

## Session Template

Use this template when recording sessions:

```markdown
## Session {N}: {Title}

**Date**: YYYY-MM-DD
**Task**: {task-name}

### Summary

{One-line summary}

### Main Changes

- {Change 1}
- {Change 2}

### Git Commits

| Hash | Message |
|------|---------|
| `abc1234` | {commit message} |

### Testing

- [OK] {Test result}

### Status

[OK] **Completed** / # **In Progress** / [P] **Blocked**

### Next Steps

- {Next step 1}
- {Next step 2}
```

---

## Key References

Before starting development, read:

| Document | Purpose |
|----------|---------|
| `.trellis/spec/backend/index.md` | Backend development guidelines |
| `.trellis/spec/backend/directory-structure.md` | Project structure |
| `.trellis/spec/backend/quality-guidelines.md` | Code quality standards |
| `AGENTS.md` | Project architecture overview |

---

**Language**: All documentation must be written in **English**.
