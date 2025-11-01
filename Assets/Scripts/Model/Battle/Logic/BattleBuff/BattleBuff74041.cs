using cfg;

//下一次术杀式的基础威力不会低于140%技（巧来方计状态）
public class BattleBuff74041 : BattleBuffBase
{
    private int DataID;
    protected override void OnStart()
    {
        base.OnStart();
        if (DataID == 0)
        {
            var skillType = Config.ParamEx[0].ToInt();
            var limitType = Config.ParamEx[1].ToInt();
            var baseValue = Config.ParamEx[2];
            var data = Subject.AddSkillDamageLimit((SkillType)skillType, (BattleSkillDamageLimitType)limitType, baseValue);
            if (data != null)
            {
                DataID = data.Guid;
            }
        }
    }

    protected override void OnBuffRemove()
    {
        if (DataID != 0)
        {
            Subject.RemoveSkillDamageLimit(DataID);
            DataID = 0;
        }
        base.OnBuffRemove();
    }

    public override void Recycle()
    {
        if (DataID != 0)
        {
            Subject.RemoveSkillDamageLimit(DataID);
            DataID = 0;
        }
        base.Recycle();
    }
}
