using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3051 : BattleSkillBase
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (paramModel is { } model)
        {
            var propertyValue = Subject.GetProperty(BattlePropertyType.Power);
            var checkValue = Config.ParamEx[0] * propertyValue;
            if (BattleUtil.CompareValue(model.AttackHpValue, checkValue, 1))
            {
                return new BattleSkillRepeatData
                {
                    SkillID = SkillID,
                    TargetID = Target.EntityID,
                    MaxRepeatCount = 2,
                    IfLostChangeToOther = false
                };
            }
        }
        
        return null;
    }
}