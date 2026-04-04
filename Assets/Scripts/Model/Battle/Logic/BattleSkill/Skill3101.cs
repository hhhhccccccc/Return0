using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill3101 : BattleSkillBase
{
    //若带有增益状态则在下一息将以随机目标重复该行动，至多重复1次
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (Subject.HasBuffType(BuffType.Gain))
        {
            var enemies = BattleManager.GetAllOpponentUnit(Subject.EntityID, true).Where(enemy => Subject.CheckSkillCanDoDesition_Logic(SkillGuid, enemy)).ToList();
            if (enemies.Count > 0)
            {
                var randomTarget = Util.GetRandom(enemies);
                return new BattleSkillRepeatData
                {
                    SkillID = SkillID,
                    VariantID = VariantID,
                    TargetID = randomTarget.EntityID,
                    MaxRepeatCount = 1,
                    IfLostChangeToOther = false
                };
            }
        }

        return null;
    }
}