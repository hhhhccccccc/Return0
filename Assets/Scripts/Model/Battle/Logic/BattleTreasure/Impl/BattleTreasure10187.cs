using System;
using cfg;

public class BattleTreasure10187 : BattleTreasureBase
{
    private float Accumulate { get; set; }
    private float DamageValue => GetConfigParamFloat(3);
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        Accumulate = 0;
        Register<UnitChangePropertyEventModel>(OnUnitChangeProperty);
    }

    private void OnUnitChangeProperty(UnitChangePropertyEventModel model)
    {
        if (model.PropType == BattlePropertyType.XuanQi && model.PropValue < 0)
        {
            Accumulate += Math.Abs(model.PropValue);
        }
    }

    protected override float OnBeDamageReducePct(int attackID, DamageType damageType)
    {
        var attacker = BattleManager.GetUnit(attackID);
        if (attacker.CheckVariety(HeroVariety.Ghost))
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }

    protected override void OnRoundEnd()
    {
        if (Accumulate >= GetConfigParamFloat(1))
        {
            for (int i = 0; i < GetConfigParamInt(2); i++)
            {
                var allOpponentUnit = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
                if (allOpponentUnit.Count > 0)
                {
                    var random = Util.GetRandom(allOpponentUnit);
                    DoReduceHp(random, DamageValue, DamageType.InDirect, Subject, BattleSource.Treasure);
                }
            }
            
            Accumulate = 0;
        }
    }

    protected override void OnTreasureRecycle()
    {
        Accumulate = 0;
    }
}


