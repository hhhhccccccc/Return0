using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public partial class UIBattleUseSkillPanel
{
    [Inject] private BattleRenderManager BattleRenderManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    private List<BattleKeyType> m_inputKeyList = new();
    private List<UIBattleHeadItem> SelfTopHeadList = new();
    private List<UIBattleHeadItem> OtherTopHeadList = new();

    private UIBattleUnitInfoItem LeftInfoItem { get; set; }
    private UIBattleUnitInfoItem RightInfoItem { get; set; }

    private List<UIBattleKeyIconItem> m_keyIconList = new();
    private List<UIBattleUseSkillItem> m_useSKillItemList = new();
    private BattleSkillData CurrUseSkillData { get; set; }
    protected override void OnPanelCreate()
    {
        LeftInfoItem = CreateItem<UIBattleUnitInfoItem>(UIBattleUnitInfoItem1);
        RightInfoItem = CreateItem<UIBattleUnitInfoItem>(UIBattleUnitInfoItem2);
        var selfUnits = BattleRenderManager.SelfBf.GetBattleUnitDict().Values.ToList();
        CreateItems(SelfTopHeadList, selfUnits.Count, TfLeftHeadNode);
        for (int i = 0; i < SelfTopHeadList.Count; i++)
        {
            SelfTopHeadList[i].Init(selfUnits[i]);
            SelfTopHeadList[i].BindEvent(o =>
            {
                SetSelectID(o.Unit.EntityID, false);
            });
        }
        
        var otherUnits = BattleRenderManager.OtherBf.GetBattleUnitDict().Values.ToList();
        CreateItems(OtherTopHeadList, otherUnits.Count, TfRightHeadNode);
        for (int i = 0; i < OtherTopHeadList.Count; i++)
        {
            OtherTopHeadList[i].Init(otherUnits[i]);
            OtherTopHeadList[i].BindEvent(o =>
            {
                SetSelectID(o.Unit.EntityID, false);
            });
        }
    }

    //如果本身没有 点击就是切换 当前有行为 点击就是切换 没行为（没技能 点击就是切换 有技能 判断释放技能）
    public void SetSelectID(int selectID, bool isFirst)
    {
        if (isFirst)
        {
            ChangeSelectUnit(selectID);
        }
        else
        {
            var currBehaviour = BattleLogicBehaviourManager.GetBattleBehaviour(BattleRenderManager.BattleViewSelectData.SelectID);
            if (currBehaviour != null)
            {
                ChangeSelectUnit(selectID);
            }
            else
            {
                if (CurrUseSkillData != null)
                {
                    var selfID = BattleRenderManager.BattleViewSelectData.SelectID;
                    var otherID = selectID;
                    var skillGuid = CurrUseSkillData.Guid;
                    TryUseSkill(selfID, otherID, skillGuid);
                }
                else
                {
                    ChangeSelectUnit(selectID);
                }
            }
        }
    }
   

    private void ChangeSelectUnit(int selectID)
    {
        BattleRenderManager.BattleViewSelectData.SelectID = selectID;
        var unitItem = BattleRenderManager.GetUnit(selectID);
        ViewManager.AdjustCameraForTwoObjects(unitItem.transform);
        RefreshImgKeyCount(selectID);
        var unit = BattleManager.GetUnit(selectID);
        LeftInfoItem.Refresh(unit);
        m_inputKeyList.Clear();
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(selectID);
        if (behaviour != null)
        {
            var costKey = unit.PreUseSkillDataManager.GetSkillPreUseKeyCost(Util.CombSkillGuid(behaviour.SkillID, behaviour.VariantID));
            m_inputKeyList.AddRange(costKey.Select(o => (BattleKeyType)o).ToList());
            var other = BattleManager.GetUnit(behaviour.TargetID);
            if (other != null)
            {
                RightInfoItem.gameObject.SetActive(true);
                RightInfoItem.Refresh(other);
            }
            else
            {
                RightInfoItem.gameObject.SetActive(false);
            }
        }

        InitUnitSkill();
        RefreshSkill();
    }
    
    private void TryUseSkill(int selfID, int otherID, int skillGuid)
    {
        var self = BattleManager.GetUnit(selfID);
        var other = BattleManager.GetUnit(otherID);
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        var config = ConfigManager.GetBattleSkillConfig(s);
        switch (config.ChooseType)
        {
            case 1:
                if (self != other)
                {
                    ShowTip("请选择自己");
                    return;
                }

                break;
            case 2:
                if (self.IsSelf != other.IsSelf)
                {
                    ShowTip("请选择队友");
                    return;
                }

                break;
            case 3:
                if (self.IsSelf == other.IsSelf)
                {
                    ShowTip("请选择敌方");
                    return;
                }

                break;
        }
        
        var checkCost = self.CheckSkillCanDoDesition_Cost(skillGuid, other);
        var checkLogic = self.CheckSkillCanDoDesition_Logic(skillGuid, other);
        if (checkCost && checkLogic)
        {
            BattleLogicBehaviourManager.AddOrSetBattleBehaviour(selfID, otherID, BattleBehaviourType.Skill, s, v);
            Close();
        }
    }
    

    private void InitUnitSkill()
    {
        var unit = BattleManager.GetUnit(BattleRenderManager.BattleViewSelectData.SelectID);
        var skills = unit.TakeSkillDataManager.GetTakeSkillData();
        CreateItems(m_useSKillItemList, skills.Count, TfSkillContent);  
        for (int i = 0; i < m_useSKillItemList.Count; i++)
        {
            m_useSKillItemList[i].Refresh(unit, skills[i]);
        }
    }

    private void RefreshImgKeyCount(int selectID)
    {
        var unit = BattleManager.GetUnit(selectID);
        var upCount = unit.GetKeyCount(BattleKeyType.KeyUp);
        SetSprite(ImgUpCount, $"key_{upCount}");
        var downCount = unit.GetKeyCount(BattleKeyType.KeyDown);
        SetSprite(ImgDownCount, $"key_{downCount}");
        var leftCount = unit.GetKeyCount(BattleKeyType.KeyLeft);
        SetSprite(ImgLeftCount, $"key_{leftCount}");
        var rightCount = unit.GetKeyCount(BattleKeyType.KeyRight);
        SetSprite(ImgRightCount, $"key_{rightCount}");
    }

    private void RefreshSkill()
    {
        CreateItems(m_keyIconList, m_inputKeyList.Count, TfKeyContent);
        for (int i = 0; i < m_keyIconList.Count; i++)
        {
            m_keyIconList[i].Refresh(m_inputKeyList[i]);
        }

        var unit = BattleManager.GetUnit(BattleRenderManager.BattleViewSelectData.SelectID);
        foreach (var skillItem in m_useSKillItemList)
        {
            var data = skillItem.CheckSameKey(m_inputKeyList);
            if (data != null)
            {
                GoSkillCurr.gameObject.SetActive(true);
                var config = ConfigManager.GetBattleSkillConfig(data.SkillID);
                SetSprite(ImgSkillIconCurr, config.Icon);
                TxtSkillNameCurr.SetText(config.Name);
                TxtSkillWelly.SetText($"威力{(unit.PreUseSkillDataManager.GetSkillPreUseWellyRateBase(data.Guid) * 100.0f).ToInt()}");
                CurrUseSkillData = data;
                return;
            }
        }

        CurrUseSkillData = null;
        GoSkillCurr.gameObject.SetActive(false);
    }

    public override void Esc()
    {
        if (m_inputKeyList.Count > 0)
        {
            m_inputKeyList.RemoveAt(m_inputKeyList.Count - 1);
            RefreshSkill();
        }
        else
        {
            Close();
        }
    }

    protected override void OnUpdate(float dt)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryAddInputKey(BattleKeyType.KeyUp);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryAddInputKey(BattleKeyType.KeyDown);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryAddInputKey(BattleKeyType.KeyLeft);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryAddInputKey(BattleKeyType.KeyRight);
        }
    }

    private void TryAddInputKey(BattleKeyType keyType)
    {
        if (m_inputKeyList.Count < 4)
        {
            var selectUnit = BattleManager.GetUnit(BattleRenderManager.BattleViewSelectData.SelectID);
            if (selectUnit == null)
            {
                return;
            }

            if (!selectUnit.IsSelf)
            {
                return;
            }
            
            var unit = BattleManager.GetUnit(BattleRenderManager.BattleViewSelectData.SelectID);
            var has = unit.GetKeyCount(keyType);
            var inputCount = m_inputKeyList.Count(o => o == keyType);
            if (has < inputCount + 1)
            {
                return;
            }
            
            m_inputKeyList.Add(keyType);
            RefreshSkill();
        }
    }
    
    protected override void OnClose()
    {
        BattleRenderManager.BattleViewSelectData.SelectID = 0;
        ViewManager.AdjustCameraForTwoObjects();
    }

    protected override void OnPanelDestroy()
    {
        m_inputKeyList.Clear();
    }
}
