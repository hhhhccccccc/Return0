using System.Collections.Generic;
using System.Linq;

public class BattleBuffArmor : BattleBuffBase
{
    private List<float> Armors = new List<float>();

    public override void AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        if (ParamList.Count > 0)
        {
            Armors.Add(ParamList[0]);
        }
    }

    public override float GetArmor() => Armors.Sum();
    
    public override float ReduceArmor(ref float allDamage)
    {
        float reduceShieldValue = 0;
        
        while (Armors.Count > 0 && allDamage > 0)
        {
            var shield = Armors[0];
            
            if (shield >= allDamage)
            {
                reduceShieldValue += allDamage;
                Armors[0] -= allDamage;
                allDamage = 0;
            }
            else
            {
                reduceShieldValue += shield;
                allDamage -= shield;
                Armors[0] = 0;
            }

            if (Armors[0] <= 0)
            {
                Armors.RemoveAt(0);
                ReduceLayerCount(1);
            }
        }

        return reduceShieldValue;
    }

    public override void Recycle()
    {
        base.Recycle();
        Armors.Clear();
    }
}
