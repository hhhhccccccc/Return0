using TMPro;
using Zenject;

public class UIBattle : Panel
{
    [AutoFind] 
    private TextMeshProUGUI Txt { get; set; }
    
    [Inject]
    private ILogManager LogManager { get; set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        if (Txt)
        {
            LogManager.Debug(":ddd");
        }
    }
}
