using System;
using cfg;
using Zenject;

public class BattleBuff20351 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }

    public override void ClearLayerCount()
    {
        var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
        ReduceLayerCount(reduceCount);
    }

    protected override void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (damageType == DamageType.Direct)
        {
            var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
            ReduceLayerCount(reduceCount);
        }
    }

    protected override bool OnCheckSkillCanUse(int skillGuid, BattleUnit target) => false;
}
