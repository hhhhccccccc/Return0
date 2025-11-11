using cfg;

public class BattleMomentEffect_ChangeProperty_Buff : BattleMomentEffect
{
    protected virtual float GetChangePropertyValue() => Config.ParamList[2];
    
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var propertyType = Config.ParamList[1].ToInt();
            var propertyValue = GetChangePropertyValue();
            var source = BattleSource.None;
            if (Config.ParamList.Count > 3)
            {
                source = (BattleSource)Config.ParamList[3].ToInt();
            }

            foreach (var target in targetList)
            {
                target.ChangeProperty((BattlePropertyType)propertyType, propertyValue * BuffLayerCount, source);
                Debug($"[扳机效果] 谁 : {target.EntityID}, 属性改变类型 : {(BattlePropertyType)propertyType}, 属性改变量 : {propertyValue}, 来源 : {source}");
            }
        }
    }
}