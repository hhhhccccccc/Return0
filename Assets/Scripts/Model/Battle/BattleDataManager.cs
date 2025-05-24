using System.Collections.Generic;

public class BattleDataManager : SingleModel
{
    public List<PlayerData> Players;

    public void SetPlayerData(List<PlayerData> players)
    {
        Players = players;
    }
}
