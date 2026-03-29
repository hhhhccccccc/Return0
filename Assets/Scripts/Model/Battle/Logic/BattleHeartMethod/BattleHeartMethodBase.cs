using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleMoment
{
    public int HeartMethodID { get; set; }
    public HeartMethodConfig Config { get; set; }
    protected float GetParamFloat(int index) => Config.ParamEx[index];
    public int GetParamInt(int index) => Config.ParamEx[index].ToInt();
    protected int GetSymbol => 100000 + Config.Id;
    public virtual void Init(int heartMethodID, BattleUnit subject)
    {
        HeartMethodID = heartMethodID;
        Config = ConfigManager.GetHeartMethodConfig(HeartMethodID);
        Subject = subject;
    }

    private bool CanTrigger()
    {
        return true;
    }
    
    protected BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType, params float[] values)
    {
        var viewModel = base.AllocViewModel(entityID, viewType);
        if (values.Length > 0)
        {
            foreach (var value in values)
            {
                viewModel.FloatParam.Add(value);
            }
        }

        return viewModel;
    }
    
    protected void EnqueueViewModel(int entityID, MomentViewType viewType, params float[] values)
    {
        EnqueueViewModel(AllocViewModel(entityID, viewType, values)); 
    }

    protected override void OnRecycle()
    {
        OnHeartMethodRecycle();   
    }
    protected virtual void OnHeartMethodRecycle(){}
}

