using System;
using cfg;
using Zenject;

//下一次术杀式的基础威力不会低于140%技（巧来方计状态）
public class BattleBuff30351 : BattleBuffBase
{
    protected override void OnTrySetWellyRateBase(int skillGuid, ref float value)
    {
        var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
        if (BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.ArtKilling)
        {
            value = Math.Max(value, GetConfigParamFloat(0));
        }
    }
}
