using cfg;
using Zenject;

public class BattleSkillBase : BattleSkillMoment, IModel
{
    [Inject] private IConfigManager ConfigManager;
    
    public int SkillID;

    public BattleUnit Subject;

    public BattleUnit Target;

    public BattleSkillConfig Cfg;
    public void Init(int skillID, BattleUnit subject, BattleUnit target)
    {
        SkillID = skillID;
        Cfg = ConfigManager.GetBattleSkill(skillID);
        Subject = subject;
        Target = target;
        InitMoment(this);
    }

    public int GetSkillDamageValue() => Cfg.Damage;
}
