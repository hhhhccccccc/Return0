using cfg;

public class BattleVariant5003 : BattleVariantBase
{
    //本次的行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
    }
    
    //行动的前2息内即将受到攻击将立即执行本次行动
    public override void BeforeUnderAction()
    {
        if (CheckBeActionInBeforeActionWheel(Subject, 2, false))
        {
            DoSetActionWheelToNow(Subject);
        }
    }
}
