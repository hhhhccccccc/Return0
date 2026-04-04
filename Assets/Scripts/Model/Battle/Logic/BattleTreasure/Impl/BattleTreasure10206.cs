using System.Collections.Generic;
using cfg;
using System.Linq;
public class BattleTreasure10206 : BattleTreasureBase
{
    protected override void OnAddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var value = Subject.GetProperty(BattlePropertyType.MaxHp) * GetConfigParamFloat(0) * model.GetSelfFinalDamageWelly(Subject.EntityID);
            dict.Add(GetSymbol, value);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddDamageInt, GetSymbol, value);
        }
    }
}


