using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10149 : BattleHeartMethodBase
{
   public override bool CanBeCounter(MomentParamModel paramModel)
   {
      var buff = Subject.GetBuff(GameConst.Battle.Buff30031);
      if (buff == null)
      {
         return true;
      }
      
      buff.ReduceLayerCount(GetParamInt(0));
      return false;
   }
}