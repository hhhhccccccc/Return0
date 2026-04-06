using cfg;

public class BattleHeartMethod10142 : BattleHeartMethodBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var other = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
            if (other.GetIsBeActionReveals())
            {
                var random = Util.GetRandomInt(0, 4);
                switch (random)
                {
                    case 0:
                        var dataList = ConfigHelper.RandomCommonPool(GetConfigParamInt(0));
                        if (dataList.Count > 0)
                        {
                            var data = dataList[0];
                            DoAddBuff(Subject, data.ID, Subject, data.Num, null, BattleMomentType.ReleaseSkillAction);
                        }
                        break;
                    case 1:
                        DoAddRandomKey(Subject, GetConfigParamInt(1), ChangeKeyReason.HeartMethodEffect);
                        break;
                    case 2:
                        DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(2), BattleSource.HeartMethod);
                        break;
                    case 3:
                        DoChangeProperty(Subject, BattlePropertyType.XuanQi, GetConfigParamFloat(3), BattleSource.HeartMethod);
                        break;
                }
            }
        }
    }
}