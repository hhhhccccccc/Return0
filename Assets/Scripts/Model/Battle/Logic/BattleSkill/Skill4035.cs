using System.Collections.Generic;
using Zenject;

public class Skill4035 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900002 - ChangeActionWheel
        Subject.ChangeActionWheel(2);
        // 效果: 5500002 - AddRandomBuff
        // TODO: AddRandomBuff - 随机添加Buff 1 200006层
    }

}