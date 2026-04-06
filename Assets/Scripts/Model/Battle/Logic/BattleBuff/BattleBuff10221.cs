using System;
using cfg;
using Zenject;

public class BattleBuff10221 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    private bool NeedTransfer { get; set; }
    private int TriggerActionWheel { get; set; }

    protected override void OnBuffStart()
    {
        NeedTransfer = false;
        Register<UnitTriggerBeforeActionMomentEventModel>(OnUnitTriggerBeforeActionMoment);
    }

    protected override void OnActionWheelEnd()
    {
        if (NeedTransfer && TriggerActionWheel != BattleLogicStateManager.ActionWheel)
        {
            NeedTransfer = false;
            TriggerActionWheel = 0;
            DoClearBuffLayerCount(Subject, BuffID);
        }
    }

    private void OnUnitTriggerBeforeActionMoment(UnitTriggerBeforeActionMomentEventModel model)
    {
        if (NeedTransfer && TriggerActionWheel == BattleLogicStateManager.ActionWheel + 1)
        {
            var target = BattleManager.GetUnit(model.AttackerID);
            if (target.Bf == Subject.Bf)
            {
                DoAddBuff(target, BuffID, target, LayerCount, null, BattleMomentType.BeforeAction);
                DoReduceBuffLayerCount(Subject, BuffID, LayerCount);
            }
        }
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        var value = Config.ParamEx[1] + Subject.Gr * Config.ParamEx[2];
        Subject.HealHp(value, BattleSource.Buff);
        NeedTransfer = true;
        TriggerActionWheel = BattleLogicStateManager.ActionWheel;
        DoReduceBuffLayerCount(Subject, BuffID, 1);
    }

    protected override void OnRoundEnd()
    {
        NeedTransfer = false;
    }

    public override int ClearLayerCount()
    {
        var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
        return ReduceLayerCount(reduceCount);
    }

    protected override void OnBuffRemove()
    {
        NeedTransfer = false;
        TriggerActionWheel = 0;
    }
}
