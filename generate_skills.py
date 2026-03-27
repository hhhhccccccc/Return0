import json
import sys

# 读取所有配置
skills = json.load(open('Assets/StreamingAssets/Luban/tbbattleskillconfig.json', 'r', encoding='utf-8'))
moments = json.load(open('Assets/StreamingAssets/Luban/tbbattlemomentconfig.json', 'r', encoding='utf-8'))
effects = json.load(open('Assets/StreamingAssets/Luban/tbbattlemomenteffectconfig.json', 'r', encoding='utf-8'))
conditions = json.load(open('Assets/StreamingAssets/Luban/tbbattlemomentconditionconfig.json', 'r', encoding='utf-8'))

# 建立索引
moment_dict = {m['id']: m for m in moments}
effect_dict = {e['id']: e for e in effects}
condition_dict = {c['id']: c for c in conditions}

# Moment字段映射
MOMENT_FIELDS = {
    'DoDesitionMoment': ('DoDesitionAction', 'bool isPreDesition'),
    'ActionWheelStartMoment': ('SelfActionWheelStart', ''),
    'BeforeActionMoment': ('BeforeAction', 'MomentParamModel paramModel'),
    'BeforeUnderActionMoment': ('BeforeUnderAction', 'MomentParamModel paramModel'),
    'BeforeClashMoment': ('BeforeClash', 'MomentParamModel paramModel'),
    'AfterClashMoment': ('AfterClash', 'MomentParamModel paramModel'),
    'ReleaseSkillActionMoment': ('ReleaseSkillAction', 'MomentParamModel paramModel'),
    'AfterUnderActionMoment': ('AfterUnderAction', 'MomentParamModel paramModel'),
    'AfterActionMoment': ('AfterAction', 'MomentParamModel paramModel'),
    'RoundEndMoment': ('RoundEnd', 'MomentParamModel paramModel'),
    'SkillEndMoment': ('SkillEnd', ''),
}

