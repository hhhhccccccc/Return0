using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1004 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    //行动期间受到直接伤害时恢复伤害量30%的体，本次行动不会被破招
    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (!isReduce)
        {
            return;
        }
        
        if (!Subject.IsAlive())
        {
            return;
        }
        
        if (!IsInAction)
        {
            return;
        }

        if (isReduceHpMax)
        {
            return;
        }

        if (damageType != DamageType.Direct)
        {
            return;
        }
        
        var pct = Config.ParamEx[0];
        var healValue = changeHp * pct;
        Subject.HealHp(healValue, BattleSource.Skill);
    }

    // Moment: 1004005 → 条件: 1300001 → 效果: 目标被缴械
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            // 条件: 1300001 → CheckMutualGoal → 判断是否互为目标
            if (CheckMutualGoal(Subject, target))
            {
                // 效果: 119000701 - AddBuff → 目标被缴械
                DoAddBuff(target, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
            }
        }
    }

    // Moment: 1004007 → 条件: 1000011 → 效果: 设置技能期间被打了
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        // 条件: 1000011 → CheckDamageType → 判断对方的伤害类型是直接伤害
        //todo
        // 效果: 3300001 - SetBeDamageInSkillAction → 设置技能期间被打了
        DoSetBeDamageInSkillAction(Subject);
    }

    // Moment: 1004004 → 无条件 → 移除自己受击回血90003,1层
    // Moment: 1004008 → 条件: 100001 → 效果: 101001 (恢复刚气15)
    public override void SkillEnd(BattleSkillBase skill)
    {
        // 效果: 19000301 - RemoveBuff → 移除自己受击回血90003,1层
        DoRemoveBuff(Subject, 90003);
        
        // 条件: 100001 → CheckBeDamageInSkillAction → 自己行动期间被打了
        // 效果: 101001 - ChangeProperty → 恢复刚气15
        if (CheckBeDamageInSkillAction())
        {
            DoChangeProperty(Subject, BattlePropertyType.GangQi, 15);
        }
    }
}