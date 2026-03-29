using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1024 : BattleSkillBase
{
    // Skill: 焚羽凌霄 (1024)
    // XuanQiCost: 80, NeedKey: [1, 4, 2, 3]
    // Moments: ActionWheelStartMoment [1024001], ReleaseSkillActionMoment [1024002], AfterActionMoment [1024003]
    
    // Moment: 1024001 → 无条件 → 给自己添加多个增益Buff + 给目标添加多个减益Buff
    public override void SelfActionWheelStart()
    {
        // 给自己添加: 迅速10041x10, 刚聚10051x5, 玄聚10061x5
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffXuanJu, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffXuanPing, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffGangJu, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffGangPing, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
    }

    // Moment: 1024002 → 无条件 → 给自己和目标添加多个Buff
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 给自己添加: 力增10071x5, 技增10081x5, 武增10091x5, 术增10101x5, 巧增10111x5
        DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffShuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffWuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1024003 → 无条件 → 设置刚气玄气各50 + 获得5个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 105001 - SetProperty → 设置自己刚气为50
        // 效果: 106001 - SetProperty → 设置自己玄气为50
        // 效果: 400005 - AddRandomKey → 我获得5个键
        DoSetProperty(Subject, BattlePropertyType.GangQi, 50, BattleSource.Skill);
        DoSetProperty(Subject, BattlePropertyType.XuanQi, 50, BattleSource.Skill);
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }


}