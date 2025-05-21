using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleField : IModel
{
    private PlayerData Data;

    private List<BattleRole> Roles = new();

    private bool IsSelf;

    [Inject] 
    private IMessageManager MessageManager;
    
    [Inject]
    private IPoolManager PoolManager;
    
    public void Init(PlayerData data, bool isSelf)
    {
        Data = data;
        /*foreach (var character in Data.Characters)
        {
            var roleInfo = PoolManager.GetClass<BattleRole>();
            roleInfo.Init(this, character);
            Roles.Add(roleInfo);
        }*/
        
        //var model = PoolManager.GetClass<BattleFieldLogicReadyEventModel>();
        //model.IsSelf = isSelf;
        //model.Roles = Roles;
        //MessageManager.Dispatch(model);
        //PoolManager.RecycleClass(model);

        var obj = PoolManager.GetGameObject("Assets/Prefab/Role.prefab", o =>
        {
            o.transform.position = new Vector3(1, 2, 3);
        });
    }
}