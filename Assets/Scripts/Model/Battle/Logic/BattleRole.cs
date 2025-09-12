public class BattleRole : BattleUnit
{
    public override void Init(BattleField bf, HeroData heroData)
    {
        ObjType = BattleObjType.Role;
        base.Init(bf, heroData);
    }
}
