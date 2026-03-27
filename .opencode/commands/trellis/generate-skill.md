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

> **重要**: 生成脚本时，**不要删除原有的代码**！
> - 如果脚本已存在，只更新需要生成的部分
> - 保留原有的 using 语句和类定义
> - 如果方法已存在需要更新，确保保留原有逻辑
> - **不能偷懒**：找得到的配置一定要写出来，找不到才留 TODO

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

### 3. Process Each Moment

For each moment ID in the skill config:

1. Read `tbbattlemomentconfig.json` to find the moment
2. If `ConditionID` exists → generate condition check code
3. If `SuccessMomentEffect` exists → generate effect trigger code
4. If `FailMomentEffect` exists → generate fail effect code

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
=== 生成完成 ===

成功: X 个
跳过: X 个 (配置不存在/脚本已存在)
TODO: X 个

TODO 列表:
- Skill{ID1}: 效果{ID} - {效果名} - {描述}
- Skill{ID2}: 条件{ID} - {条件名} - {描述}
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

## 代码生成模式

> **重要**: 生成代码时，必须将 Condition 和 Effect 的逻辑**直接写入**方法中！
> - 优先使用 `BattleSkillBase` 基类中封装的常用方法
> - 如果基类中没有封装，再直接写入逻辑
> - 禁止使用 BattleMomentConditionManager.GetCondition 或 BattleMomentEffectManager.TriggerMomentEffect！

### 多个Effect的处理

> **注意**: 如果一个 Moment 的 `SuccessMomentEffect` 或 `FailMomentEffect` 包含**多个效果ID**，把它们**全部写进同一个 override 方法**中，不需要分开成多个方法。

示例：
```csharp
// Moment: 1006003 → SuccessMomentEffect: [100006, 3810001]
public override void AfterAction(MomentParamModel paramModel)
{
    base.AfterAction(paramModel);
    // 效果: 100006 - ChangeProperty
    Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 10);
    // 效果: 3810001 - GetArmorBuffByPowerPct
    DoGetArmorBuff(Subject, 1.0f, BattleMomentType.AfterAction);
}
```

### 基类封装方法

#### Condition 检查方法

| 方法 | 说明 |
|------|------|
| `CheckBeActionInBeforeActionWheel(target, offset, includeNow)` | 检查目标是否在前offset息内被调用 |
| `CheckSkillKillingStyle(target, isKilling)` | 检查目标技能是否为杀式 |

#### Effect 执行方法

| 方法 | 说明 |
|------|------|
| `DoSetActionWheelToNow(target)` | 设置目标到当前息 |
| `DoAddBuff(target, buffID, spellCaster, layerCount, paramList, momentType)` | 添加Buff |
| `DoAddRandomKey(target, count, reason)` | 添加随机键 |
| `DoChangeProperty(target, propertyType, value)` | 恢复属性（刚气/玄气） |
| `DoChangeActionWheel(target, value)` | 加快息 |
| `DoGetShieldBuff(target, pct, momentType)` | 获取护体Buff |
| `DoGetArmorBuff(target, pct, momentType)` | 获取甲Buff |
| `DoChangeSkillGangQiCost(target, pct, maxCost)` | 设置技能刚炁消耗 |
| `DoRemoveBuff(target, buffID)` | 移除指定Buff |

### 使用示例

#### Skill1001 (快速防守)

```csharp
using System.Collections.Generic;
using Zenject;

public class Skill1001 : BattleSkillBase
{
    // Moment: 1001003 → 条件: 500001 → 效果: 谁直接变到当前息
    public override void BeforeUnderAction(MomentParamModel paramModel)
    {
        base.BeforeUnderAction(paramModel);
        // 条件: 500001 → CheckBeActionInBeforeActionWheel
        // ParamList: [1, 2, 0] → 目标前2息被调用了，是否包含当前息（0不包含）
        if (CheckBeActionInBeforeActionWheel(Subject, 2, false))
        {
            // 效果: 3000001 - SetActionWheelToNow → 谁直接变到当前息
            DoSetActionWheelToNow(Subject);
        }
    }

    // Moment: 1001004 → 条件: 700041 → 效果: 交锋者招式获得的气减少100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // 条件: 700041 → CheckSkillKillingStyle → 判断交锋者是杀式
                if (CheckSkillKillingStyle(otherUnit, true))
                {
                    // 效果: 119000701 - AddBuff → 交锋者招式获得的气减少100
                    DoAddBuff(otherUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
                }
            }
        }
    }

    // Moment: 1001006 → 无条件 → 我获得1个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400001 - AddRandomKey → 我获得1个键
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}
```

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

## Common Effect Patterns

| Effect ID | Effect Class | Action |
|-----------|--------------|--------|
| 400001 | `BattleMomentEffect_AddRandomKey` | Add random keys |
| 400003 | `BattleMomentEffect_AddRandomKey` | Add random keys |
| 900002 | `BattleMomentEffect_ReleaseSkillAction` | Release skill action |
| 119000701 | `BattleMomentEffect_AddBuff` | Add buff |

