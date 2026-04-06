using cfg;

public class BattleHeartMethod10098 : BattleHeartMethodBase
{
    public override void RoundEnd()
    {
        var buffs = Subject.GetRandomBuffByType(BuffType.Gain);
        foreach (var buff in buffs)
        {
            DoClearBuff(Subject, buff.BuffID);
        }
        
        buffs = Subject.GetRandomBuffByType(BuffType.Abnormal);
        foreach (var buff in buffs)
        {
            DoClearBuff(Subject, buff.BuffID);
        }
    }
}