using System;
using cfg;
using Zenject;

public class BattleBuff30411 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnBeAttack(float reduceHp, DamageType damageType, int attackID)
    {
        if (!Subject.IsAlive())
        {
            var attacker = BattleManager.GetUnit(attackID);
            var skillType = attacker.GetSkillType();
            if (skillType == SkillType.ArtKilling)
            {
                ReduceLayerCount(1);
            }

            if (LayerCount <= 0)
            {
                //todo 游戏结束
                return;
            }

            var teamUnit = Util.GetRandom(Subject.Bf.GetAliveUnit());
            BattleBuffManager.AddBuff(teamUnit, BuffID, Subject, LayerCount);
        }
    }
}
