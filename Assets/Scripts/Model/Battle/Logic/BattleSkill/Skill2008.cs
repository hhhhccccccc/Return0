using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill2008 : BattleSkillBase
{
    //获得2层缓速
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 2, null, BattleMomentType.DoDesitionAction);
    }
    
    private const int BuffID = 72008;
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            var targetSkill = target.GetSkill();
            if (targetSkill != null && targetSkill.Target == Subject && Target == target)
            {
                var propertyValue = target.GetProperty(BattlePropertyType.Power) * 0.25f;
                DoAddBuff(Subject, BuffID, Subject, 1, new List<float> { propertyValue }, BattleMomentType.BeforeClash);
                DoAddBuff(target, BuffID, Subject, 1, new List<float> { -propertyValue }, BattleMomentType.BeforeClash);
            }
        }
    }

    //刚炁+5
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 5, BattleSource.Skill);
    }
} 