using Zenject;

public partial class UIBattleTeamUnitInfoItem
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    private BattleUnit Unit { get; set; }
    private UIBattleHeadItem HeadItem { get; set; }
    protected override void OnItemCreate()
    {
        if (HeadItem == null)
        {
            HeadItem = CreateItemByType<UIBattleHeadItem>(TfHeadNode);
        }
    }

    protected override void RegisterEvent()
    {
        Register<BattleBehaviourChangedEventModel>(OnBattleBehaviourChanged);
    }

    private void OnBattleBehaviourChanged(BattleBehaviourChangedEventModel model)
    {
        if (model.EntityID != Unit.EntityID)
        {
            return;
        }
        
        RefreshSkillBehaviour();
    }

    public void Init(BattleRole unit)
    {
        Unit = unit;
        InitInfo();
        RegisterAction();
    }

    public void RefreshSkillBehaviour()
    {
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(Unit.EntityID);
        if (behaviour != null)
        {
            ImgSkill.gameObject.SetActive(true);
            TxtSkill.gameObject.SetActive(true);
            var skillID = behaviour.SkillID;
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            if (config != null)
            {
                SetSprite(ImgSkill, config.Icon);
                TxtSkill.SetText(config.Name);
            }
        }
        else
        {
            ImgSkill.gameObject.SetActive(false);
            TxtSkill.gameObject.SetActive(false);
        }
    }

    private void InitInfo()
    {
        TxtName.SetText(Unit.HeroData.HeroName);
        HeadItem.Init(Unit);
        RefreshActionWheel();
    }

    private void RefreshActionWheel()
    {
        TxtActionWheel.SetText(Unit.ActionWheel.GetValue().ToString());
    }

    private void RegisterAction()
    {
        Unit.ActionWheel.RegisterAction(OnActionWheelChanged);
    }
    
    private void OnActionWheelChanged(int actionWheel)
    {
        RefreshActionWheel();
    }
}
