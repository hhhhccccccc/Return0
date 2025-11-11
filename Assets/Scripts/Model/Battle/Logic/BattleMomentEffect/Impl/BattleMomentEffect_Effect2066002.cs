using cfg;
using Zenject;

public class BattleMomentEffect_Effect2066002 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    private const int BuffID = 20081;
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var skill = target.GetSkill();
                var costKeyCount = skill.GetKeyCostList.Count;
                BattleBuffManager.AddBuff(target, BuffID, Subject, costKeyCount, null, MomentType);
            }
        }
    }
}