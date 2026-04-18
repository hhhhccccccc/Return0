using Spine.Unity;
using UnityEngine;
using Zenject;

public partial class UISpineItem
{
    private GameObject m_obj { get; set; } 
    private SkeletonGraphic m_spine { get; set; }
    public void SetModel(int id)
    {
        
    }

    public void SetModel(string prefabPath)
    {
        m_obj = PoolManager.GetGameObject(prefabPath, transform);
        m_spine = m_obj.GetComponent<SkeletonGraphic>();
    }

    /// <summary>播放动画并返回动画信息</summary>
    public AnimationPlayInfo PlayAnimation(string animName, bool loop = false, int trackIndex = 0)
    {
        if (m_spine == null)
        {
            return null;
        }
        
        var track = m_spine.AnimationState.SetAnimation(trackIndex, animName, loop);
        
        var skeletonData = m_spine.SkeletonDataAsset.GetSkeletonData(false);
        var findAnimation = skeletonData.FindAnimation(animName);
        float duration = findAnimation?.Duration ?? 0f;
        
        return new AnimationPlayInfo
        {
            animationName = animName,
            currentTime = track.TrackTime,
            duration = duration,
            isLoop = loop,
            normalizedTime = duration > 0 ? track.TrackTime / duration : 0f,
            track = track
        };
    }
    
    /// <summary>获取当前播放时间</summary>
    public float GetCurrentTime(int trackIndex = 0)
    {
        var track = m_spine.AnimationState.GetCurrent(trackIndex);
        return track?.TrackTime ?? 0f;
    }
    
    /// <summary>获取指定动画时长</summary>
    public float GetAnimationLength(string animationName)
    {
        var skeletonData = m_spine.SkeletonDataAsset.GetSkeletonData(false);
        var findAnimation = skeletonData.FindAnimation(animationName);
        return findAnimation?.Duration ?? 0f;
    }
    
    /// <summary>获取播放进度 (0-1)</summary>
    public float GetProgress(int trackIndex = 0)
    {
        var track = m_spine.AnimationState.GetCurrent(trackIndex);
        if (track == null || track.Animation == null) return 0f;
        return track.TrackTime / track.Animation.Duration;
    }
    
    public class AnimationPlayInfo
    {
        public string animationName;
        public float currentTime;
        public float duration;
        public bool isLoop;
        public float normalizedTime;
        public Spine.TrackEntry track;
    }
}
