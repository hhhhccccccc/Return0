using cfg;

public partial class UIBattleKeyTxtItem
{
    public void Refresh(BattleKeyType keyType)
    {
        if (keyType == BattleKeyType.KeyUp)
        {
            TxtKey.SetText("↑");
        }
        
        if (keyType == BattleKeyType.KeyDown)
        {
            TxtKey.SetText("↓");
        }
        
        if (keyType == BattleKeyType.KeyLeft)
        {
            TxtKey.SetText("←");
        }
        
        if (keyType == BattleKeyType.KeyRight)
        {
            TxtKey.SetText("→");
        }
    }
}
