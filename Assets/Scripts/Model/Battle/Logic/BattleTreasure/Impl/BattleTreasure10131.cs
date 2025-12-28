using cfg;

public class BattleTreasure10131 : BattleTreasureBase
{
    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling && BattleUtil.CheckSkillNeedTarget(model.GetSelfSkillID(Subject.EntityID)))
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff20081, Subject, GetParamInt(0));
            }
        }
    }
}


