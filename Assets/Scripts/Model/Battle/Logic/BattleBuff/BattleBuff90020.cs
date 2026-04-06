using System;
using cfg;

public class BattleBuff90020 : BattleBuffBase
{
    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        if (layerCount >= 0)
        {
            return;
        }
        
        if (buffID == GameConst.Battle.BuffYaoDu || buffID == GameConst.Battle.BuffYaoDuQinShi)
        {
            if (Math.Abs(layerCount) > GetConfigParamInt(0))
            {
                DoAddActionTimes(Subject, GetConfigParamInt(1));
            }
        }
    }
}

