using System.Collections.Generic;
using System.Linq;
using Zenject;

/// <summary>
/// 存放战斗指令
/// </summary>
public class BattleLogicBehaviourManager : SingleModel
{
    #region Inject注入
    
    [Inject] private ILogManager LogManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private BattleManager BattleManager;
    
    #endregion

    
    
    #region 指令数据

    public DictAndList<int, BattleBehaviour> BattleBehaviourRes = new();
    
    #endregion
    
    
    #region 表现层输入输入

    private List<InputEventModel> InputList = new();

    #endregion
    
    public void BattleStart()
    {
        Register<InputEventModel>(OnGetInput);
    }
    
    public void RoundStart()
    {
        InputList.Clear();   
    }
    
    public void RoundEnd()
    {
        InputList.Clear();   
    }

    public BattleBehaviour AddOrSetBattleBehaviour(int subjectID, int targetID, BattleBehaviourType behaviourType, int skillID)
    {
        var behaviour = BattleBehaviourRes.TryGetValue(subjectID);
        if (behaviour != null)
        {
            behaviour.SubjectID = subjectID;
            behaviour.TargetID = targetID;
            behaviour.BehaviourType = behaviourType;
            behaviour.SkillID = skillID;
        }
        else
        {
            behaviour = PoolManager.GetClass<BattleBehaviour>();
            behaviour.SubjectID = subjectID;
            behaviour.TargetID = targetID;
            behaviour.BehaviourType = behaviourType;
            behaviour.SkillID = skillID;
            BattleBehaviourRes.Add(behaviour.SubjectID, behaviour);
        }
        
        return behaviour;
    }
    
    public BattleBehaviour GetBattleBehaviour(int subjectID)
    {
        return BattleBehaviourRes.TryGetValue(subjectID);
    }
    
    private void OnGetInput(InputEventModel model)
    {
        LogManager.Debug(model.InputType.ToString());
        LogManager.Debug(model.KeyCode.ToString());
    }

    public override void Clear()
    {
        base.Clear();
        InputList.Clear();
    }

    public List<BattleBehaviour> GetOpponentBehaviourByEntityID(int entityID)
    {
        var unit = BattleManager.GetUnit(entityID);
        var isSelf = unit.IsSelf;
        if (isSelf)
        {
            return BattleBehaviourRes.GetListValue()
                .Where(behaviour => !BattleManager.GetUnit(behaviour.SubjectID).IsSelf).ToList();
        }
        else
        {
            return BattleBehaviourRes.GetListValue()
                .Where(behaviour => BattleManager.GetUnit(behaviour.SubjectID).IsSelf).ToList();
        }
    }

    public void ChangeTarget(BattleUnit target1, BattleUnit target2)
    {
        var behaviour = BattleBehaviourRes.TryGetValue(target1.EntityID);
        if (behaviour != null)
        {
            behaviour.TargetID = target2.EntityID;
        }

        var skillBase = target1.GetSkill();
        if (skillBase != null)
        {
            skillBase.SetTarget(target2);
        }
    }
}
