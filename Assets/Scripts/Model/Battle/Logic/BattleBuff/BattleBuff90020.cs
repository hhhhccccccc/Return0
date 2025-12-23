using cfg;

public class BattleBuff90020 : BattleBuffBase
{
    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (buffID == GameConst.Battle.Buff20221 || buffID == GameConst.Battle.Buff20231)
        {
            if (layerCount <= Config.ParamEx[0].ToInt())
            {
                Subject.AddActionTimes(Config.ParamEx[1].ToInt());
            }
        }
    }
}

