using cfg;

//目标下回合刚炁自然恢复不会低于35，自身下回合玄炁自然恢复不会低于35 target.AddRecoverNaturalData(type, value);
public class BattleBuff74073 : BattleBuffBase
{
    private int DataID;
    protected override void OnStart()
    {
        base.OnStart();
        if (DataID == 0)
        {
            var type = ParamList[0].ToInt();
            var value = ParamList[1];
            var data = Subject.AddMinRecoverNaturalData(type, value);
            if (data != null)
            {
                DataID = data.Guid;
            }
        }
    }

    protected override void OnBuffRemove()
    {
        if (DataID != 0)
        {
            Subject.RemoveMinRecoverNaturalData(DataID);
            DataID = 0;
        }
        base.OnBuffRemove();
    }

    public override void Recycle()
    {
        if (DataID != 0)
        {
            Subject.RemoveMinRecoverNaturalData(DataID);
            DataID = 0;
        }
        base.Recycle();
    }
}
