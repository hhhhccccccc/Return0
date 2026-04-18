using System;
using UnityEngine;

public interface ITimeManager : IManager, IUpdate
{
    public Timer Delay(float delay, Action callback);
    public Timer Loop(float interval, Action callback);
    public Timer Loop(float interval, int loopCount, Action callback);
}

/// <summary>
/// 定时器类
/// </summary>
public class Timer
{
    // 基础属性
    private float duration;      // 持续时间（秒）或帧数
    private Action callback;
    private bool isLoop;
    private bool isFrameTimer;   // 是否是帧定时器
    private int loopCount;       // 剩余循环次数（-1表示无限循环）
    private int originalLoopCount;
    
    // 时间/帧计数
    private float elapsedTime;
    private int elapsedFrames;
    
    // 条件定时器专用
    private Func<bool> condition;
    private bool isConditionTimer;
    private float timeout;
    private Action onTimeout;
    private bool hasTimedOut;
    
    // 状态
    private bool isRunning;
    private bool isCompleted;
    private bool isPaused;
    
    public bool IsCompleted => isCompleted;
    public bool IsRunning => isRunning;
    public float Progress => duration > 0 ? Mathf.Clamp01(elapsedTime / duration) : 0f;
    
    // 事件
    public event Action OnComplete;
    public event Action<int> OnLoop; // 参数：剩余循环次数
    
    #region 构造函数
    
    /// <summary>时间定时器</summary>
    public Timer(float duration, Action callback, bool isLoop, bool isFrameTimer, int loopCount = -1)
    {
        this.duration = duration;
        this.callback = callback;
        this.isLoop = isLoop;
        this.isFrameTimer = isFrameTimer;
        this.loopCount = loopCount;
        this.originalLoopCount = loopCount;
        this.isRunning = true;
        this.isCompleted = false;
        this.isPaused = false;
    }
    
    /// <summary>帧定时器</summary>
    public Timer(int frameCount, Action callback, bool isLoop, bool isFrameTimer)
    {
        this.duration = frameCount;
        this.callback = callback;
        this.isLoop = isLoop;
        this.isFrameTimer = isFrameTimer;
        this.loopCount = isLoop ? -1 : 1;
        this.originalLoopCount = this.loopCount;
        this.isRunning = true;
        this.isCompleted = false;
        this.isPaused = false;
    }
    
    /// <summary>条件定时器</summary>
    public Timer(Func<bool> condition, Action callback)
    {
        this.condition = condition;
        this.callback = callback;
        this.isConditionTimer = true;
        this.isRunning = true;
        this.isCompleted = false;
        this.timeout = -1;
    }
    
    /// <summary>带超时的条件定时器</summary>
    public Timer(Func<bool> condition, float timeout, Action callback, Action onTimeout)
    {
        this.condition = condition;
        this.callback = callback;
        this.onTimeout = onTimeout;
        this.isConditionTimer = true;
        this.isRunning = true;
        this.isCompleted = false;
        this.timeout = timeout;
        this.hasTimedOut = false;
    }
    
    #endregion
    
    public void Update(float dt)
    {
        if (!isRunning || isPaused || isCompleted) return;
        
        if (isConditionTimer)
        {
            UpdateConditionTimer(dt);
        }
        else if (isFrameTimer)
        {
            UpdateFrameTimer();
        }
        else
        {
            UpdateTimeTimer(dt);
        }
    }
    
    private void UpdateTimeTimer(float dt)
    {
        elapsedTime += dt;
        
        if (elapsedTime >= duration)
        {
            ExecuteCallback();
            
            if (isLoop && (loopCount == -1 || loopCount > 1))
            {
                // 继续循环
                elapsedTime = 0;
                if (loopCount > 0)
                {
                    loopCount--;
                    OnLoop?.Invoke(loopCount);
                }
            }
            else
            {
                // 完成
                isCompleted = true;
                isRunning = false;
                OnComplete?.Invoke();
            }
        }
    }
    
    private void UpdateFrameTimer()
    {
        elapsedFrames++;
        
        if (elapsedFrames >= duration)
        {
            ExecuteCallback();
            
            if (isLoop && (loopCount == -1 || loopCount > 1))
            {
                elapsedFrames = 0;
                if (loopCount > 0)
                {
                    loopCount--;
                    OnLoop?.Invoke(loopCount);
                }
            }
            else
            {
                isCompleted = true;
                isRunning = false;
                OnComplete?.Invoke();
            }
        }
    }
    
    private void UpdateConditionTimer(float dt)
    {
        if (timeout > 0)
        {
            elapsedTime += dt;
            if (elapsedTime >= timeout && !hasTimedOut)
            {
                hasTimedOut = true;
                isCompleted = true;
                isRunning = false;
                onTimeout?.Invoke();
                return;
            }
        }
        
        if (condition())
        {
            ExecuteCallback();
            isCompleted = true;
            isRunning = false;
        }
    }
    
    private void ExecuteCallback()
    {
        try
        {
            callback?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"定时器回调执行异常: {e}");
        }
    }
    
    /// <summary>停止定时器</summary>
    public void Stop()
    {
        isRunning = false;
        isCompleted = true;
    }
    
    /// <summary>暂停定时器</summary>
    public void Pause()
    {
        isPaused = true;
    }
    
    /// <summary>恢复定时器</summary>
    public void Resume()
    {
        isPaused = false;
    }
    
    /// <summary>重置定时器</summary>
    public void Reset()
    {
        elapsedTime = 0;
        elapsedFrames = 0;
        isRunning = true;
        isCompleted = false;
        isPaused = false;
        loopCount = originalLoopCount;
        hasTimedOut = false;
    }
    
    /// <summary>修改剩余时间</summary>
    public void SetRemainingTime(float remainingTime)
    {
        if (!isFrameTimer && !isConditionTimer)
        {
            elapsedTime = duration - remainingTime;
        }
    }
    
    /// <summary>获取剩余时间</summary>
    public float GetRemainingTime()
    {
        if (isFrameTimer || isConditionTimer) return -1;
        return Mathf.Max(0, duration - elapsedTime);
    }
    
    /// <summary>获取剩余帧数</summary>
    public int GetRemainingFrames()
    {
        if (!isFrameTimer) return -1;
        return Mathf.Max(0, (int)duration - elapsedFrames);
    }
}