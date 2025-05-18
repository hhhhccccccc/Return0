using System;
using UnityEngine;
using Random = System.Random;

public class BattleRenderManager : MonoBehaviour
{
    public Transform SelfBf;
    public Transform OtherBf;

    private void Awake()
    {
        SelfBf = transform.Find("SelfBf");
        OtherBf = transform.Find("OtherBf");
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        //GameManager.MessageManager.Register<BattleFieldLogicReadyEventModel>(OnBattleFieldLogicReady);
    }

    private void OnBattleFieldLogicReady(BattleFieldLogicReadyEventModel model)
    {
        var tarTran = model.IsSelf ? SelfBf : OtherBf;
        /*foreach (var roleInfo in model.Roles)
        {
            var obj = 
            StartCoroutine(GameManager.ResourceManager.LoadPrefabAsync("Assets/Prefab/Role.prefab", (go) =>
            {
                go.transform.SetParent(tarTran);
                go.transform.localPosition = new Vector3(0, 3, 2);
            }));
        }*/
    }
}
