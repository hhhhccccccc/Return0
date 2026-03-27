import json
import os
from collections import defaultdict

# 配置路径
LUBAN_DIR = r"D:\Project\Return0\Assets\StreamingAssets\Luban"

# 读取JSON文件
def load_json(filename):
    path = os.path.join(LUBAN_DIR, filename)
    with open(path, 'r', encoding='utf-8') as f:
        return json.load(f)

# 加载所有配置
print("Loading configs...")
skills = load_json("tbbattleskillconfig.json")
moments = load_json("tbbattlemomentconfig.json")
conditions = load_json("tbbattlemomentconditionconfig.json")
effects = load_json("tbbattlemomenteffectconfig.json")

# 建立索引
moment_dict = {m['id']: m for m in moments}
condition_dict = {c['id']: c for c in conditions}
effect_dict = {e['id']: e for e in effects}

# Moment类型字段映射
MOMENT_FIELDS = {
    'CalculateActionWheelMoment': 'CalculateActionWheel',
    'DoDesitionMoment': 'DoDesitionAction',
    'ActionWheelStartMoment': 'ActionWheelStart',
    'BeforeActionMoment': 'BeforeAction',
    'BeforeUnderActionMoment': 'BeforeUnderAction',
    'BeforeClashMoment': 'BeforeClash',
    'AfterClashMoment': 'AfterClash',
    'ReleaseSkillActionMoment': 'ReleaseSkillAction',
    'AfterUnderActionMoment': 'AfterUnderAction',
    'AfterActionMoment': 'AfterAction',
    'RoundEndMoment': 'RoundEnd',
    'SkillEndMoment': 'SkillEnd',
}

# 效果类型统计
effect_types = defaultdict(int)
effect_type_details = defaultdict(list)

# 分析每个技能
skill_analysis = []

print("Analyzing skills...")
for skill in skills:
    skill_id = skill['id']
    skill_name = skill.get('name', '')
    skill_script = skill.get('SkillScript', '')
    
    # 检查是否有触发点
    has_moment = False
    moments_list = []
    
    for field, moment_type in MOMENT_FIELDS.items():
        moment_ids = skill.get(field, [])
        if moment_ids:
            has_moment = True
            for moment_id in moment_ids:
                if moment_id in moment_dict:
                    moment_data = moment_dict[moment_id]
                    
                    # 获取条件和效果
                    condition_ids = moment_data.get('ConditionID', [])
                    success_effects = moment_data.get('SuccessMomentEffect', [])
                    fail_effects = moment_data.get('FailMomentEffect', [])
                    
                    # 解析效果
                    effect_details = []
                    for effect_id in success_effects + fail_effects:
                        if effect_id in effect_dict:
                            effect = effect_dict[effect_id]
                            effect_name = effect.get('EffectName', 'Unknown')
                            effect_types[effect_name] += 1
                            effect_type_details[effect_name].append({
                                'skill_id': skill_id,
                                'moment_id': moment_id,
                                'effect_id': effect_id,
                                'desc': effect.get('desc', '')
                            })
                            effect_details.append({
                                'id': effect_id,
                                'name': effect_name,
                                'params': effect.get('ParamList', []),
                                'desc': effect.get('desc', '')
                            })
                    
                    # 解析条件
                    condition_details = []
                    for cond_id in condition_ids:
                        if cond_id in condition_dict:
                            cond = condition_dict[cond_id]
                            condition_details.append({
                                'id': cond_id,
                                'name': cond.get('ConditionName', ''),
                                'params': cond.get('ParamList', []),
                                'desc': cond.get('desc', '')
                            })
                    
                    moments_list.append({
                        'moment_id': moment_id,
                        'moment_type': moment_type,
                        'conditions': condition_details,
                        'effects': effect_details
                    })
    
    if has_moment:
        skill_analysis.append({
            'id': skill_id,
            'name': skill_name,
            'script': skill_script,
            'has_script': bool(skill_script),
            'moments': moments_list
        })

# 统计
print("\n" + "="*60)
print("技能分析报告")
print("="*60)

