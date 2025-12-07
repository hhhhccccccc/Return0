using cfg;

public class BattleBuff99999 : BattleBuffBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (Subject.IsAlive() && paramModel is DamageParamModel { HitHpValue: > 0 } model)
        {
            Subject.TryBeCounter(model.AttackID); 
            LM.D($"[扳机效果] 设置破招 {Subject.EntityID}被破招了");
        }
    }
}
