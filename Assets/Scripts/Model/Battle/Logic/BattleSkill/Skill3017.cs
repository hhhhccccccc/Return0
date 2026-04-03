using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3017 : BattleSkillBase
{
    private List<int> BuffList = new()
    {
        GameConst.Battle.BuffFengXueShang,
        GameConst.Battle.BuffFengXueXia,
        GameConst.Battle.BuffFengXueZuo,
        GameConst.Battle.BuffFengXueYou,
    };
    //施加随机按键的2层封穴
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var randomBuffID = Util.GetRandom(BuffList);
        DoAddBuff(Target, randomBuffID, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}