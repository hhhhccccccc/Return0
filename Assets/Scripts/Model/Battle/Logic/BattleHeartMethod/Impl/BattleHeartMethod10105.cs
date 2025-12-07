using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10105 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        var takeList = Subject.TakeSkillDataManager.GetTakeSkillData();
        var allCount = takeList.Count;
        var useCount = takeList.Count(takeData => Subject.UseSkillDataManager.CheckUsedSkill(takeData.Guid));
        if (useCount < allCount)
        {
            var addCount = allCount - useCount;
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10041, Subject, addCount);
        }
    }
}