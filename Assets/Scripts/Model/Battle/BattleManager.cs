using System.Collections;
using System.Collections.Generic;
using Zenject;

public class BattleManager : IManager
{
    [Inject] private IPoolManager PoolManager;
    public int Round;
    public List<PlayerData> Players;
    private List<BattleField> BfList;
    private BattleField SelfBf;
    private BattleField OtherBf;
    public void Init(List<PlayerData> players)
    {
        Round = 0;
        Players = players;
        BfList = new List<BattleField>();
        for (var i = 0; i < players.Count; i++)
        {
            var bf = PoolManager.GetClass<BattleField>();
            bf.Init(players[i], i == 0);
            BfList.Add(bf);
            if (i == 0)
            {
                SelfBf = bf;
            }
            else
            {
                OtherBf = bf;
            }
        }
    }

    public BattleField GetBf(bool isSelf)
    {
        return isSelf ? SelfBf : OtherBf;
    }
    
    private void BattleStart()
    {
        
    }

    public IEnumerator Init()
    {
        yield break;
    }
}
