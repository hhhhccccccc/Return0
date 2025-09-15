using System.Collections.Generic;
using System.Linq;

public class BattleBuffShield : BattleBuffBase
{
    private List<float> Shields = new List<float>();

    public override void AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        if (ParamList.Count > 0)
        {
            Shields.Add(ParamList[0]);
        }
    }

    public override float GetShield() => Shields.Sum();
    
    public override float ReduceShield(ref float allDamage)
    {
        float reduceShieldValue = 0;
        
        while (Shields.Count > 0 && allDamage > 0)
        {
            var shield = Shields[0];
            
            if (shield >= allDamage)
            {
                reduceShieldValue += allDamage;
                Shields[0] -= allDamage;
                allDamage = 0;
            }
            else
            {
                reduceShieldValue += shield;
                allDamage -= shield;
                Shields[0] = 0;
            }

            if (Shields[0] <= 0)
            {
                Shields.RemoveAt(0);
                ReduceLayerCount(1);
            }
        }

        return reduceShieldValue;
    }

    public override void Recycle()
    {
        base.Recycle();
        Shields.Clear();
    }
}
