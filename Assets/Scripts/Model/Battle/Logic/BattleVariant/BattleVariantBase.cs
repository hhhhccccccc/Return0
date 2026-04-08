using cfg;

public class BattleVariantBase : BattleMoment
{
    protected BattleVariantConfig Config { get; set; }
    protected override int GetSymbol => 500000 + Config.Id;
    protected int SkillGuid { get; set; }
    protected int SkillID { get; set; }
    protected int VariantID { get; set; }
    protected BattleUnit Target { get; set; }
    protected BattleSkillBase Skill { get; set; }
    
    protected override float GetConfigParamFloat(int index)
    {
        return Config.ParamEx[index];
    }

    public override int GetConfigParamInt(int index)
    {
        return Config.ParamEx[index].ToInt();
    }

    public void Init(int skillGuid, BattleUnit subject, BattleUnit target, BattleSkillBase skillBase)
    {
        SkillGuid = SkillGuid;
        (SkillID, VariantID) = Util.UnCombSkillGuid(skillGuid);
        Config = ConfigManager.GetBattleVariantConfig(VariantID);
        Subject = subject;
        Target = target;
        Skill = skillBase;
        OnInit();
    }

    protected virtual void OnInit()
    {
        
    }

    protected override void OnRecycle()
    {
        SkillGuid = 0;
        SkillID = 0;
        VariantID = 0;
        Target = null;
        Skill = null;
        OnVariantRecycle();
    }

    protected virtual void OnVariantRecycle()
    {
        
    }
}
