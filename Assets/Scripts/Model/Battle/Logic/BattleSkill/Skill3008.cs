using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3008 : BattleSkillBase
{
    //招式的刚炁消耗转为当前60%，至多60
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.6f, 60);
    }

    //2息内获得1层回避状态（下一息结束后清除1层回避状态）
    public override void SelfActionWheelStart()
    {
        DoAddBuff(Subject, 90017, Subject, 2, null, BattleMomentType.SelfActionWheelStart);
    }
}