using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1002 : BattleSkillBase
{
    //获得3个随机的键
    protected override void OnSelfActionWheelStart()
    {
        DoAddRandomKey(Subject, 3, ChangeKeyReason.SkillEffect);
    }

    //补充随机的键到达持有上限
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddRandomKeyToDefineCount(Subject, 0, ChangeKeyReason.SkillEffect);
    }

    //获得1个随机的键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}