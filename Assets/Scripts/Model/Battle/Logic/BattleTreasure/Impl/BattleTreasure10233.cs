using System.Collections.Generic;
using cfg;

public class BattleTreasure10233 : BattleTreasureBase
{
    private HashSet<int> EntityIDList = new();

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            EntityIDList.Add(model.GetOtherID(Subject.EntityID));
        }
    }

    protected override float OnBeDamageReducePct(int attackID, DamageType damageType)
    {
        if (EntityIDList.Contains(attackID))
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }

    protected override void OnRoundEnd()
    {
        EntityIDList.Clear();
    }

    protected override void OnTreasureRecycle()
    {
        EntityIDList.Clear();
    }
}


