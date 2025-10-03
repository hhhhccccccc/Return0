using Zenject;

public class BattleMomentEffect_GetTargetAllSameBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        //添加到谁身上
        var addToTarList = GetUnitByParamID(Config.ParamList[0]);
        //从哪里拿到
        var getTarList = GetUnitByParamID(Config.ParamList[1]);
        if (addToTarList.Count > 0 && getTarList.Count > 0)
        {
            foreach (var buff in addToTarList[0].GetBuffList())
            {
                BattleBuffManager.AddBuff(addToTarList[0], buff.BuffID, addToTarList[0], buff.LayerCount, buff.ParamList);
            }
        }
    }
}