# Effect处理函数
def get_effect_code(effect):
    effect_name = effect.get('EffectName', '')
    param_list = effect.get('ParamList', [])
    
    # ChangeActionWheel: [target, value]
    if effect_name == 'ChangeActionWheel':
        target = param_list[0]
        value = param_list[1]
        if target == 1:
            return f'Subject.ChangeActionWheel({value});'
        return f'// TODO: ChangeActionWheel target={target}'
    
    # AddBuff: [caster, target, buffID, layerCount]
    if effect_name == 'AddBuff':
        caster = param_list[0]
        target = param_list[1]
        buff_id = param_list[2]
        layers = param_list[3]
        if target == 1:
            return f'DoAddBuff(Subject, {buff_id}, Subject, {layers}, null, BattleMomentType.ReleaseSkillAction);'
        elif target == 2:
            return f'if (Target != null) DoAddBuff(Target, {buff_id}, Subject, {layers}, null, BattleMomentType.ReleaseSkillAction);'
        return f'// TODO: AddBuff [caster={caster}, target={target}]'
    
    # AddRandomKey: [target, count, reason]
    if effect_name == 'AddRandomKey':
        target = param_list[0]
        count = param_list[1]
        reason = param_list[2]
        if target == 1:
            return f'Subject.AddRandomKey({count}, (ChangeKeyReason){reason});'
        elif target == 2:
            return f'if (Target != null) Target.AddRandomKey({count}, (ChangeKeyReason){reason});'
        return f'// TODO: AddRandomKey target={target}'
    
    # ChangeProperty: [target, propertyType, value, source]
    if effect_name == 'ChangeProperty':
        target = param_list[0]
        prop_type = param_list[1]
        value = param_list[2]
        if target in [1, 2]:  # 1=自己, 2=目标
            target_var = 'Subject' if target == 1 else 'Target'
            if prop_type == 20031:
                return f'{target_var}.ChangeProperty_Abs(BattlePropertyType.GangQi, {value});'
            elif prop_type == 20051:
                return f'{target_var}.ChangeProperty_Abs(BattlePropertyType.XuanQi, {value});'
            elif prop_type == 20103:
                return f'{target_var}.ChangeProperty_Abs(BattlePropertyType.Hp, {value});'
            elif prop_type == 20063:
                return f'{target_var}.ChangeProperty_Abs(BattlePropertyType.Neili, {value});'
            elif prop_type == 20043:
                return f'{target_var}.ChangeProperty_Abs(BattlePropertyType.Physique, {value});'
            return f'// TODO: ChangeProperty propType={prop_type}'
        return f'// TODO: ChangeProperty target={target}'
    
    # SetProperty: [target, propertyType, value, source]
    if effect_name == 'SetProperty':
        target = param_list[0]
        prop_type = param_list[1]
        value = param_list[2]
        if target == 1:
            if prop_type == 20031:
                return f'Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, {value});'
            return f'// TODO: SetProperty propType={prop_type}'
        return f'// TODO: SetProperty target={target}'
    
    # DamageToTargetByProperty: [caster, target, propertyType, pct, minDmg, source]
    if effect_name == 'DamageToTargetByProperty':
        return f'// TODO: DamageToTargetByProperty'
    
    # GetShieldBuffByTechPct: [target, pct]
    if effect_name == 'GetShieldBuffByTechPct':
        target = param_list[0]
        pct = param_list[1]
        if target == 1:
            return f'var tech = Subject.GetProperty(BattlePropertyType.Tech); BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (tech * {pct}).ToInt(), null, BattleMomentType.ReleaseSkillAction);'
        return f'// TODO: GetShieldBuffByTechPct target={target}'
    
    # ReturnSkillCostKey: [target]
    if effect_name == 'ReturnSkillCostKey':
        target = param_list[0]
        if target == 1:
            return f'// TODO: ReturnSkillCostKey - 返还技能消耗的键'
        return f'// TODO: ReturnSkillCostKey target={target}'
    
    # AddActionTimes: [target, count]
    if effect_name == 'AddActionTimes':
        target = param_list[0]
        count = param_list[1]
        if target == 1:
            return f'Subject.AddActionTimes({count});'
        return f'// TODO: AddActionTimes target={target}'
    
    # ClearBuffByType: [target, buffType, clearLayers]
    if effect_name == 'ClearBuffByType':
        target = param_list[0]
        buff_type = param_list[1]
        clear_layers = param_list[2]
        if target == 1:
            return f'DoClearBuffByType(Subject, {buff_type}, {clear_layers});'
        return f'// TODO: ClearBuffByType target={target}'
    
    # ClearAbnormalBuffAndAddGainBuff: [target, buffCount, gainBuffID]
    if effect_name == 'ClearAbnormalBuffAndAddGainBuff':
        target = param_list[0]
        buff_count = param_list[1]
        gain_buff_id = param_list[2]
        if target == 1:
            return f'DoConvertBuffAbnormalToGain(Subject, {buff_count}, {gain_buff_id});'
        return f'// TODO: ClearAbnormalBuffAndAddGainBuff target={target}'
    
    # GetShieldBuffByPowerPct: [target, pct]
    if effect_name == 'GetShieldBuffByPowerPct':
        target = param_list[0]
        pct = param_list[1]
        if target == 1:
            return f'DoGetShieldBuff(Subject, {pct}, BattleMomentType.AfterAction);'
        return f'// TODO: GetShieldBuffByPowerPct target={target}'
    
    # ChangeSkillXuanQiCostByUnitRes: [target, pct, maxCost]
    if effect_name == 'ChangeSkillXuanQiCostByUnitRes':
        target = param_list[0]
        pct = param_list[1]
        max_cost = param_list[2]
        if target == 1:
            return f'Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * {pct}, {max_cost}));'
        return f'// TODO: ChangeSkillXuanQiCostByUnitRes target={target}'
    
    # ChangeSkillGangQiCostByUnitRes: [target, pct, maxCost]
    if effect_name == 'ChangeSkillGangQiCostByUnitRes':
        target = param_list[0]
        pct = param_list[1]
        max_cost = param_list[2]
        if target == 1:
            return f'Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * {pct}, {max_cost}));'
        return f'// TODO: ChangeSkillGangQiCostByUnitRes target={target}'
    
    # HealXuanQiPctByCurr: [target, pct, minHeal]
    if effect_name == 'HealXuanQiPctByCurr':
        target = param_list[0]
        pct = param_list[1]
        min_heal = param_list[2]
        if target == 1:
            return f'var currXuanQi = Subject.GetProperty(BattlePropertyType.XuanQi); Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, Math.Max((int)(currXuanQi * {pct}), {min_heal}));'
        return f'// TODO: HealXuanQiPctByCurr target={target}'
    
    # HealGangQiPctByCurr: [target, pct, minHeal]
    if effect_name == 'HealGangQiPctByCurr':
        target = param_list[0]
        pct = param_list[1]
        min_heal = param_list[2]
        if target == 1:
            return f'var currGangQi = Subject.GetProperty(BattlePropertyType.GangQi); Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, Math.Max((int)(currGangQi * {pct}), {min_heal}));'
        return f'// TODO: HealGangQiPctByCurr target={target}'
    
    # ReturnSkillResourceCost: [target, gangQiPct, xuanQiPct, returnKey]
    if effect_name == 'ReturnSkillResourceCost':
        return f'Subject.GetSkill()?.ReturnSkillResourceCost(true, true, true);'
    
    # AddRandomBuff: [target, buffID, layerCount]
    if effect_name == 'AddRandomBuff':
        target = param_list[0]
        buff_id = param_list[1]
        layers = param_list[2]
        if target == 1:
            return f'// TODO: AddRandomBuff - 随机添加Buff {buff_id} {layers}层'
        return f'// TODO: AddRandomBuff target={target}'
    
    # ChangeHpByAttackDamage: [target, propertyType, pct, minDmg, source]
    if effect_name == 'ChangeHpByAttackDamage':
        return f'// TODO: ChangeHpByAttackDamage'
    
    # RecoverRoundBeDamagePct: [target, pct]
    if effect_name == 'RecoverRoundBeDamagePct':
        target = param_list[0]
        pct = param_list[1]
        if target == 1:
            return f'// TODO: RecoverRoundBeDamagePct {pct}'
        return f'// TODO: RecoverRoundBeDamagePct target={target}'
    
    # RemoveRandomKeyWithActionTimes: [target, keyCount, actionTimes]
    if effect_name == 'RemoveRandomKeyWithActionTimes':
        target = param_list[0]
        key_count = param_list[1]
        action_times = param_list[2]
        if target == 1:
            return f'// TODO: RemoveRandomKeyWithActionTimes {key_count}键 {action_times}次'
        return f'// TODO: RemoveRandomKeyWithActionTimes target={target}'
    
    # ChangeTargetRandom: []
    if effect_name == 'ChangeTargetRandom':
        return f'// TODO: ChangeTargetRandom'
    
    # ChangeTargetToOther: [target, newTarget]
    if effect_name == 'ChangeTargetToOther':
        return f'// TODO: ChangeTargetToOther'
    
    # AddGainBuffByBuffIDCount: [target, buffID, count, gainBuffID]
    if effect_name == 'AddGainBuffByBuffIDCount':
        target = param_list[0]
        buff_id = param_list[1]
        count = param_list[2]
        gain_buff_id = param_list[3]
        if target == 1:
            return f'// TODO: AddGainBuffByBuffIDCount buffID={buff_id} count={count} gain={gain_buff_id}'
        return f'// TODO: AddGainBuffByBuffIDCount target={target}'
    
    # ChangeNearlyBeActionTargetToTeamOther
    if effect_name == 'ChangeNearlyBeActionTargetToTeamOther':
        return f'// TODO: ChangeNearlyBeActionTargetToTeamOther'
    
    # RemoveAllKeyAndAddAllKey: [target, count]
    if effect_name == 'RemoveAllKeyAndAddAllKey':
        target = param_list[0]
        count = param_list[1]
        if target == 1:
            return f'DoRemoveAllKeyAndAddAllKey(Subject, {count});'
        return f'// TODO: RemoveAllKeyAndAddAllKey target={target}'
    
    return f'// TODO: {effect_name}'

