# Generate Skill Script from Table Config

Generate C# skill scripts based on skill ID(s), parsing table configurations to code.

## Usage

```
/trellis:generate-skill <skill-id> [skip-existing]
```

**Parameters**:
- `skill-id`: Single ID (e.g., `1001`) or range (e.g., `1001-1010`)
- `skip-existing` (optional): 
  - `true` - Skip scripts that already exist (default)
  - `false` - Overwrite existing scripts

**Examples**:
```
# Generate single skill
/trellis:generate-skill 1002

# Generate skills 1001-1010, skip existing
/trellis:generate-skill 1001-1010

# Generate skills 1001-1010, overwrite existing
/trellis:generate-skill 1001-1010 false
```

## How It Works

This skill converts table-driven moment triggers into C# code:

```
Skill Config (JSON) → Moment Config → Condition/Effect → C# Code
```

### Data Flow

1. **Skill Config** (`tbbattleskillconfig.json`)
   - Contains moment trigger IDs (e.g., `ActionWheelStartMoment`, `BeforeClashMoment`)
   - Each moment ID points to a MomentConfig

2. **Moment Config** (`tbbattlemomentconfig.json`)
   - Contains `ConditionID` (optional) - condition to check
   - Contains `SuccessMomentEffect` - effect to run if condition passes
   - Contains `FailMomentEffect` - effect to run if condition fails

3. **Generated Code**
   - Override methods in `BattleSkillBase`
   - Check conditions if `ConditionID` exists
   - Trigger effects from `SuccessMomentEffect` / `FailMomentEffect`

## Execution Steps

### 1. Parse Input Parameters

**Single ID**: `1002` → Generate skill 1002

**Range**: `1001-1010` → Generate skills 1001, 1002, ..., 1010 (10 skills total)

**Skip Rules**:
1. If config not found for skill ID → Skip
2. If `skip-existing=true` and script already exists → Skip
3. If `skip-existing=false` → Overwrite

**Examples**:
- `1002` → Generate skill 1002 only
- `1001-1010 true` → Generate 1001-1010, skip if not found or exists
- `1001-1010 false` → Generate 1001-1010, overwrite existing

### 2. Read Skill Config

Read `tbbattleskillconfig.json` and find skill with matching ID.

> **Important**: When generating scripts, **DO NOT delete existing code**!
> - If script already exists, only update the parts that need to be generated
> - Preserve original using statements and class definitions
> - If method already exists and needs update, ensure to keep original logic
> - **DO NOT be lazy**: Must write all found configs, only leave TODO for not found ones

Extract all moment trigger fields:
- `DoDesitionMoment`
- `ActionWheelStartMoment`
- `BeforeActionMoment`
- `BeforeUnderActionMoment`
- `BeforeClashMoment`
- `AfterClashMoment`
- `ReleaseSkillActionMoment`
- `AfterUnderActionMoment`
- `AfterActionMoment`
- `RoundEndMoment`
- `SkillEndMoment`
- `DoDesitionMoment`

### 3. Process Each Moment

For each moment ID in the skill config:

1. Read `tbbattlemomentconfig.json` to find the moment
2. If `ConditionID` exists → generate condition check code
3. If `SuccessMomentEffect` exists → find each EffectID and convert to code

### 4. Generate Skill Script

Generate C# class inheriting from `BattleSkillBase`:

```csharp
using System.Collections.Generic;
using Zenject;

public class Skill{ID} : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // Generated from ActionWheelStartMoment
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // Generated from BeforeClashMoment
    }

    // ... other moments
}
```

### 5. Output Summary

After generation, output a summary of TODOs:

```
=== Generation Complete ===

Success: X
Skipped: X (config not found / script exists)
TODO: X

TODO List:
- Skill{ID1}: Effect{ID} - {EffectName} - {Description}
- Skill{ID2}: Condition{ID} - {ConditionName} - {Description}
```

## Moment → Method Mapping

