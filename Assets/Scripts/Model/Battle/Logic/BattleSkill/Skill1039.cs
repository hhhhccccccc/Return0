using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1039 : BattleSkillBase
{
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        return 1;
    }
    //todo 效果: 下次行动决定后获得1次行动次数 
    public override void SelfActionWheelStart()
    {
        DoAddBuff(Subject, 90006, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //根据毒瘴状态层数获得增益（1：2次随机获得1层武增/术增/迅速/巧增），
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        AddPoolBuffByBuffIDCount(Subject, GameConst.Battle.BuffDuZhang, 2, 200002, BattleMomentType.ReleaseSkillAction);
    }
    //期间受到攻击减少2层毒瘴状态
    public override void BeDamage(DamageType damageType)
    {
        if (damageType == DamageType.Direct)
        {
            var buff = Subject.GetBuff(GameConst.Battle.BuffDuZhang);
            if (buff != null)
            {
                buff.ReduceLayerCount(2);
            }
        }
    }
}