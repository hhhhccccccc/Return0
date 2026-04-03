using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3025 : BattleSkillBase
{
    private float AddWelly { get; set; }
    
    //玄炁+30
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 30, BattleSource.Skill);
        AddWelly = 0;
    }

    //未带有异常状态则招式的威力增加25的百分比
    public override void SelfActionWheelStart()
    {
        if (CheckBuffTypeCount(Subject, BuffType.Abnormal, 0, DataRelation.XiaoYuDengYu))
        {
            AddWelly = 0.25f;
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        return AddWelly;
    }

    protected override void OnSkillRecycle()
    {
        AddWelly = 0;
    }
}