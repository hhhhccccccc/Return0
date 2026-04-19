using System;
using System.Collections.Generic;

public class IntEx : IModel, IRecycle
{
    private List<Action<int>> m_action = new();
    private int m_value { get; set; }
    public int GetValue() => m_value;
    public void SetValue(int value) => m_value = value;

    public void SetValueWithEvent(int value)
    {
        SetValue(value);
        foreach (var action in m_action)
        {
            action.Invoke(value);
        }
    }

    public void RegisterAction(Action<int> action)
    {
        if (m_action.Contains(action))
        {
            return;
        }
        
        m_action.Add(action);
    }

    public void RemoveAction(Action<int> action)
    {
        if (!m_action.Contains(action))
        {
            return;
        }
        
        m_action.Remove(action);
    }

    public void Recycle()
    {
        m_action.Clear();
        m_value = 0;
    }
}
