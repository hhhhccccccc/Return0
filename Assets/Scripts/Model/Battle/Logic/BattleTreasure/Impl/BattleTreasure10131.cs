using cfg;

//todo 表现
public class BattleTreasure10131 : BattleTreasureBase
{
    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling && BattleUtil.CheckSkillNeedTarget(model.GetSelfSkillID(Subject.EntityID)))
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffShangKou, Subject, GetParamInt(0));
            }
        }
    }
}


