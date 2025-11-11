using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30191 : BattleBuffBase
{
    protected override void OnBeforeAction()
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return;
        }

        var costKeyList = skill.GetKeyCostList;
        if (costKeyList.Count > 0)
        {
            var keyType = (BattleKeyType)costKeyList[0];
            if (keyType == BattleKeyType.KeyUp || keyType == BattleKeyType.KeyRight)
            {
                Subject.AddActionTimes(1);
                var skillType = skill.GetSKillType;
                if (keyType == BattleKeyType.KeyUp && skillType != SkillType.TechniqueImperialStyle)
                {
                    Subject.AddRandomKey(3, ChangeKeyReason.BuffEffect);
                }

                if (keyType == BattleKeyType.KeyRight && skillType == SkillType.TechniqueImperialStyle)
                {
                    Subject.AddRandomKey(3, ChangeKeyReason.BuffEffect);
                }
            }
        }
    }
}
