using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleMoment
{
    public int HeartMethodID { get; set; }
    private HeartMethodConfig Config { get; set; }
    protected override float GetConfigParamFloat(int index) => Config.ParamEx[index];
    public override int GetConfigParamInt(int index) => Config.ParamEx[index].ToInt();
    protected override int GetSymbol => 300000 + Config.Id;


    public virtual void Init(int heartMethodID, BattleUnit subject)
    {
        HeartMethodID = heartMethodID;
        Config = ConfigManager.GetHeartMethodConfig(HeartMethodID);
        Subject = subject;
    }

    protected override void OnRecycle()
    {
        OnHeartMethodRecycle();   
    }
    protected virtual void OnHeartMethodRecycle(){}
}

