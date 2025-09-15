using Zenject;

public class BattleMomentEffect_GetTargetAllSameBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        //添加到谁身上
        var addToTar = GetUnitByParamID(Config.ParamList[0]);
        //从哪里拿到
        var getTar = GetUnitByParamID(Config.ParamList[1]);
        if (addToTar != null && getTar != null)
        {
            foreach (var buff in addToTar.GetBuffList())
            {
                BattleBuffManager.AddBuff(addToTar, buff.BuffID, addToTar, buff.LayerCount, buff.ParamList);
            }
        }
    }
}