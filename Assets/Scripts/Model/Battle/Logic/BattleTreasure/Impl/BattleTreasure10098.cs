
//todo 表现
public class BattleTreasure10098 : BattleTreasureBase
{
    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var directHp = model.GetDirectDamageValue(Subject.EntityID);
            if (directHp > 0)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30051, Subject, (directHp * GetParamFloat(0)).ToInt());
            }
        }
    }
}


