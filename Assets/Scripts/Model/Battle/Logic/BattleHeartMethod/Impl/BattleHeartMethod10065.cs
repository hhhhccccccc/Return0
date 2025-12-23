using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10065 : BattleHeartMethodBase
{
    private int MinChangeValue => GetParamInt(0);
    private bool CanTrigger { get; set; }
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        var skill = Subject.GetSkill();
        if (skill != null && isPreDesition)
        {
            CanTrigger = true;
            var preCalculate = Subject.PreChangeActionWheel;
            if (preCalculate < MinChangeValue)
            {
                var delta = MinChangeValue - preCalculate;
                Subject.ChangeActionWheel(delta);
            }
        }
    }

    public override void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        if (changeActionWheel < MinChangeValue)
        {
            changeActionWheel = MinChangeValue;
        }
    }
    
    public override void EndAction()
    {
        CanTrigger = false;
        base.EndAction();
    }
}