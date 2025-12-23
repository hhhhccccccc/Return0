using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff10021 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    /// <summary>
    /// 被攻击时 减少伤害
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public override void ChangeDamageValue(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (Subject.HasBuffMechanism(BuffMechanism.NotEffectGainBuff))
        {
            return;
        }
        
        if (paramModel is DamageParamModel model)
        {
            if (model.SelfID == Subject.EntityID)
            {
                return;
            }

            var attacker = BattleManager.GetUnit(model.SelfID);
            
            var targetSkill = attacker.GetSkill();
            if (targetSkill == null)
                return;

            var final = 0.0f;
        
            if (targetSkill.GetSKillType == SkillType.PowerKilling)
            {
                var pct = Config.ParamEx[LayerCount - 1];
                var targetPower = attacker.GetProperty(BattlePropertyType.Power);
                final = targetPower * pct;
            }
        
            if (targetSkill.GetSKillType == SkillType.ArtKilling)
            {
                var selfSkill = Subject.GetSkill();
                if (selfSkill != null && selfSkill.Target.EntityID == attacker.EntityID)
                {
                    var pct = Config.ParamEx[LayerCount - 1];
                    var targetPower = attacker.GetProperty(BattlePropertyType.Tech);
                    final = targetPower * pct;
                }
            }

            if (final > 0)
            {
                var mechanism = Config.Mechanism[0];
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
