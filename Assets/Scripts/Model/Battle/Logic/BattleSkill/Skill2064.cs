using System.Collections.Generic;
using Zenject;

public class Skill2064 : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 400003 - AddRandomKey
        Subject.AddRandomKey(3, (ChangeKeyReason)4);
    }

}