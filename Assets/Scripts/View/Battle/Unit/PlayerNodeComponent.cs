using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerNodeComponent : View
{
    [Inject] private DiContainer DiContainer;
    [Inject] private ILogManager LogManager;
    [Inject] private BattleRenderManager BattleRenderManager;
    [AutoFind] private Transform SelfNode { get; set; }
    [AutoFind] private Transform OtherNode { get; set; }

    [Inject] private BattleManager BattleManager;
    [Inject] private IPoolManager PoolManager;

    private Dictionary<int, Transform> SelfSlot = new();
    private Dictionary<int, Transform> OtherSlot = new();
    protected override void OnAwake()
    {
        base.OnAwake();
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
        PoolManager.GetGameObject("Assets/Prefab/Unit/Battle/BattleRole.prefab", o =>
        {
            o.transform.SetParent(slotTran);
            o.transform.localPosition = Vector3.zero;
            var unitComponent = o.AddComponent<BattleRoleComponent>();
            unitComponent.SetUnit(unit);
        });
    }

    public void RefreshUnitRender(bool refreshSelfBf, bool refreshOtherBf)
    {
        foreach (var (entityID, unitComponent) in BattleRenderManager.GetUnitDict())
        {
            if (unitComponent.IsSelf && refreshSelfBf)
                unitComponent.SetRenderState();
            
            if (!unitComponent.IsSelf && refreshOtherBf)
                unitComponent.SetRenderState();
        }   
    }
}
