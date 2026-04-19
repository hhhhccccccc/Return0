using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10095 : BattleHeartMethodBase
{
    private HashSet<int> EntityIDSet = new();
    
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Register<UnitChangePropertyEventModel>(OnUnitChangeProperty);
        Register<UnitChangeKeyEventModel>(OnUnitChangeKey);
        Register<UnitTriggerAfterActionMomentEventModel>(OnUnitTriggerAfterActionMoment);
    }

    private void OnUnitTriggerAfterActionMoment(UnitTriggerAfterActionMomentEventModel model)
    {
        if (GameConst.Battle.UseItemSkillIDList.Contains(model.SkillID) && model.UseSuccess)
        {
            OnEffect(model.EntityID);
        }
    }

    private void OnUnitChangeProperty(UnitChangePropertyEventModel model)
    {
        if (BattleLogicStateManager.ActionWheel == Subject.ActionWheel.GetValue())
        {
            return;
        }
        
        if ((model.PropType == BattlePropertyType.GangQi || model.PropType == BattlePropertyType.XuanQi) &&
            model.PropValue > 0 && model.Source == BattleSource.Skill)
        {
            OnEffect(model.UnitID);
        }
    }

    private void OnEffect(int entityID)
    {
        if (!EntityIDSet.Contains(entityID))
        {
            var unit = BattleManager.GetUnit(entityID);
            DoAddBuff(unit, GameConst.Battle.BuffCanQue, Subject, GetConfigParamInt(0), null, BattleMomentType.None);
            EntityIDSet.Add(entityID);
        }
    }
    
    public override void EveryActionWheelStart()
    {
        EntityIDSet.Clear();
    }

    public override void RoundEnd()
    {
        EntityIDSet.Clear();
    }

    private void OnUnitChangeKey(UnitChangeKeyEventModel model)
    {
        if (BattleLogicStateManager.ActionWheel == Subject.ActionWheel.GetValue())
        {
            return;
        }
        
        if (EntityIDSet.Contains(model.UnitID))
        {
            return;
        }
        
        if (model.KeyTypeList.Count > 0 && model.Reason == ChangeKeyReason.SkillEffect)
        {
            var unit = BattleManager.GetUnit(model.UnitID);
            DoAddBuff(unit, GameConst.Battle.BuffCanQue, Subject, GetConfigParamInt(0), null, BattleMomentType.None);
            EntityIDSet.Add(model.UnitID);
        }
    }
}