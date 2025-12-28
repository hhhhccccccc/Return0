using System.Collections.Generic;
using cfg;
using System.Linq;
public class BattleTreasure10206 : BattleTreasureBase
{
    protected override void OnAddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            dict.Add(GetSymbol, Subject.GetProperty(BattlePropertyType.MaxHp) * GetParamFloat(0) * model.GetSelfFinalDamageWelly(Subject.EntityID));
        }
    }
}


