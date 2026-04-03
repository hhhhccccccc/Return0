using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2071 : BattleSkillBase
{
    //每次使用该招式威力增加10的百分比且获得的迅速状态增加1层、玄消耗增加20；每消耗1个键减少5玄炁消耗但不会低于{[int]}（10+使用次数*5）直到下次使用
    
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 1, null, BattleMomentType.DoDesitionAction);
    }
}