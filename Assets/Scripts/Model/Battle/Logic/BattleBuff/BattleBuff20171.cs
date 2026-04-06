using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20171 : BattleBuffBase
{
    /// <summary>
    /// 攻击时 减少伤害 受伤时增加伤害
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public override void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (Subject.HasBuffMechanism(BuffMechanism.NotEffectAbnormalBuff))
        {
            return;
        }
        
        if (paramModel is DamageParamModel model)
        {
            var selfSkillType = model.GetSelfSkillType(Subject.EntityID);

            var final = 0.0f;
        
            if (selfSkillType == SkillType.PowerKilling)
            {
                var pct = GetConfigParamFloat(LayerCount - 1);
                var targetPower = Subject.GetProperty(BattlePropertyType.Power);
                final = targetPower * pct;
            }
        
            if (selfSkillType == SkillType.ArtKilling)
            {
                var selfSkill = Subject.GetSkill();
                if (selfSkill != null && model.BattleClashType == BattleClashType.SingleAction)
                {
                    var pct = GetConfigParamFloat(LayerCount - 1);
                    var targetPower = Subject.GetProperty(BattlePropertyType.Tech);
                    final = targetPower * pct;
                }
            }

            if (final > 0)
            {
                var mechanism = GetMechanism(0);
                if (dict.TryGetValue(mechanism, out var value))
                {
                    if (final >= Math.Abs(value))
                    {
                        dict[mechanism] = -final;
                    }
                }
                else
                {
                    dict.Add(mechanism, -final);
                }
            }
        }
    }
}
