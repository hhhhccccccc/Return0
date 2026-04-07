using cfg;

public class BattleVariant1002 : BattleVariantBase
{
    //本次的行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
    }
}
