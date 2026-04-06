using cfg;

public class BattleBuff90004 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Remove);
        DoClearBuff(Subject, BuffID);
    }
}
