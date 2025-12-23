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
        base.AfterUnderAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var addCount = 0;
            var selfBehaviour = BattleLogicBehaviourManager.GetBattleBehaviour(Subject.EntityID);
            if (selfBehaviour == null)
            {
                return;
            }
            //自己的目标式攻击方 且攻击方的技能式杀式
            if (model.SelfID == selfBehaviour.TargetID && BattleUtil.SkillIsKillingStyle(model.GetOtherID(Subject.EntityID)))
            {
                addCount++;
                if (model.BattleClashType == BattleClashType.DoubleClash &&
                    BattleUtil.SkillIsKillingStyle(model.GetSelfID(Subject.EntityID)) &&
                    BattleUtil.SkillIsKillingStyle(model.GetOtherID(Subject.EntityID)))
                {
                    addCount++;
                }

                if (CanTrigger)
                {
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30011, Subject, addCount);
                    CanTrigger = false;
                }
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var addCount = 0;
            var targetBehaviour = BattleLogicBehaviourManager.GetBattleBehaviour(model.OtherID);
            if (targetBehaviour == null)
            {
                return;
            }
            //对方的目标是自己 且自己是杀式
            if (targetBehaviour.TargetID == Subject.EntityID && BattleUtil.SkillIsKillingStyle(model.GetSelfSkillID(Subject.EntityID)))
            {
                addCount++;
                if (model.BattleClashType == BattleClashType.DoubleClash &&
                    BattleUtil.SkillIsKillingStyle(model.GetSelfSkillID(Subject.EntityID)) &&
                    BattleUtil.SkillIsKillingStyle(model.GetOtherSkillID(Subject.EntityID)))
                {
                    addCount++;
                }

                if (CanTrigger)
                {
                    BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30011, Subject, addCount);
                    CanTrigger = false;
                }
            }
        }
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        CanTrigger = true;
    }

    public override void Recycle()
    {
        CanTrigger = false;
    }
}