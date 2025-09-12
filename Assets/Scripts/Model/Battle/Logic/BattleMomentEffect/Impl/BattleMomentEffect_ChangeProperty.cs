using cfg;

public class BattleMomentEffect_ChangeProperty : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var propertyType = Config.ParamList[1].ToInt();
            var propertyValue = Config.ParamList[2];
            var source = BattleSource.None;
            if (Config.ParamList.Count > 3)
            {
                source = (BattleSource)Config.ParamList[3].ToInt();
            }
       
            target.ChangeProperty((BattlePropertyType)propertyType, propertyValue, source);
            Debug($"[扳机效果] 谁 : {target.EntityID}, 属性改变类型 : {(BattlePropertyType)propertyType}, 属性改变量 : {propertyValue}, 来源 : {source}");
        }
    }
}