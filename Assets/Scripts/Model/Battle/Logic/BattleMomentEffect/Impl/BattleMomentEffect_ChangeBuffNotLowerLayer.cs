using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeBuffNotLowerLayer : BattleMomentEffect
{
    protected override void OnEffect()
    {
        if (Subject != null)
        {
            var buffID = Config.ParamList[0].ToInt();
            var buff = Subject.GetBuff(buffID);
            if (buff != null)
            {
                var layerCount = Config.ParamList[1].ToInt();
                buff.SetBuffNotLowerLayerCount(layerCount);
            }
        }
    }
}