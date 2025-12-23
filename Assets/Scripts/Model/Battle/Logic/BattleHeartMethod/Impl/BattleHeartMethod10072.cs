using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10072 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }
    private HashSet<int> RoundAddList = new();
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
        RoundAddList.Clear();
        Register<UnitTriggerAfterActionMomentEventModel>(OnUnitTriggerAfterActionMoment);
    }

    private void OnUnitTriggerAfterActionMoment(UnitTriggerAfterActionMomentEventModel model)
    {
        if (!CanTrigger)
        {
            return;
        }
        var unit = BattleManager.GetUnit(model.EntityID);
        if (unit.Bf == Subject.Bf)
        {
            return;
        }
        var skill = unit.GetSkill();
        if (skill != null)
        {
            var skillType = skill.GetSKillType;
            var cost = skill.GetKeyCostList;
            var keyType = cost.Last();
            var buffID = 30110 + ((int)skillType - 1) * 4 + keyType;
            var buff = BattleBuffManager.AddBuff(unit, buffID, Subject, 1);
            if (buff != null)
            {
                if (!RoundAddList.Contains(model.EntityID))
                {
                    RoundAddList.Add(model.EntityID);
                    Subject.AddActionTimes(GetParamInt(0));
                }
            }
            
            CanTrigger = false;
        }
    }

    public override void EveryActionWheelStart()
    {
        base.EveryActionWheelStart();
        CanTrigger = true;
    }

    public override void Recycle()
    {
        CanTrigger = false;
        RoundAddList.Clear();
        base.Recycle();
    }
}