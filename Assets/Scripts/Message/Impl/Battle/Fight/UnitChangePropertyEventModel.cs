using cfg;

public class UnitChangePropertyEventModel : MessageModel
{
    public int UnitID { get; set; }
    public BattlePropertyType PropType { get; set; }
    public float PropValue { get; set; }
    public BattleSource Source { get; set; }
    public override void Recycle()
    {
        UnitID = 0;
        PropType = BattlePropertyType.None;
        PropValue = 0;
        Source = 0;
        base.Recycle();
    }
}
