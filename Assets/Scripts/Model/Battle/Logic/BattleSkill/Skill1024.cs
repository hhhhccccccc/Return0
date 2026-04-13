using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1024 : BattleSkillBase
{
    //获得10层缓速和10层迅速和5层玄聚和5层玄屏和5层刚聚和5层刚屏
    protected override void OnSelfActionWheelStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 10, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffXuanJu, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffXuanPing, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffGangJu, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
        DoAddBuff(Subject, GameConst.Battle.BuffGangPing, Subject, 5, null, BattleMomentType.SelfActionWheelStart);
    }

    //获得5层术衰状态和5层术增状态和5层技增状态和5层技衰状态和5层力衰状态和5层力增状态和5层武增状态和5层武衰状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffShuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffJiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffWuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    //将玄炁和刚炁的持有量变为50，获得5个随机的键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoSetProperty(Subject, BattlePropertyType.GangQi, 50, BattleSource.Skill);
        DoSetProperty(Subject, BattlePropertyType.XuanQi, 50, BattleSource.Skill);
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}