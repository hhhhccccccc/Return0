using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1004 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
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

    //若互为目标则敌手因招式效果获得的炁-100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            if (CheckMutualGoal(Subject, target))
            {
                DoReduceHealQi(target, BattleMomentType.BeforeClash);
            }
        }
    }

    //行动期间若受到过直接伤害则刚炁+15
    public override void SkillEnd(BattleSkillBase skill)
    {
        if (CheckBeDamageInSkillAction(Subject))
        {
            DoChangeProperty(Subject, BattlePropertyType.GangQi, 15, BattleSource.Skill);
        }
    }
}