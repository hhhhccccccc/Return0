using TMPro;
using Zenject;

public class UIBattle : Panel
{
    [AutoFind] 
    private TextMeshProUGUI Txt { get; set; }
    [Inject]
    private ILogManager LogManager { get; set; }
    [Inject]
    private BattleManager BattleManager { get; set; }
    
    protected override void OnAwake()
    {
        base.OnAwake();
        
        
        
        LogManager.Debug("[UI加载完毕]");
    }
    
    protected override void OnStart()
    {
        base.OnStart();
    }
}
