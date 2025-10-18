using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentCondition_CheckWeather : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    protected override bool OnCondition()
    {
        var check = Config.ParamList[0].ToInt();
        var state = Config.ParamList[1].ToInt() == 1;
        switch (check)
        {
            case 1:
                if (state && BattleLogicStateManager.BattleWeatherType == WeatherType.Sunny)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleWeatherType != WeatherType.Sunny)
                {
                    return true;
                }
                break;
            case 2:
                if (state && BattleLogicStateManager.BattleWeatherType == WeatherType.Shade)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleWeatherType != WeatherType.Shade)
                {
                    return true;
                }
                break;
            case 3:
                if (state && BattleLogicStateManager.BattleWeatherType == WeatherType.Rain)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleWeatherType != WeatherType.Rain)
                {
                    return true;
                }
                break;
            case 4:
                if (state && BattleLogicStateManager.BattleWeatherType == WeatherType.Fog)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleWeatherType != WeatherType.Fog)
                {
                    return true;
                }
                break;
        }
        
        
        return false;
    }
}