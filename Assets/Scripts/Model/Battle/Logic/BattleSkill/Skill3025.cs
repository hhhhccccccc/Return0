using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3025 : BattleSkillBase
{
    //玄炁+30
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 30, BattleSource.Skill);
    }

    //未带有异常状态则招式的威力增加25的百分比
    protected override void OnSelfActionWheelStart()
    {
       
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CheckBuffTypeCount(Subject, BuffType.Abnormal, 0, DataRelation.XiaoYuDengYu))
        {
            return 0.25f;
        }

        return 0;
    }
}