total_skills = len(skills)
skills_with_moment = len(skill_analysis)
skills_with_script = sum(1 for s in skill_analysis if s['has_script'])
skills_need_script = sum(1 for s in skill_analysis if not s['has_script'])

print(f"\n总技能数: {total_skills}")
print(f"有触发点的技能数: {skills_with_moment}")
print(f"已有脚本的技能数: {skills_with_script}")
print(f"需要生成脚本的技能数: {skills_need_script}")

print("\n" + "-"*60)
print("效果类型统计 (Top 20)")
print("-"*60)
sorted_effects = sorted(effect_types.items(), key=lambda x: x[1], reverse=True)
for i, (effect_name, count) in enumerate(sorted_effects[:20]):
    print(f"{i+1:2}. {effect_name}: {count}次")

# 生成详细报告
report_lines = []
report_lines.append("="*80)
report_lines.append("技能触发链详细分析报告")
report_lines.append("="*80)
report_lines.append("")
report_lines.append(f"总技能数: {total_skills}")
report_lines.append(f"有触发点的技能数: {skills_with_moment}")
report_lines.append(f"已有脚本的技能数: {skills_with_script}")
report_lines.append(f"需要生成脚本的技能数: {skills_need_script}")
report_lines.append("")
report_lines.append("-"*80)
report_lines.append("效果类型统计")
report_lines.append("-"*80)
for effect_name, count in sorted_effects:
    report_lines.append(f"  {effect_name}: {count}次")

report_lines.append("")
report_lines.append("-"*80)
report_lines.append("需要生成脚本的技能列表")
report_lines.append("-"*80)

for skill in skill_analysis:
    if not skill['has_script']:
        report_lines.append(f"\n技能 {skill['id']}: {skill['name']}")
        for moment in skill['moments']:
            report_lines.append(f"  - {moment['moment_type']} (MomentID: {moment['moment_id']})")
            
            # 条件
            if moment['conditions']:
                report_lines.append(f"    条件:")
                for cond in moment['conditions']:
                    report_lines.append(f"      [{cond['id']}] {cond['name']} - {cond['desc']}")
            
            # 效果
            if moment['effects']:
                report_lines.append(f"    效果:")
                for eff in moment['effects']:
                    params_str = ', '.join(str(p) for p in eff['params']) if eff['params'] else ''
                    report_lines.append(f"      [{eff['id']}] {eff['name']} ({params_str})")

# 保存报告
report_path = os.path.join(LUBAN_DIR, "skill_analysis_report.txt")
with open(report_path, 'w', encoding='utf-8') as f:
    f.write('\n'.join(report_lines))

print(f"\n详细报告已保存到: {report_path}")

# 生成CSV格式的技能列表 (方便程序处理)
csv_lines = ["skill_id,skill_name,has_script,moment_type,moment_id,has_condition,effect_id,effect_name"]
for skill in skill_analysis:
    if not skill['has_script']:
        for moment in skill['moments']:
            has_cond = "Yes" if moment['conditions'] else "No"
            for eff in moment['effects']:
                csv_lines.append(f"{skill['id']},{skill['name']},{skill['has_script']},{moment['moment_type']},{moment['moment_id']},{has_cond},{eff['id']},{eff['name']}")

csv_path = os.path.join(LUBAN_DIR, "skills_need_script.csv")
with open(csv_path, 'w', encoding='utf-8') as f:
    f.write('\n'.join(csv_lines))

print(f"CSV数据已保存到: {csv_path}")

# 统计效果类型的参数结构
print("\n" + "-"*60)
print("效果类型的参数结构分析")
print("-"*60)

effect_param_structure = {}
for effect in effects:
    name = effect.get('EffectName', '')
    params = effect.get('ParamList', [])
    params_tuple = tuple(params) if params else ()
    if name not in effect_param_structure:
        effect_param_structure[name] = set()
    effect_param_structure[name].add(len(params))

for name, param_counts in sorted(effect_param_structure.items()):
    print(f"  {name}: 参数数量 = {sorted(param_counts)}")

print("\n分析完成!")