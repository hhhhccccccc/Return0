using System;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuff10181 : BattleBuffBase
{
    /// <summary>
    /// 每层玄炁上限增加10
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.MaxXuanQiInt)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }

    public override void BuffLayerCountChanged(int buffID, int layerCount)
    {
        DoForceRefreshPropertyLimit(Subject);
    }
}