| Moment Field | Override Method |
|--------------|-----------------|
| `DoDesitionMoment` | `DoDesitionAction(bool isPreDesition)` |
| `ActionWheelStartMoment` | `SelfActionWheelStart()` |
| `BeforeActionMoment` | `BeforeAction(MomentParamModel paramModel)` |
| `BeforeUnderActionMoment` | `BeforeUnderAction(MomentParamModel paramModel)` |
| `BeforeClashMoment` | `BeforeClash(MomentParamModel paramModel)` |
| `AfterClashMoment` | `AfterClash(MomentParamModel paramModel)` |
| `ReleaseSkillActionMoment` | `ReleaseSkillAction(MomentParamModel paramModel)` |
| `AfterUnderActionMoment` | `AfterUnderAction(MomentParamModel paramModel)` |
| `AfterActionMoment` | `AfterAction(MomentParamModel paramModel)` |
| `RoundEndMoment` | `RoundEnd(MomentParamModel paramModel)` |
| `SkillEndMoment` | `SkillEnd()` (no param) |

## Code Generation Patterns

> **Important**: When generating code, Condition and Effect logic must be written directly in methods!
> - Prefer using encapsulated methods in `BattleSkillBase` base class
> - If not encapsulated in base class, write logic directly
> - NEVER use BattleMomentConditionManager.GetCondition or BattleMomentEffectManager.TriggerMomentEffect!

### Base Class Encapsulated Methods

#### Condition Check Methods

| Method | Description |
|--------|-------------|
| `CheckBeActionInBeforeActionWheel(target, offset, includeNow)` | Check if target was called within offset action wheels |
| `CheckSkillKillingStyle(target, isKilling)` | Check if target skill is killing style |

#### Effect Execution Methods

| Method | Description |
|--------|-------------|
| `DoSetActionWheelToNow(target)` | Set target to current action wheel |
| `DoAddBuff(target, buffID, spellCaster, layerCount, paramList, momentType)` | Add buff |
| `DoAddRandomKey(target, count, reason)` | Add random keys |
| `DoChangeProperty(target, propertyType, value)` | Change property (GangQi/XuanQi) |
| `DoChangeActionWheel(target, value)` | Change action wheel |
| `DoGetShieldBuff(target, pct, momentType)` | Get shield buff |
| `DoGetArmorBuff(target, pct, momentType)` | Get armor buff |
| `DoChangeSkillGangQiCost(target, pct, maxCost)` | Set skill GangQi cost |
| `DoRemoveBuff(target, buffID)` | Remove buff |

### Usage Example

#### Skill1001 (Quick Defense)

```csharp
using System.Collections.Generic;
using Zenject;

public class Skill1001 : BattleSkillBase
{
    // Moment: 1001003 → Condition: 500001 → Effect: Set to current action wheel
    public override void BeforeUnderAction(MomentParamModel paramModel)
    {
        base.BeforeUnderAction(paramModel);
        // Condition: 500001 → CheckBeActionInBeforeActionWheel
        // ParamList: [1, 2, 0] → Target called within 2 action wheels, exclude current
        if (CheckBeActionInBeforeActionWheel(Subject, 2, false))
        {
            // Effect: 3000001 - SetActionWheelToNow
            DoSetActionWheelToNow(Subject);
        }
    }

    // Moment: 1001004 → Condition: 700041 → Effect: Add buff to clash target
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // Condition: 700041 → CheckSkillKillingStyle → Check if clash target is killing style
                if (CheckSkillKillingStyle(otherUnit, true))
                {
                    // Effect: 119000701 - AddBuff
                    DoAddBuff(otherUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
                }
            }
        }
    }

    // Moment: 1001006 → No condition → Get 1 random key
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // Effect: 400001 - AddRandomKey
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}
```

### 步骤1: 查找 Effect 配置

在 `tbbattlemomenteffectconfig.json` 中查找 EffectID，获取:
- `EffectName`: 效果类型 (如 `ChangeProperty`, `AddBuff`, `AddRandomKey`)
- `ParamList`: 参数列表
- `desc`: 效果描述

### 步骤2: 查找 Effect 脚本

