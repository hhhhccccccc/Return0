using System.Collections;
using System.Collections.Generic;
using cfg;
using UnityEngine;
using Zenject;

public partial class BattleSettlementItem
{
    [Inject] private BattleManager BattleManager;

    private List<BattleMomentDesItem> ItemList = new();
    
    public void PlayAnim(string aniName)
    {
        
    }
    
    public void ShowMoment(int entityID, BattleMomentType momentType, BattleSource battleSource, int configID)
    {
        var item = CreateItemByType<BattleMomentDesItem>(TfMomentContent.transform);
        item.ShowText($"EntityID : {entityID}, MomentType : {momentType}, BattleSource : {battleSource}, ConfigID : {configID}");
        ItemList.Add(item); 
    }
}
