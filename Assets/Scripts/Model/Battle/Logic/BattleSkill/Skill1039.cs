using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1039 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }
    protected override void OnSelfActionWheelStart()
    {
        DoAddBuff(Subject, 90006, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //根据毒瘴状态层数获得增益（1：2次随机获得1层武增/术增/迅速/巧增），
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var buffCount = Subject.GetBuffCountByID(GameConst.Battle.BuffDuZhang);
        DoAddPoolBuffByCount(Subject, buffCount * 2, 200002, BattleMomentType.ReleaseSkillAction);
    }
    //期间受到攻击减少2层毒瘴状态
    protected override void OnBeDamage(DamageType damageType)
    {
        if (damageType == DamageType.Direct)
        {
            var buff = Subject.GetBuff(GameConst.Battle.BuffDuZhang);
            if (buff != null)
            {
                DoReduceBuffLayerCount(Subject, buff.BuffID, 2);
            }
        }
    }
}