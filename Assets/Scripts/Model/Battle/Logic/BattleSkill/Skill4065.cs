using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4065 : BattleSkillBase
{
    //清除全部角色的异常效果，将天气覆盖为雨天8个回合
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var unitList = BattleManager.GetAllAliveUnit();
        foreach (var unit in unitList)
        {
            var buffList = unit.GetRandomBuffByType(BuffType.Abnormal);
            foreach (var buff in buffList)
            {
                unit.ClearBuff(buff.BuffID);
            }
        }
        
        DoChangeWeather(BattleWeatherType.Rain, BattleWeatherContinueType.Round, 8);
    }
}