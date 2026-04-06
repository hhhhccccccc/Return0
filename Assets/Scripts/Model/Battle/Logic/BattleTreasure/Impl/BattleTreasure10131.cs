using cfg;
public class BattleTreasure10131 : BattleTreasureBase
{
    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling && BattleUtil.CheckSkillNeedTarget(model.GetSelfSkillID(Subject.EntityID)))
            {
                DoAddBuff(Subject, GameConst.Battle.BuffShangKou, Subject, GetConfigParamInt(0), null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }
}


