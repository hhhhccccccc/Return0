using System;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect3065002 : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var buffID = Config.ParamList[1].ToInt();
            var delta = Config.ParamList[2].ToInt();
            foreach (var target in targetList)
            {
                var buff = target.GetBuff(buffID);
                if (buff != null)
                {
                    buff.TriggerBuffMomentByCountIgnoreLayerCount(buff.LayerCount + delta, ParamModel);
                }
            }
        }
    }
}