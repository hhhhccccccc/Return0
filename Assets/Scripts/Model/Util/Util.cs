
using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;
using Random = System.Random;

public static class Util
{
    private static Random random = new Random();
    
    public static List<BattleKeyType> KeyList = new()
    {
        BattleKeyType.KeyUp,
        BattleKeyType.KeyDown,
        BattleKeyType.KeyLeft,
        BattleKeyType.KeyRight,
    };
    
    
    public static List<BattleKeyType> GetRandomKey(int count)
    {
        var result = new List<BattleKeyType>();
        for (int i = 0; i < count; i++)
        {
            var key = GetRandom<BattleKeyType>(KeyList);
            result.Add(key);
        }

        return result;
    }

    public static T GetRandom<T>(List<T> list)
    {
        var count = list.Count;
        var index = random.Next(0,  count);
        return list[index];
    }

    public static Dictionary<int, int> KeyListToDictionary(List<int> keyList)
    {
        var result = new Dictionary<int, int>();
        foreach (var key in keyList)
        {
            if (!result.TryAdd(key, 1))
            {
                result[key]++;
            }
        }

        return result;
    }
    
    
}
