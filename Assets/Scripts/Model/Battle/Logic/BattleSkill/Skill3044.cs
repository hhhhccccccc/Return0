using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill3044 : BattleSkillBase
{
    private int WinTargetID { get; set; }
    public override void AfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfClashState(Subject.EntityID))
            {
                WinTargetID = model.GetSelfID(Subject.EntityID);
            }
            else if (model.GetOtherClashState(Subject.EntityID))
            {
                WinTargetID = model.GetOtherID(Subject.EntityID);
            }
        }
    }
    
    //至少造成80%技的伤害时返还消耗的键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var damage = model.GetSelfAttackHpValue(Subject.EntityID);
            if (damage >= Subject.GetProperty(BattlePropertyType.Tech) * 0.8f)
            {
                DoAddKey(Subject, TruthCostKey, ChangeKeyReason.SkillEffect, ChangeKeyType.Back);
            }
        }
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        if (WinTargetID > 0)
        {
            var unit = BattleManager.GetUnit(WinTargetID);
            var skill = unit.GetSkill();
            DoAddKey(unit, skill.TruthCostKey, ChangeKeyReason.SkillEffect, ChangeKeyType.Back);
            WinTargetID = 0;
        }
    }

    protected override void OnSkillRecycle()
    {
        WinTargetID = 0;
    }
}