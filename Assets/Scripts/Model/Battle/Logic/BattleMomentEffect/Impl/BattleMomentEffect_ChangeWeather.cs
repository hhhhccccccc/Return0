using System;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeWeather : BattleMomentEffect
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    protected override void OnEffect()
    {
        var weatherType = (WeatherType)(Config.ParamList[0].ToInt());
        var continueType = (BattleWeatherContinueType)(Config.ParamList[1].ToInt());    
        var times = Config.ParamList[2].ToInt();    
        BattleLogicStateManager.ChangeWeather(weatherType, continueType, times);
    }
}