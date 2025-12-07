using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10146 : BattleHeartMethodBase
{
    public override float GetReplaceSkillXuanQiCost()
    {
        return Subject.GetProperty(BattlePropertyType.GangQi);
    }
    
    public override void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        if (xuanQiDelta <= 0)
            return;


        var replace = Subject.GetProperty(BattlePropertyType.GangQi);
        if (replace >= xuanQiDelta)
        {
            Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, xuanQiDelta, BattleSource.HeartMethod);
            xuanQiDelta = 0;
        }
        else
        {
            Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, replace, BattleSource.HeartMethod);
            xuanQiDelta -= replace;
        }
    }
}