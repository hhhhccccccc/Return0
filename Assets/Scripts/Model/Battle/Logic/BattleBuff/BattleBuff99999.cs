using cfg;

public class BattleBuff99999 : BattleBuffBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (Subject.IsAlive() && paramModel is DamageParamModel model)
        {
            if (model.GetOtherHpValue(Subject.EntityID) > 0)
            {
                Subject.TryBeCounter(model.GetOtherID(Subject.EntityID), model); 
                LM.D($"[扳机效果] 设置破招 {Subject.EntityID}被破招了");
            }
        }
    }
}
