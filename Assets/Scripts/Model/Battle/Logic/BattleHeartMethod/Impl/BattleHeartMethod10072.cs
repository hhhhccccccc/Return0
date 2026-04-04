using System.Collections.Generic;
using System.Linq;
//todo 表现
public class BattleHeartMethod10072 : BattleHeartMethodBase
{
    private int ActionTimes => GetConfigParamInt(0);
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
                    Subject.AddActionTimes(ActionTimes);
                    //todo 表现需要对应做 本体ID, buffID, 行动次数
                    EnqueueViewModel(unit.EntityID, MomentViewType.HeartMethod10072, Subject.EntityID, buff.BuffID, ActionTimes);
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

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
        RoundAddList.Clear();
    }
}