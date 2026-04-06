using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30081 : BattleBuffBase
{
    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        var skillID = Subject.GetSkillID();
        if (skillID == GameConst.Battle.SkillFuXiaoJian)
        {   
            var other = GetOtherUnit(paramModel);
            DoAddBuff(other, GetConfigParamInt(0), Subject, GetConfigParamInt(1), null, BattleMomentType.AfterClash);
        }        
    }
}
