using System.Collections.Generic;
using Zenject;

public class HeroSys : SingleArchiveModel
{
    public DictAndList<int, HeroData> HeroMap = new();

    public override void Init()
    {
        base.Init();
        foreach (var kv in HeroMap.GetDictionary())
        {
            DiContainer.Inject(kv.Value);
        }
    }

    public void AddHero(int heroID)
    {
        var hero = GetClass<HeroData>();
        hero.Init(heroID);
        HeroMap.Add(hero.Guid, hero);
    }
}
