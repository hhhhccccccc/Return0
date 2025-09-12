using System.Collections.Generic;

public class BattlePlayerData : IModel
{
    public int Uid;


    /// <summary>
    /// entityID, character
    /// </summary>
    public List<HeroData> HeroDatas = new();
}
