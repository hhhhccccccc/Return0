using System.Collections.Generic;
using Zenject;

public class Skill3017 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5500003 - AddRandomBuff
        // TODO: AddRandomBuff - 随机添加Buff 2 200007层
    }

}