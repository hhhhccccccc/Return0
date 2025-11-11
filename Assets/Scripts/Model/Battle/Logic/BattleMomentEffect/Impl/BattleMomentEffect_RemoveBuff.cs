using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_RemoveBuff : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var removeTarget = GetUnitByParamID(Config.ParamList[0]);
        if (removeTarget.Count > 0)
        {
            var buffID = Config.ParamList[1].ToInt();
            var removeCount = Config.ParamList[2].ToInt();
            foreach (var target in removeTarget)
            {
                if (removeCount == 0)
                {
                    target.ClearBuff(buffID);
                    Debug($"[扳机效果] 移除buff 目标 : {target.EntityID}, buffID : {buffID}, 所有层数");
                }
                else
                {
                    target.ReduceBuffLayerCount(buffID, removeCount);
                    Debug($"[扳机效果] 移除buff 目标 : {target.EntityID}, buffID : {buffID}, 层数 : {removeCount}");
                }
            }
        }
    }
}