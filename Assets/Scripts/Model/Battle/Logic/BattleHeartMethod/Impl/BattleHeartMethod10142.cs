using cfg;
//todo 表现
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
                        var dataList = ConfigHelper.RandomCommonPool(GetParamInt(0));
                        if (dataList.Count > 0)
                        {
                            var data = dataList[0];
                            BattleBuffManager.AddBuff(Subject, data.ID, Subject, data.Num);
                        }
                        break;
                    case 1:
                        Subject.AddRandomKey(GetParamInt(1), ChangeKeyReason.HeartMethodEffect);
                        break;
                    case 2:
                        Subject.ChangeProperty(BattlePropertyType.GangQi, GetParamFloat(2), BattleSource.HeartMethod);
                        break;
                    case 3:
                        Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamFloat(3), BattleSource.HeartMethod);
                        break;
                }
            }
        }
    }
}