using System;
using UnityEngine;

public enum EPauseReason
{
    None,
    UI,         //팝업 UI 작동으로 인한 정지(설정, 인벤 등)
    Cutscene    //보스 연출로 인한 정지
}

public static class PauseContext
{
    public static bool IsPaused { get; private set; }
    public static EPauseReason Reason { get; private set; }

    public static event Action<bool, EPauseReason> OnPauseChanged;

    public static void Pause(EPauseReason reason)
    {
        if (IsPaused)
            return;

        IsPaused = true;
        Reason = reason;

        Debug.Log($"[PauseContext] Paused ({reason})");
        OnPauseChanged?.Invoke(true, reason);
    }

    public static void Resume()
    {
        if (!IsPaused)
            return;

        IsPaused = false;
        Reason = EPauseReason.None;

        Debug.Log("[PauseContext] Resumed");
        OnPauseChanged?.Invoke(false, Reason);
    }
}
