using System.Collections.Generic;

public class PlayerData : IModel
{
    public int Uid;


    /// <summary>
    /// entityID, character
    /// </summary>
    public List<Character> Characters = new();

}
