using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3066 : BattleSkillBase
{
    //获得4层伤口状态和2层武增状态
    public override void SelfActionWheelStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShangKou, Subject, 4, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 2, null, BattleMomentType.SelfActionWheelStart);
    }

    //施加2层伤口状态2次
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffShangKou, Subject, 2, null, BattleMomentType.BeforeClash);
        DoAddBuff(clashUnit, GameConst.Battle.BuffShangKou, Subject, 2, null, BattleMomentType.BeforeClash);
    }

    //玄炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}