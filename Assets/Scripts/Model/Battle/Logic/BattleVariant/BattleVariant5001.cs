using cfg;

public class BattleVariant5001 : BattleVariantBase
{
    //本次的行动延迟1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -1);
    }
}
