using cfg;

public class BattleBuffDontBeCounter : BattleBuffBase
{
    private int DontBeCounterType;
    protected override void OnStart()
    {
        base.OnStart();
        DontBeCounterType = Config.ParamEx[0].ToInt();
        switch (DontBeCounterType)
        {
            case 0:
                Subject.SetDontBeCounter(1);
                break;
            case 1:
                Subject.AddIgnoreTargetNotHasUpBuff(1);
                break;
            case 2:
                Subject.AddIgnoreTargetNotHasDownBuff(1);
                break;
            case 3:
                Subject.AddIgnoreTargetNotHasLeftBuff(1);
                break;
            case 4:
                Subject.AddIgnoreTargetNotHasRightBuff(1);
                break;
        }
    }

    protected override void OnBuffRemove()
    {
        switch (DontBeCounterType)
        {
            case 0:
                Subject.SetDontBeCounter(-1);
                break;
            case 1:
                Subject.AddIgnoreTargetNotHasUpBuff(-1);
                break;
            case 2:
                Subject.AddIgnoreTargetNotHasDownBuff(-1);
                break;
            case 3:
                Subject.AddIgnoreTargetNotHasLeftBuff(-1);
                break;
            case 4:
                Subject.AddIgnoreTargetNotHasRightBuff(-1);
                break;
        }
    }
}
