using System;
using cfg;
using Zenject;

public class BattleBuff30411 : BattleBuffBase
{
    protected override void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
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
