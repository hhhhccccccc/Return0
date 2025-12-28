using cfg;

public class BattleBuff10121 : BattleBuffBase
{
    private bool IsTrigger { get; set; }
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.DefendInt)
        {
            return Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        }

        return 0;
    }

    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType = BattleMomentType.None)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        var spellCaster = BattleManager.GetUnit(spellCasterID);
        if (buffConfig.BuffType == (int)BuffType.Abnormal && momentType == BattleMomentType.ReleaseSkillAction &&
            spellCaster.HasBuff(GameConst.Battle.Buff10121) &&
            (spellCaster.GetSkillType() == SkillType.PowerKilling ||
             spellCaster.GetSkillType() == SkillType.TechniqueImperialStyle))
        {
            TriggerBuffMomentByCountIgnoreLayerCount(1, null);
        }
        
        return true;
    }

    protected override void OnTriggerBuffMomentByCountIgnoreLayerCount(int count, MomentParamModel paramModel)
    {
        IsTrigger = true;
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        if (IsTrigger)
        {
            IsTrigger = false;
            ReduceLayerCount(1);
        }
    }
    

    protected override void OnRecycle()
    {
        IsTrigger = false;
    }
}
