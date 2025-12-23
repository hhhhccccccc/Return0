using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveRandomBuffByBuffLayerCount : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var buffID = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                var buff = target.GetBuff(buffID);
                var layerCount = buff?.LayerCount ?? 0;
                target.RemoveRandomKey(layerCount, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
            }
        }
    }
}