# 解析参数
if len(sys.argv) < 2:
    print("Usage: python generate_skills.py <start-id>-<end-id> [existing_skill_ids...]")
    print("Example: python generate_skills.py 2001-2020")
    sys.exit(1)

# 解析范围
range_arg = sys.argv[1]
if '-' in range_arg:
    start_id, end_id = map(int, range_arg.split('-'))
else:
    start_id = end_id = int(range_arg)

# 解析existing列表
existing = []
if len(sys.argv) > 2:
    existing = [int(x) for x in sys.argv[2].split(',')] if ',' in sys.argv[2] else [int(sys.argv[2])]

# 生成技能脚本
output_dir = 'Assets/Scripts/Model/Battle/Logic/BattleSkill'

generated_count = 0
skipped_count = 0

for skill in skills:
    sid = skill.get('id', 0)
    if not (start_id <= sid <= end_id):
        continue
    
    if sid in existing:
        print(f'Skip {sid}: already exists')
        skipped_count += 1
        continue
    
    # 检查是否有moments
    has_any_moment = False
    for key in MOMENT_FIELDS.keys():
        if skill.get(key):
            moment_ids = skill.get(key, [])
            for mid in moment_ids:
                if mid in moment_dict:
                    m = moment_dict[mid]
                    if m.get('SuccessMomentEffect') or m.get('FailMomentEffect'):
                        has_any_moment = True
                        break
    
    if not has_any_moment:
        print(f'Skip {sid}: no moments')
        skipped_count += 1
        continue
    
    # 生成代码
    lines = []
    lines.append('using System.Collections.Generic;')
    lines.append('using Zenject;')
    lines.append('')
    lines.append(f'public class Skill{sid} : BattleSkillBase')
    lines.append('{')
    
    # 收集所有effects，按method分组 (同一个method只生成一个override)
    method_effects = {}  # key: (method, params), value: [(moment_id, effect_id, effect), ...]
    
    for field, (method, params) in MOMENT_FIELDS.items():
        moment_ids = skill.get(field, [])
        if not moment_ids:
            continue
        
        for mid in moment_ids:
            if mid not in moment_dict:
                continue
            
            moment = moment_dict[mid]
            success_effects = moment.get('SuccessMomentEffect', [])
            fail_effects = moment.get('FailMomentEffect', [])
            
            if not success_effects and not fail_effects:
                continue
            
            has_any_moment = True
            
            key = (method, params)
            if key not in method_effects:
                method_effects[key] = []
            
            # 添加所有success effects
            for eid in success_effects:
                if eid in effect_dict:
                    method_effects[key].append((mid, eid, effect_dict[eid]))
    
    # 按method分组生成代码
    for (method, params), effects in method_effects.items():
        # 生成方法定义
        if params:
            lines.append(f'    public override void {method}({params})')
        else:
            lines.append(f'    public override void {method}()')
        
        lines.append('    {')
        
        # 调用基类
        if params:
            param_name = params.split(" ")[-1]
            lines.append(f'        base.{method}({param_name});')
        else:
            lines.append(f'        base.{method}();')
        
        # 生成这个method的所有效果代码
        for mid, eid, effect in effects:
            effect_name = effect.get('EffectName', '')
            param_list = effect.get('ParamList', [])
            
            # 检查是否是AddBuff且target=4 (交锋目标)
            if effect_name == 'AddBuff' and len(param_list) >= 2 and param_list[1] == 4:
                buff_id = param_list[2]
                layers = param_list[3]
                lines.append(f'        // 效果: {eid} - {effect.get("EffectName")}')
                lines.append(f'        if (paramModel is DamageParamModel dm)')
                lines.append(f'        {{')
                lines.append(f'            var otherID = dm.GetOtherID(Subject.EntityID);')
                lines.append(f'            var otherUnit = BattleManager.GetUnit(otherID);')
                lines.append(f'            if (otherUnit != null)')
                lines.append(f'            {{')
                lines.append(f'                DoAddBuff(otherUnit, {buff_id}, Subject, {layers}, null, BattleMomentType.BeforeClash);')
                lines.append(f'            }}')
                lines.append(f'        }}')
            else:
                code = get_effect_code(effect)
                lines.append(f'        // 效果: {eid} - {effect.get("EffectName")}')
                lines.append(f'        {code}')
        
        lines.append('    }')
        lines.append('')
    
    lines.append('}')
    
    # 写入文件
    filepath = f'{output_dir}/Skill{sid}.cs'
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write('\n'.join(lines))
    
    print(f'Generated: Skill{sid}.cs')
    generated_count += 1

print(f'\nTotal: generated={generated_count}, skipped={skipped_count}')