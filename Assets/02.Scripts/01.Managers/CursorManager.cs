using UnityEngine;


public class CursorManager : GlobalSingleton<CursorManager>
{
    [SerializeField]
    private bool _isLock;

    private void Start()
    {
        if (_isLock) LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
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