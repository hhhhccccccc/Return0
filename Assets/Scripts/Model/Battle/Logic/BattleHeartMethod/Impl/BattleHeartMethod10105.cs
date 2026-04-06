using System.Linq;
using cfg;

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
            DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, addCount, null, BattleMomentType.RoundStart);
        }
    }
}