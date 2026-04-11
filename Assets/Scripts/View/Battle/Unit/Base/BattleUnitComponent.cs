
using System.Globalization;
using cfg;
using TMPro;
using UnityEngine;
using Zenject;

public class BattleUnitComponent : View
{
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private IMessageManager MessageManager { get; set; }
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    #region 代码
    [AutoFind] private Transform TfRenderNode  { get; set; }
    [AutoFind] private Animator AniHeroKnight  { get; set; }
    [AutoFind] private SpriteRenderer SpInChooseNode  { get; set; }
    [AutoFind] private SpriteRenderer SpInActionNode  { get; set; }
    [AutoFind] private Transform TfBeAttackPosition  { get; set; }
    [AutoFind] private GameObject GoHpProgressBar  { get; set; }
    [AutoFind] private GameObject GoXuanQiProgressBar  { get; set; }
    [AutoFind] private GameObject GoGangQiProgressBar  { get; set; }
    [AutoFind] private TextMeshPro TxtDamageHp  { get; set; }
    [AutoFind] private TextMeshPro TxtActionTimes  { get; set; }
    [AutoFind] private TextMeshPro TxtAddBeCounterBuff  { get; set; }
    #endregion

    private ProgressBar HpProgressBar { get; set; }
    private ProgressBar XuanQiProgressBar { get; set; }
    private ProgressBar GangQiProgressBar{ get; set; }
    
    public BattleUnit Unit { get; private set; }
    public int EntityID => Unit.EntityID;
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
    
    protected override void OnCreate()
    {
        base.OnCreate();
        HpProgressBar = CreateUIComponent<ProgressBar>(GoHpProgressBar);
        XuanQiProgressBar = CreateUIComponent<ProgressBar>(GoXuanQiProgressBar);
        GangQiProgressBar = CreateUIComponent<ProgressBar>(GoGangQiProgressBar);
        
        TxtDamageHp.gameObject.SetActive(false);
        TxtActionTimes.gameObject.SetActive(false);
        TxtAddBeCounterBuff.gameObject.SetActive(false);
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
            TfBeAttackPosition.transform.localPosition = new Vector3(0.5f, 0, 0);
        }
        else
        {
            TfBeAttackPosition.transform.localPosition = new Vector3(-0.5f, 0, 0);
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
        SpInChooseNode.gameObject.SetActive(isShow);
    }

    private void ShowInAction(bool isShow)
    {
        SpInActionNode.gameObject.SetActive(isShow);
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

    public Vector3 GetBeAttackPosition() => TfBeAttackPosition.position;


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
        AniHeroKnight.Play(aniName);
    }

    public bool ShowSkillKeyRender(float time)
    {
        var skill = Unit.GetSkill();
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
        TxtDamageHp.gameObject.SetActive(true);
        HpChanged(-damage, delayClose);
        TxtDamageHp.SetText($"-{damage.ToString(CultureInfo.InvariantCulture)}");
        BattleRenderManager.DelayCall(() =>
        {
            TxtDamageHp.gameObject.SetActive(false);
        }, delayClose);
    }

    public void ShowReduceRoundTimes(int times, float delayClose)
    {
        TxtActionTimes.gameObject.SetActive(true);
        TxtActionTimes.SetText($"行动次数-{times}");
        BattleRenderManager.DelayCall(() =>
        {
            TxtActionTimes.gameObject.SetActive(false);
        }, delayClose);
    }
    
    public void ShowAddBeCounterBuff(float delayClose)
    {
        TxtAddBeCounterBuff.gameObject.SetActive(true);
        TxtAddBeCounterBuff.SetText($"添加破招buff");
        BattleRenderManager.DelayCall(() =>
        {
            TxtAddBeCounterBuff.gameObject.SetActive(false);
        }, delayClose);
    }
    
    #endregion
   
}
