
public class BattleHeartMethod10089 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Register<UnitDieEventModel>(OnUnitDie);
    }

    private void OnUnitDie(UnitDieEventModel model)
    {
        //避免递归
        if (model.DieID != Subject.EntityID)
        {
            var aliveList = Subject.Bf.GetAliveUnit();
            if (aliveList.Count == 1 && aliveList[0].EntityID == Subject.EntityID)
            {
                DoSetBreak(Subject, true);
            }
        }
    }
}