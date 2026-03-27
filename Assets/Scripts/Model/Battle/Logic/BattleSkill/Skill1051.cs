using System.Collections.Generic;
using Zenject;

public class Skill1051 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400001 - AddRandomKey
        Subject.AddRandomKey(1, (ChangeKeyReason)4);
    }

}