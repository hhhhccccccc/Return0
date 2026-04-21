public partial class UIBattleTeamUnitInfoItem
{
    private BattleUnit Unit { get; set; }
    private UIBattleHeadItem HeadItem { get; set; }
    protected override void OnItemCreate()
    {
        if (HeadItem == null)
        {
            HeadItem = CreateItemByType<UIBattleHeadItem>(TfHeadNode);
        }
    }

    public void Init(BattleRole unit)
    {
        Unit = unit;
        SetSprite(ImgSkill, "1");
        RegisterAction();
        InitInfo();
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
