using UnityEngine;
using Zenject;

public class BattleRoleComponent : BattleUnitComponent
{
    [Inject] private ILogManager LogManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private IMessageManager MessageManager;
    [Inject] private BattleRenderManager BattleRenderManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;

    private Transform RenderNode;
    private Transform Role;

    private Transform InChooseNode;
    private Transform InActionNode;

    protected override void OnAwake()
    {
        base.OnAwake();
        RenderNode = transform.Find("RenderNode");
        Role = transform.Find("RenderNode/Role");
        InChooseNode = transform.Find("InChooseNode");
        InActionNode = transform.Find("InActionNode");
    }

    protected override void OnStart()
    {
        base.OnStart();
        ShowInChoose(false);
    }

    public override void OnClick()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Entity, Unit.EntityID);
    }

    public override void ShowInChoose(bool isShow)
    {
        InChooseNode.gameObject.SetActive(isShow);
    }

    public override void ShowInAction(bool isShow)
    {
        InActionNode.gameObject.SetActive(isShow);
    }

    public override void SetRenderState()
    {
        if (Unit.IsSelf)
        {
            ShowInAction(Unit.EntityID == BattleLogicStateManager.GetActionSubjectID);
        }
        else
        {
            ShowInAction(false);
        }
    }
}
