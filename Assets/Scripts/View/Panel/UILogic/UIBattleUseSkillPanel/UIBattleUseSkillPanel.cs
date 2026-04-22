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
    private List<BattleKeyType> m_inputKey = new();
    private List<UIBattleHeadItem> SelfTopHeadList = new();
    private List<UIBattleHeadItem> OtherTopHeadList = new();
    protected override void OnPanelCreate()
    {
        var selfUnits = BattleRenderManager.SelfBf.GetBattleUnitDict().Values.ToList();
        CreateItems(SelfTopHeadList, selfUnits.Count, TfLeftHeadNode);
        for (int i = 0; i < SelfTopHeadList.Count; i++)
        {
            SelfTopHeadList[i].Init(selfUnits[i]);
            SelfTopHeadList[i].BindEvent(o =>
            {
                SetSelectID(o.Unit.EntityID);
            });
        }
        
        var otherUnits = BattleRenderManager.OtherBf.GetBattleUnitDict().Values.ToList();
        CreateItems(OtherTopHeadList, otherUnits.Count, TfRightHeadNode);
        for (int i = 0; i < OtherTopHeadList.Count; i++)
        {
            OtherTopHeadList[i].Init(otherUnits[i]);
            OtherTopHeadList[i].BindEvent(o =>
            {
                SetSelectID(o.Unit.EntityID);
            });
        }
    }

    public void SetSelectID(int selectID)
    {
        BattleRenderManager.BattleViewSelectData.SelectID = selectID;
        var unitItem = BattleRenderManager.GetUnit(selectID);
        ViewManager.AdjustCameraForTwoObjects(unitItem.transform);
        
        m_inputKey.Clear();
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(selectID);
        if (behaviour != null)
        {
            var unit = BattleManager.GetUnit(selectID);
            var costKey = unit.PreUseSkillDataManager.GetSkillPreUseKeyCost(Util.CombSkillGuid(behaviour.SkillID, behaviour.VariantID));
            m_inputKey.AddRange(costKey.Select(o => (BattleKeyType)o).ToList());
            RefreshSkill();
        }
    }

    private void RefreshSkill()
    {
        
    }

    protected override void RegisterEvent()
    {
        Register<KeyCodeClickEventModel>(OnKeyCodeClickEvent);
    }

    private void OnKeyCodeClickEvent(KeyCodeClickEventModel model)
    {
        if (model.KeyCode == KeyCode.UpArrow || model.KeyCode == KeyCode.DownArrow || model.KeyCode == KeyCode.LeftArrow || model.KeyCode == KeyCode.RightArrow)
        {
            if (m_inputKey.Count < 4)
            {
                if (model.KeyCode == KeyCode.UpArrow)
                {
                    m_inputKey.Add(BattleKeyType.KeyUp);
                }   
                
                if (model.KeyCode == KeyCode.DownArrow)
                {
                    m_inputKey.Add(BattleKeyType.KeyDown);
                }   
                
                if (model.KeyCode == KeyCode.LeftArrow)
                {
                    m_inputKey.Add(BattleKeyType.KeyLeft);
                }   
                
                if (model.KeyCode == KeyCode.RightArrow)
                {
                    m_inputKey.Add(BattleKeyType.KeyRight);
                }   
                
                RefreshSkill();
            }
        }
    }

    protected override void OnClose()
    {
        BattleRenderManager.BattleViewSelectData.SelectID = 0;
        ViewManager.AdjustCameraForTwoObjects();
    }

    protected override void OnPanelDestroy()
    {
        m_inputKey.Clear();
    }
}
