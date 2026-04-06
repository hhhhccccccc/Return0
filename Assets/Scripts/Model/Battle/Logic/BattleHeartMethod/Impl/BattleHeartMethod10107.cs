using cfg;

public class BattleHeartMethod10107 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfClashState(Subject.EntityID))
            {
                DoAddRandomKey(Subject, GetConfigParamInt(0), ChangeKeyReason.HeartMethodEffect);
            }
        }
    }
}