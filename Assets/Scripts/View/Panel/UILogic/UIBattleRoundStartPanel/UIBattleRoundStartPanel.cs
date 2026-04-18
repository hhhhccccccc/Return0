using System.Globalization;
using Zenject;

public partial class UIBattleRoundStartPanel
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; } 
    private UISpineItem SpineItem { get; set; }
    protected override void OnCreate()
    {
        if (SpineItem == null)
        {
            SpineItem = CreateUIComponentByType<UISpineItem>(TfSpine);
        }
        
        SpineItem.SetModel("Assets/GameResource/Prefab/SpinePrefab/BattleRoundStart/BattleRoundStart.prefab");
    }

    public override void OnShow()
    {
        var info = SpineItem.PlayAnimation("animation");
        LogManager.D(info.duration.ToString(CultureInfo.InvariantCulture));
        var chronoConfig = ConfigManager.GetChronoConfig(BattleLogicStateManager.BattleChronoType);
        TxtChrono.SetText(chronoConfig.Des);
        var weatherConfig = ConfigManager.GetBattleWeatherConfig(BattleLogicStateManager.BattleWeatherType);
        TxtWeather.SetText(weatherConfig.Des);
        TxtRound.SetText($"回合{Util.ToChineseNumber(BattleLogicStateManager.Round)}");
        TimeManager.Delay(info.duration, Close);
    }
}
