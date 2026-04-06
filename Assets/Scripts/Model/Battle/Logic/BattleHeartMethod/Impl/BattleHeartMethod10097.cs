using cfg;

public class BattleHeartMethod10097 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void RoundStart()
    {
        CanTrigger = true;
    }

    public override void EndAction()
    {
        if (CanTrigger && Subject.ActionTimes == 0)
        {
            DoAddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, GetConfigParamInt(0), null, BattleMomentType.AfterAction);
            CanTrigger = false;
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}