public class BattleRole : BattleUnit
{
    public override void Init(BattleField bf, Character character, int slotIndex)
    {
        ObjType = BattleObjType.Role;
        base.Init(bf, character, slotIndex);
    }
}
