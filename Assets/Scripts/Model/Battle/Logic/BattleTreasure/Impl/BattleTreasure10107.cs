public class BattleTreasure10107 : BattleTreasureBase
{
    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var directHp = model.GetDirectDamageValue(Subject.EntityID);
            if (directHp > 0)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, (directHp * GetParamFloat(0)).ToInt());
            }
        }
    }
}


