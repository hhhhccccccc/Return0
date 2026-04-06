using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10003 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var addCount = 0;
            var skill = Subject.GetSkill();
            if (skill == null)
            {
                return;
            }

            var other = GetOtherUnit(paramModel);
            //自己的目标是攻击方 且攻击方的技能式杀式
            if (other == skill.Target && CheckSkillIsKillingStyle(other, true))
            {
                addCount++;
                if (model.BattleClashType == BattleClashType.DoubleClash &&
                    CheckSkillIsKillingStyle(Subject, true) &&
                    CheckSkillIsKillingStyle(other, true))
                {
                    addCount++;
                }

                if (CanTrigger)
                {
                    DoAddBuff(Subject, GameConst.Battle.BuffJiaoMing, Subject, addCount, null, BattleMomentType.AfterUnderAction);
                    CanTrigger = false;
                }
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var addCount = 0;
            var other = GetOtherUnit(paramModel);
            var otherSkill = other.GetSkill();
            if (otherSkill == null)
            {
                return;
            }
            //对方的目标是自己 且自己是杀式
            if (otherSkill.Target == Subject && CheckSkillIsKillingStyle(Subject, true))
            {
                addCount++;
                if (model.BattleClashType == BattleClashType.DoubleClash &&
                    CheckSkillIsKillingStyle(Subject, true) &&
                    CheckSkillIsKillingStyle(other, true))
                {
                    addCount++;
                }

                if (CanTrigger)
                {
                    DoAddBuff(Subject, GameConst.Battle.BuffJiaoMing, Subject, addCount, null, BattleMomentType.ReleaseSkillAction);
                    CanTrigger = false;
                }
            }
        }
    }

    public override void ClearTempData()
    {
        CanTrigger = true;
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = true;
    }
}