using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10021 : BattleBuffBase
{
    /// <summary>
    /// 至少减少受到的武杀式{[int]}%敌手的力的直接伤害，至少减少行动目标对自身的术杀式伤害{[int]}%敌手的技的直接伤害
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    protected override void OnReduceDamageInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (Subject.HasBuffMechanism(BuffMechanism.NotEffectGainBuff))
        {
            return;
        }
        
        if (paramModel is DamageParamModel model)
        {
            var attacker = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
            var attackerSkillType = model.GetOtherSkillType(Subject.EntityID);

            var final = 0.0f;
        
            if (attackerSkillType == SkillType.PowerKilling)
            {
                var pct = GetConfigParamFloat(LayerCount - 1);
                var targetPower = attacker.GetProperty(BattlePropertyType.Power);
                final = targetPower * pct;
            }
        
            if (attackerSkillType == SkillType.ArtKilling)
            {
                var selfSkill = Subject.GetSkill();
                if (selfSkill != null && selfSkill.Target.EntityID == attacker.EntityID)
                {
                    var pct = GetConfigParamFloat(LayerCount - 1);
                    var targetPower = attacker.GetProperty(BattlePropertyType.Tech);
                    final = targetPower * pct;
                }
            }

            if (final > 0)
            {
                var mechanism = GetMechanism(0);
                if (dict.TryGetValue(mechanism, out var value))
                {
                    if (final >= value)
                    {
                        dict[mechanism] = final;
                    }
                }
                else
                {
                    dict.Add(mechanism, final);
                }
            }
        }
    }
}
