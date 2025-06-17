using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class InputManager : ManagerBase, IInitRootAfter, IUpdate
{
    [Inject] private IMessageManager MessageManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private ILogManager LogManager;
    private bool BattleInputValid;

    protected override IEnumerator OnInit()
    {
        BattleInputValid = false;
        yield break;
    }
    public void SetBattleInputValid(bool value) => BattleInputValid = value;

    private void BattleInputListen()
    {
        if (Input.GetMouseButtonDown(0)) //检测鼠标左键点击
        { 
            MessageManager.Dispatch<MouseClickEventModel>(null);
        }
    }

    public void OnUpdate(float dt)
    {
        if (BattleInputValid)
            BattleInputListen();
    }
}