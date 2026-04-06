using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

//todo 表现
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
        if (BattleLogicStateManager.ActionWheel == Subject.ActionWheel)
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
            BattleBuffManager.AddBuff(unit, GameConst.Battle.BuffCanQue, Subject, GetConfigParamInt(0));
            EntityIDSet.Add(entityID);
        }
    }
    
    public override void EveryActionWheelStart()
    {
        base.EveryActionWheelStart();
        EntityIDSet.Clear();
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        EntityIDSet.Clear();
    }

    private void OnUnitChangeKey(UnitChangeKeyEventModel model)
    {
        if (BattleLogicStateManager.ActionWheel == Subject.ActionWheel)
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
            BattleBuffManager.AddBuff(unit, GameConst.Battle.BuffCanQue, Subject, GetConfigParamInt(0));
            EntityIDSet.Add(model.UnitID);
        }
    }
}