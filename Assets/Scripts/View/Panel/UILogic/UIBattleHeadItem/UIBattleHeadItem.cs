public partial class UIBattleHeadItem
{
    public BattleUnit Unit { get; set; }
    public void Init(BattleUnit unit)
    {
        Unit = unit;
    }
}
