using System;

//todo 行动意图揭示
public class BattleHeartMethod10091 : BattleHeartMethodBase
{
    public override void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        var isSame = Math.Abs(subjectDamageRate - targetDamageRate) <= 0.001f;
        if (isSame)
        {
            state = true;
        }
    }
}