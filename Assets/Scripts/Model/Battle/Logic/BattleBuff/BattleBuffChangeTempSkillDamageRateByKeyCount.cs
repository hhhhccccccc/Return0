using cfg;

public class BattleBuffChangeTempSkillDamageRateByKeyCount : BattleBuffBase
{
    private float AddValue;
    protected override void OnStart()
    {
        base.OnStart();
        var keyCount = Subject.GetKeyCount();
        AddValue = Config.ParamEx[0] * keyCount;
        Subject.ChangeProperty(BattlePropertyType.TempSkillDamageAddValue, AddValue, BattleSource.Skill);
    }

    protected override void OnBuffRemove()
    {
        Subject.ChangeProperty(BattlePropertyType.TempSkillDamageAddValue, -AddValue, BattleSource.Skill);
    }
}
