using cfg;
using UnityEngine;

public class BattleMomentCondition_CheckDamageType : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        if (ParamModel is DamageParamModel model)
        {
            var targetIndex = Config.ParamList[0].ToInt();
            var relation = Config.ParamList[1].ToInt();
            //1是攻击方
            if (targetIndex == 1)
            {
                if (relation == 1)
                {
                    return (int)model.GetSelfDamageType(Subject.EntityID) == Config.ParamList[2].ToInt();
                }
                else
                {
                    return (int)model.GetSelfDamageType(Subject.EntityID) != Config.ParamList[2].ToInt();
                }
            }

            if (targetIndex == 2)
            {
                if (relation == 1)
                {
                    return (int)model.GetOtherDamageType(Subject.EntityID) == Config.ParamList[2].ToInt();
                }
                else
                {
                    return (int)model.GetOtherDamageType(Subject.EntityID) != Config.ParamList[2].ToInt();
                }
            }
        }
        
        return false;
    }
}