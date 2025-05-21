using UnityEngine;
using Zenject;

public class GameStartController : ControllerBase<GameStartEventModel>
{
    [Inject] private ILogManager LogManager;
    [Inject] private UIManager UIManager;
    public override void Handle(GameStartEventModel model)
    {
        LogManager.Debug("游戏开始");
        UIManager.ShowUI("UIBattle");
        var uibattle = UIManager.GetUI<UIBattle>("UIBattle");
        if (uibattle)
        {
            LogManager.Debug("<UNK>");
        }
    }
}
