using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10149 : BattleHeartMethodBase
{
   //todo 交锋失败时若存在猊煞变状态，则消耗1层抵免破招效果
   public override bool CheckDontBeCounter(MomentParamModel paramModel)
   {
      var buff = Subject.GetBuff(GameConst.Battle.BuffNiSha);
      if (buff == null)
      {
         return false;
      }
      
      buff.ReduceLayerCount(GetConfigParamInt(0));
      return true;
   }
}