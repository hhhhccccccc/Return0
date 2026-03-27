using System.Collections.Generic;
using Zenject;

public class Skill3092 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 6301201 - ChangeChrono
        // TODO: ChangeChrono
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 6401201 - ChangeWeather
        // TODO: ChangeWeather
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 6700001 - SetMinRecoverQiNatural
        // TODO: SetMinRecoverQiNatural
    }

}