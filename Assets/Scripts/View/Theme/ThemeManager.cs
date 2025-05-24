
using UnityEngine;
using Zenject;

public class ThemeManager : View
{
    [Inject]
    private ILogManager LogManager;
    [AutoFind]
    private PlayerNodeComponent BattleNode { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        
        
        
        LogManager.Debug("[场景加载完毕]");
    }

    protected override void OnStart()
    {
        base.OnStart();
    }
}
