using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10181 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.MaxXuanQiInt)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }

    protected override void OnLayerCountChanged()
    {
        Subject.ForceRefreshPropertyLimit();
        base.OnLayerCountChanged();
    }
}
