using cfg;

public partial class UIBattleKeyIconItem
{
    public void Refresh(BattleKeyType keyType)
    {
        if (keyType == BattleKeyType.KeyUp)
        {
            SetSprite(ImgIcon, "key_up");
        }
        
        if (keyType == BattleKeyType.KeyDown)
        {
            SetSprite(ImgIcon, "key_down");
        }
        
        if (keyType == BattleKeyType.KeyLeft)
        {
            SetSprite(ImgIcon, "key_left");
        }
        
        if (keyType == BattleKeyType.KeyRight)
        {
            SetSprite(ImgIcon, "key_right");
        }
    }
}
