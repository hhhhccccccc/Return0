using System;
using cfg;

public class BattleTreasure10196 : BattleTreasureBase
{
    private float Damage => GetParamFloat(0) + GetParamFloat(1) * Subject.Gr;
    private bool CanTrigger => Cd <= 0;
    private bool TriggerAndNotInCd { get; set; }
    private int Cd { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        TriggerAndNotInCd = false;
        Cd = 0;
    }

    protected override void OnBeforeAttack(MomentParamModel paramModel)
    {
        if (!CanTrigger)
        {
            return;
        }
        
        if (paramModel is DamageParamModel model)
        {
            var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
            if (target.GetProperty(BattlePropertyType.Hp) / target.GetProperty(BattlePropertyType.MaxHp) >= GetParamFloat(3))
            {
                TriggerAndNotInCd = true;
            }
            else
            {
                TriggerAndNotInCd = false;
            }
        }
    }

    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    { 
        if (!CanTrigger)
        {
            return;
        }
        
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling)
            {
                var target = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
                target.ReduceHp(Damage, DamageType.InDirect, Subject.EntityID, false, BattleSource.Treasure, false);
                EnqueueViewModel(Subject.EntityID, MomentViewType.Treasure10196, target.EntityID, Damage);
                if (TriggerAndNotInCd)
                {
                    TriggerAndNotInCd = false;
                }
                else
                {
                    Cd = GetParamInt(2);
                }
            }
        }
    }

    protected override void OnRoundEnd()
    {
        if (Cd > 0)
        {
            Cd--;
        }
    }

    protected override void OnRecycle()
    {
        TriggerAndNotInCd = false;
        Cd = 0;
    }
}


