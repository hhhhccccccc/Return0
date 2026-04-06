using cfg;

public class BattleTreasure10098 : BattleTreasureBase
{
    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var directHp = model.GetDirectDamageValue(Subject.EntityID);
            if (directHp > 0)
            {
                DoAddBuff(Subject, GameConst.Battle.BuffTaoTieWanWu, Subject, (directHp * GetConfigParamFloat(0)).ToInt(), null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }
}


