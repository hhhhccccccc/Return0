using cfg;

public class BattleBuff10131 : BattleBuffBase
{
    //抵免{[int]}次敌手术杀式、法咒式带来的异常状态，破增加30+GR*3
    
    
    /// <summary>
    /// 是否触发过 一次可以触发很多buff  阻挡一次技能的全部buff
    /// </summary>
    private bool IsTrigger { get; set; }
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.BreakInt)
        {
            return Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        }

        return 0;
    }

    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        var spellCaster = BattleManager.GetUnit(spellCasterID);
        if (buffConfig.BuffType == (int)BuffType.Abnormal && momentType == BattleMomentType.ReleaseSkillAction &&
            spellCaster.HasBuff(GameConst.Battle.BuffYinHun) &&
            (spellCaster.GetSkillType() == SkillType.ArtKilling ||
             spellCaster.GetSkillType() == SkillType.SpellFormula))
        {
            TriggerBuffMomentByCountIgnoreLayerCount(1, null);
            return false;
        }

        return true;
    }

    protected override void OnTriggerBuffMomentByCountIgnoreLayerCount(int count, MomentParamModel paramModel)
    {
        IsTrigger = true;
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (IsTrigger)
        {
            IsTrigger = false;
            DoReduceBuffLayerCount(Subject, BuffID, 1);
        }
    }
    protected override void OnBuffRecycle()
    {
        IsTrigger = false;
    }
}
