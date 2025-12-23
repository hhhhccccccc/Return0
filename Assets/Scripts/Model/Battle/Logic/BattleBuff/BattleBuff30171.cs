using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30171 : BattleBuffBase
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
            if (keyType == BattleKeyType.KeyDown || keyType == BattleKeyType.KeyLeft)
            {
                Subject.AddActionTimes(1);
                var skillType = skill.GetSKillType;
                if (keyType == BattleKeyType.KeyDown && skillType != SkillType.ArtKilling)
                {
                    Subject.AddRandomKey(3, ChangeKeyReason.BuffEffect);
                }

                if (keyType == BattleKeyType.KeyLeft && skillType == SkillType.ArtKilling)
                {
                    Subject.AddRandomKey(3, ChangeKeyReason.BuffEffect);
                }
            }
        }
        
        ClearLayerCount();
    }
}
