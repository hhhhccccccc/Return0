public class BattleRole : BattleUnit
{
    public override void Init(BattleField bf)
    {
        ObjType = BattleObjType.Role;
        base.Init(bf);
    }
}
