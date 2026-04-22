using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleUnitNodeItem : Item
{
    [Inject] private DiContainer DiContainer { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    [AutoFind] private Transform SelfNode { get; set; }
    [AutoFind] private Transform OtherNode { get; set; }

    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }

    private Dictionary<int, Transform> SelfSlot = new();
    private Dictionary<int, Transform> OtherSlot = new();
    protected override void OnItemCreate()
    {
        for (var index = 1; index <= 5; index++)
        {
            SelfSlot.Add(index, SelfNode.transform.Find(index.ToString()));
            OtherSlot.Add(index, OtherNode.transform.Find(index.ToString()));
        }
    }

    public void CreateBattleRole()
    {
        foreach (var bf in BattleManager.BfList)
        {
            foreach (var (slotIndex, unit) in bf.GetBattleUnitDict())
            {
                CreateBattleRole(bf.Uid == 1, slotIndex, unit);
            }
        }
    }

    private void CreateBattleRole(bool isSelf, int slotIndex, BattleUnit unit)
    {
        var tempSlot = isSelf ? SelfSlot : OtherSlot;
        var slotTran = tempSlot[slotIndex];
        var component = CreateItemByType<BattleUnitItem>(slotTran);
        component.SetUnit(unit);
    }

    public void RefreshUnitRender(bool refreshSelfBf, bool refreshOtherBf)
    {
        /*foreach (var (entityID, unitComponent) in BattleRenderManager.GetUnitDict())
        {
            if (unitComponent.IsSelf && refreshSelfBf)
                unitComponent.SetRenderState();
            
            if (!unitComponent.IsSelf && refreshOtherBf)
                unitComponent.SetRenderState();
        }   */
    }
}