在 `Assets/Scripts/Model/Battle/Logic/BattleMomentEffect/Impl/` 目录查找对应的 Effect 脚本:

| EffectName | 脚本文件名 |
|------------|-----------|
| `ChangeProperty` | `BattleMomentEffect_ChangeProperty.cs` |
| `AddBuff` | `BattleMomentEffect_AddBuff.cs` |
| `AddRandomKey` | `BattleMomentEffect_AddRandomKey.cs` |
| `ChangeActionWheel` | `BattleMomentEffect_ChangeActionWheel.cs` |
| `GetShieldBuffByPowerPct` | `BattleMomentEffect_GetShieldBuffByPowerPct.cs` |
| `GetArmorBuffByPowerPct` | `BattleMomentEffect_GetArmorBuffByPowerPct.cs` |
| `ChangeSkillGangQiCostByUnitRes` | `BattleMomentEffect_ChangeSkillGangQiCostByUnitRes.cs` |
| `RemoveBuff` | `BattleMomentEffect_RemoveBuff.cs` |
| `ClearBuffByType` | `BattleMomentEffect_ClearBuffByType.cs` |

### 步骤3: 转换逻辑到 Skill 方法

读取 Effect 脚本的 `OnEffect()` 方法，将逻辑转换为 Skill 方法中的代码。

---

### 示例: Simple Moment (无 Condition)

**MomentConfig**: `1002001` → `SuccessMomentEffect: [400003]`

1. 查找 EffectID `400003`:
   - `EffectName`: `AddRandomKey`
   - `ParamList`: `[1, 3, 4]` → 自己，获得3个键

2. 查找脚本: `BattleMomentEffect_AddRandomKey.cs`
   ```csharp
   protected override void OnEffect()
   {
       var targetList = GetUnitByParamID(Config.ParamList[0]);
       var count = Config.ParamList[1].ToInt() * BuffLayerCount;
       foreach (var target in targetList)
       {
           target.AddRandomKey(count, (ChangeKeyReason)Config.ParamList[2].ToInt());
       }
   }
   ```

3. 生成代码:
```csharp
// Moment: 1002001 → 无条件 → 添加随机键
public override void SelfActionWheelStart()
{
    base.SelfActionWheelStart();
    // 效果: 400003 - AddRandomKey [1, 3, 4] → 自己获得3个键
    Subject.AddRandomKey(3, ChangeKeyReason.SkillEffect);
}
```

---

### 示例: ChangeProperty 效果

**EffectID**: `101001` → `ChangeProperty`, Param: `[1, 20031, 15, 3]`
- `ParamList[0] = 1`: 自己
- `ParamList[1] = 20031`: 刚气
- `ParamList[2] = 15`: 数值
- `ParamList[3] = 3`: 招式

```csharp
// 效果: 101001 → 恢复刚气15
Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 15);
```

**EffectID**: `102003` → `ChangeProperty`, Param: `[1, 20051, 15, 3]`
```csharp
// 效果: 102003 → 恢复玄气15
Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 15);
```

---

### 示例: AddBuff 效果

**EffectID**: `111003101` → `AddBuff`, Param: `[1, 1, 10031, 1]`
- `ParamList[0] = 1`: 施法者
- `ParamList[1] = 1`: 目标
- `ParamList[2] = 10031`: BuffID
- `ParamList[3] = 1`: 层数

```csharp
// 效果: 111003101 → 添加反击Buff 10031, 1层
BattleBuffManager.AddBuff(Subject, 10031, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
```

---

### 示例: ChangeActionWheel 效果

**EffectID**: `2900001` → `ChangeActionWheel`, Param: `[1, 1]`
- `ParamList[0] = 1`: 自己
- `ParamList[1] = 1`: 加快1息

```csharp
// 效果: 2900001 → 自己加快1息
Subject.ChangeActionWheel(1);
```

---

### 示例: GetShieldBuffByPowerPct 效果

**EffectID**: `3800001` → `GetShieldBuffByPowerPct`, Param: `[1, 0.8]`
- `ParamList[0] = 1`: 自己
- `ParamList[1] = 0.8`: 80%力量

