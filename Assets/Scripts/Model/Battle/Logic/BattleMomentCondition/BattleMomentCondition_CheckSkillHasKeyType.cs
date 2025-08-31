using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckSkillHasKeyType : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil;
    
    protected override bool OnCondition()
    {
        var relation = Config.ParamList[1].ToInt();
        var checkKey = Config.ParamList[2].ToInt();
        if (SkillID != 0)
        {
            if (relation == 1)
            {
                return BattleUtil.GetSkillNeedKey(SkillID).Any(key => key == checkKey);
            }
            else
            {
                return BattleUtil.GetSkillNeedKey(SkillID).All(key => key != checkKey);
            }
        }

        var target = GetUnitByParamID(Config.ParamList[0]);
        var firstKey = target.GetSkillBase.GetKeyCostList;
        if (relation == 1)
        {
            return firstKey.Any(key => key == checkKey);
        }
        else
        {
            return firstKey.All(key => key != checkKey);
        }
    }
}