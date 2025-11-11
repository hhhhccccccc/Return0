using cfg;

public class BattleBuff30361 : BattleBuffBase
{
    protected override int OnGetKeyPropertyMax()
    {
        return LayerCount * Config.ParamEx[0].ToInt();
    }

    protected override void OnLayerCountChanged()
    {
        Subject.CheckKeyLimit();
    }
}
