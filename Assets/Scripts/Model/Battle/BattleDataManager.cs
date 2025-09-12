using System.Collections.Generic;

public class BattleDataManager : SingleModel
{
    public List<BattlePlayerData> Players;

    public void SetPlayerData(List<BattlePlayerData> players)
    {
        Players = players;
    }
}
