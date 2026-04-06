
using cfg;

public class BattleHeartMethod10128 : BattleHeartMethodBase
{
    private bool NeedSuccess { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        NeedSuccess = true;
    }

    public override void AfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var state = model.GetSelfClashState(Subject.EntityID);
            if ((state && NeedSuccess) || (!state && !NeedSuccess))
            {
                var commonPool = ConfigHelper.RandomCommonPool(GetConfigParamInt(0));
                DoAddBuff(Subject, commonPool[0].ID, Subject, commonPool[0].Num, null, BattleMomentType.AfterClash);
            }
        }
    }

    public override void RoundStart()
    {
        NeedSuccess = !NeedSuccess;
    }

    protected override void OnHeartMethodRecycle()
    {
        NeedSuccess = false;
    }
}