using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_SetBuffIgnoreReduceLayer : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var target = GetUnitByParamID(1);
        if (target != null)
        {
            var buffID = Config.ParamList[0].ToInt();
            var buff = target.GetBuff(buffID);
            if (buff != null)
            {
                var isIgnore = Config.ParamList[1].ToInt() == 0;
                buff.SetIgnoreReduceLayer(isIgnore);
            }
        }
    }
}