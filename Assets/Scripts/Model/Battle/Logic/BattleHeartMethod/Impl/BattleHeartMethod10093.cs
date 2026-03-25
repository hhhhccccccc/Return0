using cfg;

//异体
//todo 表现
public class BattleHeartMethod10093 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }
    private int ChangeActionWheel { get; set; }
    public override void RoundStart()
    {
        base.RoundStart();
        CanTrigger = true;
        ChangeActionWheel = Util.GetRandomInt(GetParamInt(0), GetParamInt(1) + 1);
    }

    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        if (Subject.HasBuff(buffID))
        {
            return false;
        }

        return true;
    }

    public override int GetChangeActionWheel()
    {
        if (CanTrigger)
        {
            return ChangeActionWheel;
        }

        return 0;
    }

    public override void EndAction()
    {
        CanTrigger = false;
        base.EndAction();
    }
}