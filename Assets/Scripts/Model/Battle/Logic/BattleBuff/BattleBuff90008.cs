using cfg;

public class BattleBuff90008 : BattleBuffBase
{
    //todo buff层数不会减少
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject != null)
        {
            var buffID = GameConst.Battle.BuffNiSha;
            var buff = Subject.GetBuff(buffID);
            if (buff != null)
            {
                buff.SetIgnoreReduceLayer(1);
            }
        }
    }

    protected override void OnBuffRemove()
    {
        if (Subject != null)
        {
            var buffID = GameConst.Battle.BuffNiSha;
            var buff = Subject.GetBuff(buffID);
            if (buff != null)
            {
                buff.SetIgnoreReduceLayer(-1);
            }
        }
        base.OnBuffRemove();
    }
}
