using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveBuff : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var removeTarget = GetUnitByParamID(Config.ParamList[0]);
        if (removeTarget != null)
        {
            var buffID = Config.ParamList[1].ToInt();
            var removeCount = Config.ParamList[2].ToInt();
            removeTarget.ReduceBuff(buffID, removeCount);
            Debug($"[扳机效果] 移除buff 目标 : {removeTarget.EntityID}, buffID : {buffID}, 层数 : {removeCount}");
        }
    }
}