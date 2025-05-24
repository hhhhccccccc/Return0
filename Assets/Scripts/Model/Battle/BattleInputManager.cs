using System.Collections.Generic;
using Zenject;

public class BattleInputManager : SingleModel
{
    [Inject] private ILogManager LogManager;
    
    private List<InputEventModel> InputList = new();
    
    public void BattleStart()
    {
        Register<InputEventModel>(OnGetInput);
        Register<BattleRoundStartEventModel>(OnBattleRoundStart);
    }

    private void OnBattleRoundStart(BattleRoundStartEventModel model)
    {
        InputList.Clear();   
    }

    private void OnGetInput(InputEventModel model)
    {
        LogManager.Debug(model.InputType.ToString());
        LogManager.Debug(model.KeyCode.ToString());
    }

    public override void Clear()
    {
        base.Clear();
        InputList.Clear();
    }
}
