using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class DebugPlayer
{
    [LabelText("角色")]
    public List<DebugCharacter> Characters;
}
