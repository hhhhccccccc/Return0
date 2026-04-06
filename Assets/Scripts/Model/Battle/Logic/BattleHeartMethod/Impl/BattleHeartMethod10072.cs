using System.Collections.Generic;
using System.Linq;
using cfg;

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
            var buffID = GameConst.Battle.BuffLiuJinWuShaShiShang + ((int)skillType - 1) * 4 + keyType;
            var buff = DoAddBuff(unit, buffID, Subject, 1, null, BattleMomentType.AfterAction);
            if (buff != null)
            {
                if (!RoundAddList.Contains(model.EntityID))
                {
                    RoundAddList.Add(model.EntityID);
                    DoAddActionTimes(Subject, ActionTimes);
                }
            }
            
            CanTrigger = false;
        }
    }

    public override void EveryActionWheelStart()
    {
        CanTrigger = true;
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
        RoundAddList.Clear();
    }
}