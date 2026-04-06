using cfg;

public class BattleBuff10061 : BattleBuffBase
{
    /// <summary>
    /// 每层使获得的玄炁增加1
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRecInt)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
