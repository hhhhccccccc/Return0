using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1039 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 下次行动决定后获得1次行动次数
        DoAddBuff(Subject, 90006, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //todo 期间受到攻击减少2层毒瘴状态
        //根据毒瘴状态层数获得增益（1：2次随机获得1层武增/术增/迅速/巧增），
        AddGainBuffByBuffIDCount(Subject, GameConst.Battle.BuffDuZhang, 2, 200002, BattleMomentType.ReleaseSkillAction);
    }
}