```csharp
// 效果: 3800001 → 获取80%力量的护体
var power = Subject.GetProperty(BattlePropertyType.Power);
BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (power * 0.8f).ToInt(), null, BattleMomentType.ReleaseSkillAction);
```

---

### 示例: GetArmorBuffByPowerPct 效果

**EffectID**: `3810001` → `GetArmorBuffByPowerPct`, Param: `[1]`
- `ParamList[0] = 1`: 100%力量

```csharp
// 效果: 3810001 → 获取100%力量的甲
var power = Subject.GetProperty(BattlePropertyType.Power);
BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, (power * 1.0f).ToInt(), null, BattleMomentType.AfterAction);
```

---

### 示例: ChangeSkillGangQiCostByUnitRes 效果

**EffectID**: `2300001` → `ChangeSkillGangQiCostByUnitRes`, Param: `[1, 0.5, 50]`
```csharp
// 效果: 2300001 → 招式的刚炁消耗转为当前50%，至多50
var skillBase = Subject.GetSkill();
if (skillBase != null)
{
    var curr = Subject.GetProperty(BattlePropertyType.GangQi);
    var cost = Math.Min(curr * 0.5f, 50);
    skillBase.SetGangQiCost(cost);
}
```

---

### 示例: RemoveBuff 效果

**EffectID**: `22001100` → `RemoveBuff`, Param: `[2, 20011, 0]`
- `ParamList[0] = 2`: 目标
- `ParamList[1] = 20011`: BuffID
- `ParamList[2] = 0`: 所有层数

```csharp
// 效果: 22001100 → 移除目标缓速20011
var buffs = Subject.GetBuffList();
foreach (var buff in buffs)
{
    if (buff.BuffID == 20011)
    {
        Subject.ClearBuff(20011);
    }
}
```

---

### 示例: 多个 Effect

**Moment**: `1007003` → `SuccessMomentEffect: [102002, 400002, 400012]`

```csharp
// Moment: 1007003 → 无条件 → 恢复玄气 + 双方获得键
public override void AfterAction(MomentParamModel paramModel)
{
    base.AfterAction(paramModel);
    
    // 效果: 102002 → 恢复玄气20
    Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 20);
    
    // 效果: 400002 → 自己获得2个键
    Subject.AddRandomKey(2, ChangeKeyReason.SkillEffect);
    
    // 效果: 400012 → 对方获得2个键
    if (Target != null)
    {
        Target.AddRandomKey(2, ChangeKeyReason.SkillEffect);
    }
}
```

---

### 示例: 有 Condition 的 Moment

**Moment**: `1001004` → `ConditionID: [700041]`, `SuccessMomentEffect: [119000701]`

1. 查找 Condition 配置 (`tbbattlemomentconditionconfig.json`):
   - `ConditionID`: `700041`
   - 获取 `ConditionName` 和 `ParamList`

2. 查找 Condition 脚本:
   在 `Assets/Scripts/Model/Battle/Logic/BattleMomentCondition/` 目录查找

3. 转换逻辑到代码中

**示例1**: ConditionID 700041 → CheckBuff
```csharp
// 条件: 700041 → 检查目标是否有特定Buff
var targetCheck = Subject; // 根据ParamList[0]=1
var buffID = 90007; // 根据ParamList
var hasCount = targetCheck.GetBuffCountByID(buffID);
var checkLevel = 1; // 根据ParamList
if (hasCount >= checkLevel) // 根据ParamList关系
{
    // 满足条件，给交锋目标添加增益Buff
    BattleBuffManager.AddBuff(otherID, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
}
```

**完整示例**:
```csharp
// Moment: 1001004 → 条件: 700041 → 效果: 判断成功后给交锋目标添加增益Buff
public override void BeforeClash(MomentParamModel paramModel)
{
    base.BeforeClash(paramModel);
    if (paramModel is DamageParamModel model)
    {
        var otherID = model.GetOtherID(Subject.EntityID);
        var otherUnit = BattleManager.GetUnit(otherID);
        if (otherUnit != null)
        {
            // 条件: 700041 → 检查目标是否有特定Buff
            // 查找Condition脚本，将逻辑直接写入
            var hasCount = otherUnit.GetBuffCountByID(90007);
            if (hasCount >= 1)
            {
                // 满足条件，给交锋目标添加增益Buff
                BattleBuffManager.AddBuff(otherID, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
            }
        }
    }
}
```

