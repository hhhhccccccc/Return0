using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定时器管理类 - 支持延时、循环、帧计时、条件等待
/// </summary>
public class TimerManager : ManagerBase, ITimeManager
{
    // 定时器列表
    private List<Timer> timers = new List<Timer>();
    private List<Timer> toAdd = new List<Timer>();
    private List<Timer> toRemove = new List<Timer>();
    
    private bool isUpdating = false;
    
    #region 基础定时器
    
    /// <summary>
    /// 延时执行一次
    /// </summary>
    public Timer Delay(float delay, Action callback)
    {
        if (delay <= 0)
        {
            callback.Invoke();
            return null;
        }
        
        Timer timer = new Timer(delay, callback, false, false);
        AddTimer(timer);
        return timer;
    }
    
    /// <summary>
    /// 循环执行
    /// </summary>
    public Timer Loop(float interval, Action callback)
    {
        Timer timer = new Timer(interval, callback, true, false);
        AddTimer(timer);
        return timer;
    }
    
    /// <summary>
    /// 循环执行指定次数
    /// </summary>
    public Timer Loop(float interval, int loopCount, Action callback)
    {
        Timer timer = new Timer(interval, callback, true, false, loopCount);
        AddTimer(timer);
        return timer;
    }
    
    #endregion
    
    #region 条件定时器
    
    /// <summary>
    /// 等待条件满足后执行一次
    /// </summary>
    public Timer WaitUntil(Func<bool> condition, Action callback)
    {
        Timer timer = new Timer(condition, callback);
        AddTimer(timer);
        return timer;
    }
    
    /// <summary>
    /// 带超时的条件等待
    /// </summary>
    public Timer WaitUntil(Func<bool> condition, float timeout, Action callback, Action onTimeout = null)
    {
        Timer timer = new Timer(condition, timeout, callback, onTimeout);
        AddTimer(timer);
        return timer;
    }
    
    #endregion
    
    #region 协程风格定时器
    
    /// <summary>
    /// 使用协程风格的定时器（推荐在 MonoBehaviour 中使用）
    /// </summary>
    public static System.Collections.IEnumerator WaitForSeconds(float seconds)
    {
        float startTime = Time.time;
        while (Time.time - startTime < seconds)
        {
            yield return null;
        }
    }
    
    /// <summary>
    /// 等待帧数
    /// </summary>
    public static System.Collections.IEnumerator WaitForFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }
    }
    
    /// <summary>
    /// 等待条件
    /// </summary>
    public static System.Collections.IEnumerator WaitUntil(Func<bool> condition)
    {
        while (!condition())
        {
            yield return null;
        }
    }
    
    #endregion
    
    private void AddTimer(Timer timer)
    {
        if (isUpdating)
            toAdd.Add(timer);
        else
            timers.Add(timer);
    }
    
    private void RemoveTimer(Timer timer)
    {
        if (isUpdating)
            toRemove.Add(timer);
        else
            timers.Remove(timer);
    }
    
    
    /// <summary>
    /// 停止所有定时器
    /// </summary>
    public void StopAll()
    {
        foreach (var timer in timers)
        {
            timer.Stop();
        }
        timers.Clear();
        toAdd.Clear();
        toRemove.Clear();
    }
    
    /// <summary>
    /// 暂停所有定时器
    /// </summary>
    public void PauseAll()
    {
        foreach (var timer in timers)
        {
            timer.Pause();
        }
    }
    
    /// <summary>
    /// 恢复所有定时器
    /// </summary>
    public void ResumeAll()
    {
        foreach (var timer in timers)
        {
            timer.Resume();
        }
    }

    public void OnUpdate(float dt)
    {
        isUpdating = true;
        
        // 添加新定时器
        if (toAdd.Count > 0)
        {
            timers.AddRange(toAdd);
            toAdd.Clear();
        }
        
        // 更新定时器
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            Timer timer = timers[i];
            if (timer == null || timer.IsCompleted)
            {
                timers.RemoveAt(i);
                continue;
            }
            
            timer.Update(dt);
            
            if (timer.IsCompleted)
            {
                timers.RemoveAt(i);
            }
        }
        
        // 移除待删除的定时器
        if (toRemove.Count > 0)
        {
            foreach (var timer in toRemove)
            {
                timers.Remove(timer);
            }
            toRemove.Clear();
        }
        
        isUpdating = false;
    }
}

