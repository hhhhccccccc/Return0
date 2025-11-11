using cfg;

public class BattleBuff30261 : BattleBuffBase
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
            if (keyType == BattleKeyType.KeyLeft || keyType == BattleKeyType.KeyUp)
            {
                Subject.AddActionTimes(1);
                var skillType = skill.GetSKillType;
                if (keyType == BattleKeyType.KeyLeft && skillType != SkillType.SpellFormula)
                {
                    Subject.AddRandomKey(3, ChangeKeyReason.BuffEffect);
                }

                if (keyType == BattleKeyType.KeyUp && skillType == SkillType.SpellFormula)
                {
                    Subject.AddRandomKey(3, ChangeKeyReason.BuffEffect);
                }
            }
        }
    }
}
