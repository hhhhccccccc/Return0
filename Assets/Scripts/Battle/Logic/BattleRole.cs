public class BattleRole : BattleUnit
{
    public override void Init(BattleField bf, Character character)
    {
        ObjType = BattleObjType.Role;
        base.Init(bf, character);
    }
}