> **重要**: Condition 也要像 Effect 一样，直接将判断逻辑写入方法中，而不是调用 `BattleMomentConditionManager.GetCondition`！

### Condition → 脚本映射表

| ConditionName | 脚本文件名 | 说明 |
|---------------|-----------|------|
| `CheckBuff` | `BattleMomentCondition_CheckBuff.cs` | 检查Buff |
| `CheckProperty` | `BattleMomentCondition_CheckProperty.cs` | 检查属性 |
| `CheckKeyCount` | `BattleMomentCondition_CheckKeyCount.cs` | 检查键数量 |
| `CheckSkillType` | `BattleMomentCondition_CheckSkillType.cs` | 检查技能类型 |
| `CheckDamageType` | `BattleMomentCondition_CheckDamageType.cs` | 检查伤害类型 |
| `CheckRandomSuccess` | `BattleMomentCondition_CheckRandomSuccess.cs` | 随机判定 |
| `CheckBeDamageInSkillAction` | `BattleMomentCondition_CheckBeDamageInSkillAction.cs` | 技能期间被攻击 |
| `CheckWeather` | `BattleMomentCondition_CheckWeather.cs` | 检查天气 |

---

### 示例: 有 Fail Effect 的 Moment

**Moment**: `1004005` → `ConditionID: [1300001]`, `SuccessMomentEffect: [119000701]`, `FailMomentEffect: []`

```csharp
// Moment: 1004005 → 条件: 1300001 → 成功效果: 添加Buff / 失败效果: 无
public override void BeforeClash(MomentParamModel paramModel)
{
    base.BeforeClash(paramModel);
    if (paramModel is DamageParamModel model)
    {
        var otherID = model.GetOtherID(Subject.EntityID);
        var otherUnit = BattleManager.GetUnit(otherID);
        
        // 条件: 1300001 → 检查目标是否有特定Buff
        var hasCount = otherUnit.GetBuffCountByID(90007);
        if (hasCount >= 1) // 根据Condition ParamList判断
        {
            // 成功效果: 添加Buff
            BattleBuffManager.AddBuff(otherID, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
        }
        else
        {
            // 失败效果 (如果有)
            // ...
        }
    }
}
```

---

### 完整技能示例

以下是从真实配表生成的技能脚本示例：

#### Skill1006 (外炁屏障)

```csharp
using System;
using System.Collections.Generic;
using Zenject;

public class Skill1006 : BattleSkillBase
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
    // Moment: 1006001 → 无条件 → 招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300001 - ChangeSkillGangQiCostByUnitRes
        // ParamList: [1, 0.5, 50] → 自己，50%，至多50
        var skillBase = Subject.GetSkill();
        if (skillBase != null)
        {
            var curr = Subject.GetProperty(BattlePropertyType.GangQi);
            var pct = 0.5f;
            var cost = curr * pct;
            cost = Math.Min(cost, 50);
            skillBase.SetGangQiCost(cost);
        }
    }

    // Moment: 1006002 → 无条件 → 自己获取80%力的护体
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3800001 - GetShieldBuffByPowerPct
        // ParamList: [1, 0.8] → 自己，80%
        var power = Subject.GetProperty(BattlePropertyType.Power);
        var pct = 0.8f;
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (power * pct).ToInt(), null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1006003 → 无条件 → 获取100%力的甲
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        
        // 效果: 3810001 - GetArmorBuffByPowerPct
        // ParamList: [1] → 自己100%
        var power = Subject.GetProperty(BattlePropertyType.Power);
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, (power * 1.0f).ToInt(), null, BattleMomentType.AfterAction);
    }
}
```

#### Skill1007 (风传)

