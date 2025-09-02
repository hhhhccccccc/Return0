using System.Globalization;
using Zenject;

public class DebugController : ControllerBase<DebugEventModel>
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private ILogManager LogManager;
    [Inject] private PlayerArchiveData PlayerArchiveData;
    public override void Handle(DebugEventModel model)
    {
        PlayerArchiveData.Data1++;
        LogManager.Debug(PlayerArchiveData.Data1.ToString());
        LogManager.Debug(ConfigManager.GetSceneConfig(1).InteractionItem[0].InteractionItemID.ToString());
        LogManager.Debug(ConfigManager.GetSceneConfig(1).InteractionItem[0].X.ToString(CultureInfo.InvariantCulture));
        LogManager.Debug(ConfigManager.GetSceneConfig(1).InteractionItem[0].Y.ToString(CultureInfo.InvariantCulture));
        
        LogManager.Debug(ConfigManager.GetSceneConfig(1).MiniMapPos.X.ToString(CultureInfo.InvariantCulture));
        LogManager.Debug(ConfigManager.GetSceneConfig(1).MiniMapPos.Y.ToString(CultureInfo.InvariantCulture));
    }
}
