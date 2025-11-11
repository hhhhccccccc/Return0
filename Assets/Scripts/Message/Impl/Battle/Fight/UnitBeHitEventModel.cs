using cfg;

public class UnitBeHitEventModel : MessageModel
{
    public int AttackID { get; set; }
    public int HitID { get; set; }
    public float DamageValue { get; set; }
    public DamageType DamageType { get; set; }

    public override void Recycle()
    {
        AttackID = 0;
        HitID = 0;
        DamageValue = 0;
        DamageType = DamageType.None;
        base.Recycle();
    }
}
