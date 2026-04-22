using System.Globalization;
using Zenject;

public partial class UIBattleRoundStartPanel
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; } 
    private UISpineItem SpineItem { get; set; }
    private Timer Timer { get; set; }
    protected override void OnPanelCreate()
    {
        if (SpineItem == null)
        {
            SpineItem = CreateItemByType<UISpineItem>(TfSpine);
        }
        
        SpineItem.SetModel("Assets/GameResource/Prefab/SpinePrefab/BattleRoundStart/BattleRoundStart.prefab");
    }

    public void Play()
    {
        var info = SpineItem.PlayAnimation("animation");
        LogManager.D(info.duration.ToString(CultureInfo.InvariantCulture));
        var chronoConfig = ConfigManager.GetChronoConfig(BattleLogicStateManager.BattleChronoType);
        TxtChrono.SetText(chronoConfig.Des);
        var weatherConfig = ConfigManager.GetBattleWeatherConfig(BattleLogicStateManager.BattleWeatherType);
        TxtWeather.SetText(weatherConfig.Des);
        
        TxtRound.SetText($"回合{Util.ToChineseNumber(BattleLogicStateManager.Round)}");
        Timer = TimeManager.Delay(info.duration, ()=>
        {
            var ui = UIManager.GetUI<UIBattlePanel>();
            ui.SetTopActive(true);
            Close();
        });
    }

    protected override void OnPanelDestroy()
    {
        if (Timer != null)
        {
            TimeManager.RemoveTimer(Timer);
            Timer = null;
        }
    }

    public override void Esc()
    {
        
    }
}
