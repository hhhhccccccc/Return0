using System;
using cfg;
using Zenject;

public class BattleBuff10221 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    private bool NeedTransfer { get; set; }
    private int TriggerActionWheel { get; set; }

    protected override void OnStart()
    {
        base.OnStart();
        NeedTransfer = false;
        Register<UnitTriggerBeforeActionMomentEventModel>(OnUnitTriggerBeforeActionMoment);
    }

    protected override void OnActionWheelEnd()
    {
        base.OnActionWheelEnd();
        if (NeedTransfer && TriggerActionWheel != BattleLogicStateManager.ActionWheel)
        {
            NeedTransfer = false;
            TriggerActionWheel = 0;
            ClearLayerCount();
        }
    }

    private void OnUnitTriggerBeforeActionMoment(UnitTriggerBeforeActionMomentEventModel model)
    {
        if (NeedTransfer && TriggerActionWheel == BattleLogicStateManager.ActionWheel + 1)
        {
            var target = BattleManager.GetUnit(model.AttackerID);
            if (target.Bf == Subject.Bf)
            {
                BattleBuffManager.AddBuff(target, BuffID, Subject, LayerCount);
                ReduceLayerCount(LayerCount);
            }
        }
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        base.OnAfterAction(paramModel);
        var value = Config.ParamEx[1] + Subject.Gr * Config.ParamEx[2];
        Subject.HealHp(value, BattleSource.Buff);
        NeedTransfer = true;
        TriggerActionWheel = BattleLogicStateManager.ActionWheel;
        ReduceLayerCount(1);
    }

    protected override void OnRoundEnd()
    {
        NeedTransfer = false;
        base.OnRoundEnd();
    }

    public override void ClearLayerCount()
    {
        var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
        ReduceLayerCount(reduceCount);
    }

    protected override void OnBuffRemove()
    {
        NeedTransfer = false;
        TriggerActionWheel = 0;
        base.OnBuffRemove();
    }
}
