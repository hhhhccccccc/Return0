

//todo 表现
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
        base.RoundStart();
        CanTrigger = true;
    }

    public override void EndAction()
    {
        base.EndAction();
        if (CanTrigger && Subject.ActionTimes == 0)
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, GetConfigParamInt(0));
            CanTrigger = false;
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}