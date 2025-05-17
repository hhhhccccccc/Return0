using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EditorTest", menuName = "Test/EditorTest")]
public class EditorTest : ScriptableObject
{
    [LabelText("啊啊啊")]
    public string testString = "1234567890";
}
