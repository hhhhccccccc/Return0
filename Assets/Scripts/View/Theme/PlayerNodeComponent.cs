using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerNodeComponent : View
{
    [Inject] private DiContainer DiContainer;
    [Inject] private ILogManager LogManager;
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
        
        foreach (var bf in BattleManager.BfList)
        {
            var tempSlot = bf.Uid == 1 ? SelfSlot : OtherSlot;
            foreach (var kv in tempSlot)
            {
                var slotIndex = kv.Key;
                var slotTran = kv.Value;
                var roleInfo = bf.GetBattleRole(slotIndex);
                PoolManager.GetGameObject("Assets/Prefab/Battle/Unit/Cube.prefab", o =>
                {
                    o.transform.SetParent(slotTran);
                    o.transform.localPosition = Vector3.zero;
                    var unitComponent = o.AddComponent<BattleUnitComponent>();
                    DiContainer.Inject(unitComponent);
                    unitComponent.SetUnit(roleInfo);
                });
            }
        }
    }
}
