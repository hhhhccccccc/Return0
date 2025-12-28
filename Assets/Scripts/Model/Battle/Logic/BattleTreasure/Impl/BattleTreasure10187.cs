using System;
using cfg;

public class BattleTreasure10187 : BattleTreasureBase
{
    private float Accumulate { get; set; }
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

    protected override float OnGetDamageReducePct(int attackID, DamageType damageType)
    {
        var attacker = BattleManager.GetUnit(attackID);
        if (attacker.CheckVariety(HeroVariety.Ghost))
        {
            return GetParamFloat(0);
        }

        return 0;
    }

    protected override void OnRoundEnd()
    {
        if (Accumulate >= GetParamFloat(1))
        {
            for (int i = 0; i < GetParamInt(2); i++)
            {
                var allOpponentUnit = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
                if (allOpponentUnit.Count > 0)
                {
                    var random = Util.GetRandom(allOpponentUnit);
                    random.ReduceHp(GetParamFloat(3), DamageType.InDirect, Subject.EntityID, false,
                        BattleSource.Treasure, false);
                }
            }

            Accumulate = 0;
        }
    }

    protected override void OnRecycle()
    {
        Accumulate = 0;
    }
}


