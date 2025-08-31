using Zenject;

public class DebugController : ControllerBase<DebugEventModel>
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private ILogManager LogManager;
 
    public override void Handle(DebugEventModel model)
    {
        
    }
}
