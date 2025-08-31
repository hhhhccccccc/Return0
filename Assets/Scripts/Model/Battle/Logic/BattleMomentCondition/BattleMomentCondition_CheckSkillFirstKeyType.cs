using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckSkillFirstKeyType : BattleMomentCondition
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
                return (int)BattleUtil.GetSkillFirstKey(SkillID) == checkKey;
            }
            else
            {
                return (int)BattleUtil.GetSkillFirstKey(SkillID) != checkKey;
            }
        }

        var target = GetUnitByParamID(Config.ParamList[0]);
        var firstKey = target.GetSkillFirstKey();
        if (relation == 1)
        {
            return (int)firstKey == checkKey;
        }
        else
        {
            return (int)firstKey != checkKey;
        }
    }
}