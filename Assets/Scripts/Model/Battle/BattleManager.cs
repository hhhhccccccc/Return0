using System.Collections.Generic;
using Zenject;

public class BattleManager : SingleModel
{
    [Inject] private IPoolManager PoolManager;
    [Inject] private ILogManager LogManager;
    [Inject] private BattleDataManager BattleDataManager;
    public int Round;
    public List<BattleField> BfList { get; private set; }
    
    public BattleField SelfBf;
    public BattleField OtherBf;

    public BattleState BattleState => _battleState;
    private BattleState _battleState;
    
    public void BattleInit(List<PlayerData> players)
    {
        BattleDataManager.SetPlayerData(players);
        BfList = new List<BattleField>();
        foreach (var playerData in players)
        {
            var bf = PoolManager.GetClass<BattleField>();
            bf.Init(playerData);
            BfList.Add(bf);
            if (playerData.Uid == 1)
            {
                SelfBf = bf;
            }
            else
            {
                OtherBf = bf;
            }
        }
        
        MessageManager.Dispatch<BattleLogicReadyEventModel>(null);
    }

    public void BattleStart()
    {
        LogManager.Debug("[战斗开始]");
        MessageManager.Dispatch<BattleRoundStartEventModel>(null);
    }

    public void RoundStart()
    {
        Round = 1;
        _battleState = BattleState.Running;
    }
    
    public BattleField GetBf(int uid)
    {
        return uid == 1 ? SelfBf : OtherBf;
    }
}