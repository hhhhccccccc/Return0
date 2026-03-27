using System.Collections.Generic;
using Zenject;

public class Skill4079 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400004 - AddRandomKey
        Subject.AddRandomKey(4, (ChangeKeyReason)4);
    }

}