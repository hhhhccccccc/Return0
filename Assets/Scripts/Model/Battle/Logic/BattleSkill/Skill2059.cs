using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2059 : BattleSkillBase
{
    private List<int> BuffList = new()
    {
        GameConst.Battle.BuffFengXueShang,
        GameConst.Battle.BuffFengXueXia,
        GameConst.Battle.BuffFengXueZuo,
        GameConst.Battle.BuffFengXueYou,
    };


    
    //施加随机按键的1层封穴状态和1层过劲状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var randomBuff = Util.GetRandom(BuffList);
        DoAddBuff(Target, randomBuff, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    private bool DontBeCounter = false;

    //若敌手的毒瘴状态层数不低于自身则不会被敌手破招
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        if (CheckBuffCompare(Subject, GameConst.Battle.BuffDuZhang, clashUnit, GameConst.Battle.BuffDuZhang,
                DataRelation.XiaoYu))
        {
            DontBeCounter = true;
        }
    }

    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        if (DontBeCounter)
        {
            return 1;
        }

        return 0;
    }

    public override void ClearTempData()
    {
        DontBeCounter = false;
    }

    protected override void OnSkillRecycle()
    {
        DontBeCounter = false;
    }
}