## Output

Generate file: `Assets/Scripts/Model/Battle/Logic/BattleSkill/Skill{ID}.cs`

## Verification

After generating:
1. Check if class name matches skill ID
2. Verify all moment methods are correctly overridden
3. Verify condition checks are properly implemented
4. Verify effect triggers use correct effect IDs

---

## 常见问题与优化记录

### 问题1: ParamModel 未定义

**问题描述**: 在基类中添加的 `CheckDamageType` 方法使用了 `ParamModel`，但 `BattleSkillBase` 中未定义此属性。

**解决方案**: 
- 将 `paramModel` 作为方法参数传入，而非使用类成员变量
- 示例: `CheckDamageType(MomentParamModel paramModel, int targetIndex = 2, ...)`

---

### 问题2: 效果ID在配表中不存在

**问题描述**: 某些 Moment 配置的效果ID在 `tbbattlemomenteffectconfig.json` 中找不到。

**解决方案**:
- 先在配表中查找，确认是否存在
- 如果不存在，在代码中标记 TODO，待后续确认

---

### 问题3: 条件判断需要具体实现

**问题描述**: 某些条件如 `CheckBeDamageInSkillAction`（条件ID: 100001）需要检查技能对象的状态。

**解决方案**:
- 在基类中添加对应的检查方法
- 示例: `protected bool CheckBeDamageInSkillAction()` → `return Subject?.GetSkill()?.GetBeDamageInSkillAction() == true;`

---

### 问题4: 效果需要调用技能方法

**问题描述**: 某些效果如 `SetBeDamageInSkillAction`（3300001）需要调用技能对象的方法。

**解决方案**:
- 在基类中添加对应的执行方法
- 示例: `protected void DoSetBeDamageInSkillAction(BattleUnit target)` → `target.GetSkill()?.SetBeDamageInSkillAction();`

---

### 已封装的基类方法汇总

#### Condition 检查方法

| 方法 | 对应条件ID | 说明 |
|------|-----------|------|
| `CheckBeActionInBeforeActionWheel(target, offset, includeNow)` | 500001 | 检查目标是否在前offset息内被调用 |
| `CheckSkillKillingStyle(target, isKilling)` | 700041 | 检查目标技能是否为杀式 |
| `CheckMutualGoal(target)` | 1300001 | 检查是否互为目标 |
| `CheckDamageType(paramModel, targetIndex, isDirect, damageType)` | 1000011 | 检查伤害类型 |
| `CheckBeDamageInSkillAction()` | 100001 | 检查自己技能期间是否被打了 |
| `CheckSkillTriggerMoment(momentType)` | 1400001 | 检查技能是否经过特定时机 |
| `DoRandomAllKey(target, addCount)` | 600004 | 随机转化所有键 |
| `DoAddActionTimes(target, times)` | 3400001 | 添加行动次数 |
| `DoRemoveAllKeyAndAddAllKey(target, count)` | 4100002 | 移除所有键并添加各种键 |
| `DoChangeProperty(target, propertyType, value, source)` | 104001等 | 改变属性（支持百分比） |
| `DoConvertBuffAbnormalToGain(target, poolID, convertCount)` | 3900002 | 转换异常Buff为增益Buff |

#### Effect 执行方法

| 方法 | 对应效果ID | 说明 |
|------|-----------|------|
| `DoSetActionWheelToNow(target)` | 3000001 | 设置目标到当前息 |
| `DoAddBuff(...)` | 119000301, 119000701 | 添加Buff |
| `DoAddRandomKey(target, count, reason)` | 400001, 400003 | 添加随机键 |
| `DoChangeProperty(target, propertyType, value)` | 101001, 102003 | 恢复属性 |
| `DoChangeActionWheel(target, value)` | 2900001, 2900002 | 加快息 |
| `DoGetShieldBuff(target, pct, momentType)` | 3800001 | 获取护体 |
| `DoGetArmorBuff(target, pct, momentType)` | 3810001 | 获取甲 |
| `DoChangeSkillGangQiCost(target, pct, maxCost)` | 2300001 | 设置技能刚炁消耗 |
| `DoRemoveBuff(target, buffID)` | 19000301, 22001100 | 移除Buff |
| `DoSetBeDamageInSkillAction(target)` | 3300001 | 设置技能期间被打了 |

---

### 迭代日志

- **v1.0 (2026-03-26)**: 初始版本，支持基本Moment生成
- **v1.1**: 增加Condition和Effect直接写入方法的支持
- **v1.2**: 增加基类封装方法，统一调用方式
- **v1.3**: 修复ParamModel参数问题，增加CheckDamageType、CheckMutualGoal等方法
- **v1.4**: 增加CheckBeDamageInSkillAction、DoSetBeDamageInSkillAction方法