```csharp
using System.Collections.Generic;
using Zenject;

public class Skill1007 : BattleSkillBase
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
    // Moment: 1007001 → 无条件 → 自己加快2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900002 - ChangeActionWheel
        // ParamList: [1, 2] → 自己，加快2息
        Subject.ChangeActionWheel(2);
    }

    // Moment: 1007002 → 无条件 → 添加增益Buff + 移除异常Buff
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        
        // 效果: 121004102 - AddBuff
        // ParamList: [1, 2, 10041, 2] → 自己给目标添加迅速10041,2层
        BattleBuffManager.AddBuff(Subject, 10041, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 22001100 - RemoveBuff
        // ParamList: [2, 20011, 0] → 目标，缓速，所有层数
        var buffs = Subject.GetBuffList();
        foreach (var buff in buffs)
        {
            if (buff.BuffID == 20011)
            {
                Subject.ClearBuff(20011);
            }
        }
    }

    // Moment: 1007003 → 无条件 → 恢复玄气 + 双方获得键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        
        // 效果: 102002 - ChangeProperty (玄气)
        // ParamList: [1, 20051, 20, 3] → 自己，玄气，20，招式
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 20);
        
        // 效果: 400002 - AddRandomKey (自己获得2个键)
        // ParamList: [1, 2, 4] → 我获得2个键
        Subject.AddRandomKey(2, ChangeKeyReason.SkillEffect);
        
        // 效果: 400012 - AddRandomKey (对方获得2个键)
        // ParamList: [2, 2, 4] → 对方获得2个键
        if (Target != null)
        {
            Target.AddRandomKey(2, ChangeKeyReason.SkillEffect);
        }
    }
}
```

#### Skill1008 (临阵之志)

```csharp
using System.Collections.Generic;
using Zenject;

public class Skill1008 : BattleSkillBase
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
    // Moment: 1008001 → 无条件 → 自己加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900001 - ChangeActionWheel
        // ParamList: [1, 1] → 自己，加快1息
        Subject.ChangeActionWheel(1);
    }

    // Moment: 1008002 → 无条件 → 给自己添加反击10011,1层
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111003101 - AddBuff
        // ParamList: [1, 1, 10031, 1] → 自己给自己添加反击10011,1层
        BattleBuffManager.AddBuff(Subject, 10011, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1008003 → 无条件 → 3息内反击buff不会低于1层
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 119000904 - AddBuff
        // ParamList: [1, 1, 90009, 4] → 3息内反击buff不会低于1层
        BattleBuffManager.AddBuff(Subject, 90009, Subject, 4, null, BattleMomentType.AfterAction);
    }
}
```

#### Skill1009 (聚炁)

```csharp
using System.Collections.Generic;
using Zenject;

public class Skill1009 : BattleSkillBase
{
    // Moment: 1009001 → 无条件 → 恢复刚气 + 恢复玄气
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        
        // 效果: 101001 - ChangeProperty (刚气)
        // ParamList: [1, 20031, 15, 3] → 自己，刚气，15，招式
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 15);
        
        // 效果: 102003 - ChangeProperty (玄气)
        // ParamList: [1, 20051, 15, 3] → 自己，玄气，15招式
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 15);
    }
}
```

---

### 常见 EffectID 速查表

| EffectID | EffectName | 说明 |
|----------|------------|------|
| 101001 | ChangeProperty | 刚气+15 |
| 102003 | ChangeProperty | 玄气+15 |
| 400001 | AddRandomKey | 自己+1键 |
| 400002 | AddRandomKey | 自己+2键 |
| 400003 | AddRandomKey | 自己+3键 |
| 400012 | AddRandomKey | 对方+2键 |
| 2900001 | ChangeActionWheel | 加快1息 |
| 2900002 | ChangeActionWheel | 加快2息 |
| 3800001 | GetShieldBuffByPowerPct | 护体 |
| 3810001 | GetArmorBuffByPowerPct | 甲 |
| 2300001 | ChangeSkillGangQiCostByUnitRes | 刚炁消耗转化 |
| 22001100 | RemoveBuff | 移除Buff |

## Output

