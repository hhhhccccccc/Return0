using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIBattle : BattleUIWindow
{
    [Inject] private DiContainer diContainer;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    private void OnBattleFieldLogicReady(BattleFieldLogicReadyEventModel model)
    {
        //var logicBf = diContainer.Resolve<BattleManager>().GetBf(model.IsSelf);
        //var roles = model.Roles;
    }
}
