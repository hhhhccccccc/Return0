using System;
using cfg;
using Zenject;

public class BattleBuff20351 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }

    public override int ClearLayerCount()
    {
        var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
        return ReduceLayerCount(reduceCount);
    }

    protected override void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (damageType == DamageType.Direct && isReduce)
        {
            var reduceCount = Math.Min(Config.ParamEx[0].ToInt(), LayerCount);
            DoReduceBuffLayerCount(Subject, BuffID, reduceCount);
        }
    }

    protected override bool OnCheckSkillCanUse(int skillGuid, BattleUnit target) => false;
}
