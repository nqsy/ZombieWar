using System;
using UnityEngine;

public class Cooldown
{
    private float duration;
    private float remain;
    private float warningRemain;
    private bool isCanCooldown = true;

    public bool IsFinishing { get => remain < 0; }
    public bool IsNearFinishing { get => remain < warningRemain; }
    public float Process { get => 1 - Math.Max(remain, 0) / duration; }
    public float NearProcess { get => 1 - Math.Max(warningRemain, 0) / duration; }
    public float Remain { get => remain; }
    public float Duration { get => duration; }

    public void Restart()
    {
        remain = duration;
    }

    public void Restart(float duration)
    {
        this.duration = duration;
        Restart();
    }

    public void ReduceCooldown(float val)
    {
        if (!isCanCooldown)
            return;

        remain -= val;
    }

    public void SetRemain(float val)
    {
        remain = val;
    }

    public void ReduceCooldown()
    {
        ReduceCooldown(Time.deltaTime);
    }

    public void DisableCooldown()
    {
        isCanCooldown = false;
    }

    //unit duration: seconds
    public Cooldown(float duration, float warningRemain = 2)
    {
        this.warningRemain = warningRemain;
        Restart(duration);
    }
}
