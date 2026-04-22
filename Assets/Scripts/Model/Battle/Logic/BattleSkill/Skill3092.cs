using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3092 : BattleSkillBase
{
    //在本回合将时段变为昼
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeChrono(ChronoType.Morning, BattleChronoContinueType.Round, 1);
    }

    //在本回合将天气变为晴
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeWeather(BattleWeatherType.Sunny, BattleWeatherContinueType.Round, 1);
    }

    //本次战斗刚炁的自然恢复不会低于25
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.Buff73092, Subject, 1, null, BattleMomentType.AfterAction);
    }
}