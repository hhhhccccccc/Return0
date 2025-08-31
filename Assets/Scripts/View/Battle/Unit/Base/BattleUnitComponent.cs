
using System.Globalization;
using cfg;
using TMPro;
using UnityEngine;
using Zenject;

public class BattleUnitComponent : View
{
    [Inject] private ILogManager LogManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private IMessageManager MessageManager;
    [Inject] private BattleRenderManager BattleRenderManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;

    [AutoFind] private Transform RenderNode { get; set; }
    [AutoFind] private Transform InChooseNode{ get; set; }
    [AutoFind] private Transform InActionNode{ get; set; }
    [AutoFind] private Transform BeAttackPosition{ get; set; }
    [AutoFind] private ProgressBar HpProgressBar { get; set; }
    [AutoFind] private ProgressBar XuanQiProgressBar { get; set; }
    [AutoFind] private ProgressBar GangQiProgressBar { get; set; }
    [AutoFind] private Animator HeroKnight { get; set; }
    [AutoFind] private TextMeshPro DamageHp { get; set; }
    [AutoFind] private TextMeshPro RoundTimes { get; set; }
    [AutoFind] private TextMeshPro AddBeCounterBuff { get; set; }
    public BattleUnit Unit { get; set; }
    public bool IsSelf => Unit.IsSelf;

    private Vector3 NodePosition;

    #region 属性

    private float MaxHp;
    private float Hp;
    private float MaxGangQi;
    private float GangQi;
    private float MaxXuanQi;
    private float XuanQi;
    
    #endregion
    
    protected override void OnAwake()
    {
        base.OnAwake();
        DamageHp.gameObject.SetActive(false);
        RoundTimes.gameObject.SetActive(false);
        AddBeCounterBuff.gameObject.SetActive(false);
    }
    protected override void OnStart()
    {
        base.OnStart();
        ShowInChoose(false);
    }

    public void SetUnit(BattleUnit unit)
    {
        InitRender();
        Unit = unit;
        BattleRenderManager.ResetUnitToDict(this);
        if (unit.IsSelf)
        {
            BeAttackPosition.transform.localPosition = new Vector3(0.5f, 0, 0);
        }
        else
        {
            BeAttackPosition.transform.localPosition = new Vector3(-0.5f, 0, 0);
        }

        MaxHp = unit.GetProperty(BattlePropertyType.MaxHp);
        Hp = unit.GetProperty(BattlePropertyType.Hp);
        MaxGangQi = unit.GetProperty(BattlePropertyType.MaxGangQi);
        GangQi = unit.GetProperty(BattlePropertyType.GangQi);
        MaxXuanQi = unit.GetProperty(BattlePropertyType.MaxXuanQi);
        XuanQi = unit.GetProperty(BattlePropertyType.XuanQi);
        
        HpProgressBar.Init(unit.GetProperty(BattlePropertyType.Hp), unit.GetProperty(BattlePropertyType.MaxHp));
        GangQiProgressBar.Init(unit.GetProperty(BattlePropertyType.GangQi), unit.GetProperty(BattlePropertyType.MaxGangQi));
        XuanQiProgressBar.Init(unit.GetProperty(BattlePropertyType.XuanQi), unit.GetProperty(BattlePropertyType.MaxXuanQi));
    }

    private void InitRender()
    {
        NodePosition = transform.position;
        InitTween();
    }

    public void OnClick()
    {
        BattleRenderManager.DispatchClickEventModel(BattleClickType.Entity, Unit.EntityID);
    }

    private void ShowInChoose(bool isShow)
    {
        InChooseNode.gameObject.SetActive(isShow);
    }

    private void ShowInAction(bool isShow)
    {
        InActionNode.gameObject.SetActive(isShow);
    }

    public virtual void SetRenderState()
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

    public Vector3 GetBeAttackPosition() => BeAttackPosition.position;


    #region 表现
    public void HpChanged(float changeValue, float time = 0.3f)
    {
        Hp += changeValue;
        HpProgressBar.SetValue(Hp, MaxHp, true, time);
    }

    public void GangQiChanged(float changeValue, float time = 0.3f)
    {
        GangQi += changeValue;
        GangQiProgressBar.SetValue(GangQi, MaxGangQi, true, time);
    }

    public void XuanQiChanged(float changeValue, float time = 0.3f)
    {
        XuanQi += changeValue;
        XuanQiProgressBar.SetValue(XuanQi, MaxXuanQi, true, time);
    }

    public void PlayAnim(string aniName, bool loop = false)
    {
        HeroKnight.Play(aniName);
    }

    public bool ShowSkillKeyRender(float time)
    {
        var skill = Unit.GetSkillBase;
        if (skill != null)
        {
            var model = PoolManager.GetClass<ShowSkillKeyRenderEventModel>();
            model.SKillCost = skill.GetKeyCostList;
            model.Time = time;
            MessageManager.DispatchMsg(model);
            PoolManager.RecycleClass(model);
            return true;
        }

        return false;
    }

    private UnitMoveTween MoveTween;
    
    private void InitTween()
    {
        MoveTween = new UnitMoveTween(this.gameObject);
    }
    
    public void MoveToTarget(BattleUnitComponent target, float time)
    {
        MoveTween.Play(target.GetBeAttackPosition(), time);
    }

    public void MoveToBack(float time)
    {
        MoveTween.Play(NodePosition, time);
    }

    public void ShowDamage(float damage, float delayClose)
    {
        DamageHp.gameObject.SetActive(true);
        HpChanged(-damage, delayClose);
        DamageHp.SetText($"-{damage.ToString(CultureInfo.InvariantCulture)}");
        BattleRenderManager.DelayCall(() =>
        {
            DamageHp.gameObject.SetActive(false);
        }, delayClose);
    }

    public void ShowReduceRoundTimes(int times, float delayClose)
    {
        RoundTimes.gameObject.SetActive(true);
        RoundTimes.SetText($"RoundTimes-{times}");
        BattleRenderManager.DelayCall(() =>
        {
            RoundTimes.gameObject.SetActive(false);
        }, delayClose);
    }
    
    public void ShowAddBeCounterBuff(float delayClose)
    {
        AddBeCounterBuff.gameObject.SetActive(true);
        AddBeCounterBuff.SetText($"ShowAddBeCounterBuff");
        BattleRenderManager.DelayCall(() =>
        {
            AddBeCounterBuff.gameObject.SetActive(false);
        }, delayClose);
    }
    
    #endregion
   
}
