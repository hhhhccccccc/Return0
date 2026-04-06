using cfg;

public class BattleBuff10121 : BattleBuffBase
{
    //抵免{[int]}次敌手武杀式、技御式带来的异常状态，防增加30+GR*3
    
    
    /// <summary>
    /// 是否触发过 一次可以触发很多buff  阻挡一次技能的全部buff
    /// </summary>
    private bool IsTrigger { get; set; }
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.DefendInt)
        {
            return GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        }

        return 0;
    }

    public override bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType)
    {
        var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
        var spellCaster = BattleManager.GetUnit(spellCasterID);
        if (buffConfig.BuffType == (int)BuffType.Abnormal && momentType == BattleMomentType.ReleaseSkillAction &&
            spellCaster.HasBuff(GameConst.Battle.BuffCangShen) &&
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
            DoReduceBuffLayerCount(Subject, BuffID, 1);
        }
    }

    protected override void OnBuffRecycle()
    {
        IsTrigger = false;
    }
}