Generate file: `Assets/Scripts/Model/Battle/Logic/BattleSkill/Skill{ID}.cs`

## Verification

After generating:
1. Check if class name matches skill ID
2. Verify all moment methods are correctly overridden
3. Verify condition checks are properly implemented
4. Verify effect logic is directly implemented in methods (NOT TriggerMomentEffect)

---

## Common Issues and Optimization Records

### Issue 1: ParamModel Not Defined

**Problem**: The `CheckDamageType` method uses `ParamModel`, but it's not defined in `BattleSkillBase`.

**Solution**: Pass `paramModel` as method parameter instead of using class member variable.
- Example: `CheckDamageType(MomentParamModel paramModel, int targetIndex = 2, ...)`

---

### Issue 2: Effect ID Not Found in Config

**Problem**: Some Moment config effect IDs (e.g., 100005) don't exist in `tbbattlemomenteffectconfig.json`.

**Solution**: 
- Check in config first
- If not found, mark as TODO for later confirmation

---

### Issue 3: Condition Check Requires Implementation

**Problem**: Some conditions like `CheckBeDamageInSkillAction` (ID: 100001) need to check skill object state.

**Solution**: Add corresponding check method in base class
- Example: `protected bool CheckBeDamageInSkillAction()`

---

### Issue 4: Effect Requires Calling Skill Method

**Problem**: Some effects like `SetBeDamageInSkillAction` (3300001) need to call skill object methods.

**Solution**: Add corresponding execute method in base class
- Example: `protected void DoSetBeDamageInSkillAction(BattleUnit target)`

---

### Encapsulated Base Class Methods

#### Condition Check Methods

| Method | Condition ID | Description |
|--------|-------------|-------------|
| `CheckBeActionInBeforeActionWheel(target, offset, includeNow)` | 500001 | Check if target called within offset action wheels |
| `CheckSkillKillingStyle(target, isKilling)` | 700041 | Check if target skill is killing style |
| `CheckMutualGoal(target)` | 1300001 | Check if mutual target |
| `CheckDamageType(paramModel, targetIndex, isDirect, damageType)` | 1000011 | Check damage type |
| `CheckBeDamageInSkillAction()` | 100001 | Check if damaged in skill action |
| `CheckSkillTriggerMoment(momentType)` | 1400001 | Check if skill passed specific moment |
| `DoRandomAllKey(target, addCount)` | 600004 | Random all keys |
| `DoAddActionTimes(target, times)` | 3400001 | Add action times |
| `DoRemoveAllKeyAndAddAllKey(target, count)` | 4100002 | Remove all keys and add all types |

#### Effect Execution Methods

| Method | Effect ID | Description |
|--------|----------|-------------|
| `DoSetActionWheelToNow(target)` | 3000001 | Set target to current action wheel |
| `DoAddBuff(...)` | 119000301, 119000701 | Add buff |
| `DoAddRandomKey(target, count, reason)` | 400001, 400003 | Add random keys |
| `DoChangeProperty(target, propertyType, value)` | 101001, 102003 | Change property |
| `DoChangeActionWheel(target, value)` | 2900001, 2900002 | Change action wheel |
| `DoGetShieldBuff(target, pct, momentType)` | 3800001 | Get shield buff |
| `DoGetArmorBuff(target, pct, momentType)` | 3810001 | Get armor buff |
| `DoChangeSkillGangQiCost(target, pct, maxCost)` | 2300001 | Set skill GangQi cost |
| `DoRemoveBuff(target, buffID)` | 19000301, 22001100 | Remove buff |
| `DoSetBeDamageInSkillAction(target)` | 3300001 | Set be damaged in skill action |

---

### Changelog

- **v1.0 (2026-03-26)**: Initial version, basic Moment generation
- **v1.1**: Add Condition and Effect direct implementation support
- **v1.2**: Add base class encapsulation methods
- **v1.3**: Fix ParamModel parameter issue, add CheckDamageType, CheckMutualGoal
- **v1.4**: Add CheckBeDamageInSkillAction, DoSetBeDamageInSkillAction methods