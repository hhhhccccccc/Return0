using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class InputManager : MonoSingleton<InputManager>
{
    private DiContainer DiContainer;
    private IMessageManager MessageManager;
    private IPoolManager PoolManager;
    private ILogManager LogManager;
    private bool BattleInputValid;
    public override void SingletonInit(DiContainer diContainer)
    {
        DiContainer =  diContainer;
        MessageManager = diContainer.Resolve<IMessageManager>();
        PoolManager = diContainer.Resolve<IPoolManager>();
        LogManager = diContainer.Resolve<ILogManager>();
        BattleInputValid = false;
    }

    private void Update()
    {
        if (BattleInputValid)
            BattleInputListen();
    }

    public void SetBattleInputValid(bool value) => BattleInputValid = value;

    private void BattleInputListen()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var model = PoolManager.GetClass<InputEventModel>();
            model.InputType = InputType.Keyboard;
            model.KeyCode = KeyCode.Q;
            MessageManager.Dispatch(model);
            PoolManager.RecycleClass(model);
        }
    }
}
