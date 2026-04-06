using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30241 : BattleBuffBase
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
            if (keyType == BattleKeyType.KeyRight || keyType == BattleKeyType.KeyDown)
            {
                DoAddActionTimes(Subject, 1);
                var skillType = skill.GetSKillType;
                if (keyType == BattleKeyType.KeyRight && skillType != SkillType.SpellFormula)
                {
                    DoAddRandomKey(Subject, 3, ChangeKeyReason.BuffEffect);
                }

                if (keyType == BattleKeyType.KeyDown && skillType == SkillType.SpellFormula)
                {
                    DoAddRandomKey(Subject, 3, ChangeKeyReason.BuffEffect);
                }
            }
        }
        
        ClearLayerCount();
    }
}
