# Task: createskill1031-1053

## Overview
Generate skill scripts for skill IDs 1031 to 1053 (inclusive) using the existing /trellis:generate-skill command.

## Requirements
- Generate all 23 skills from ID 1031 to 1053
- Each skill is generated based on its configuration in `tbbattleskillconfig.json`
- Use the generate-skill command with range format (e.g., 1031-1053)
- Skip existing scripts (default behavior) unless they need updating

## Acceptance Criteria
- [ ] All 23 skill scripts (1031-1053) are generated
- [ ] Each script has correct class name (Skill{ID})
- [ ] Each script inherits from BattleSkillBase
- [ ] All moment triggers are properly converted to override methods
- [ ] Conditions are directly written in methods (not using Manager.GetCondition)
- [ ] Effects are directly written in methods (not using Manager.TriggerMomentEffect)
- [ ] Base class封装 methods are used when available
- [ ] Scripts are placed in correct directory: `Assets/Scripts/Model/Battle/Logic/BattleSkill/`

## Technical Notes
- Use the /trellis:generate-skill skill from .opencode/commands/trellis/generate-skill.md
- The skill accepts range format: 1031-1053
- Default skip-existing=true to avoid overwriting good scripts

## Out of Scope
- Testing the generated skills in Unity editor
- Creating unit tests for skill scripts
- Modifying the generate-skill command itself