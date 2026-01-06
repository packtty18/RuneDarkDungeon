using DG.Tweening.Core.Easing;
using UnityEngine;


public class CursorManager : GlobalSingleton<CursorManager>
{
    public bool _isLock;

    private void Start()
    {
        if (_isLock) LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetCursorLock(bool lockCursor)
    {
        if (_isLock == lockCursor) return;

        _isLock = lockCursor;
        if (lockCursor) LockCursor();
        else UnlockCursor();
    }
}