using UnityEngine;

public class InGameDataInitializer : MonoBehaviour
{
    [Header("연결 대상 UI")]
    [SerializeField] private UI_Initializer _initializer;

    private void Start()
    {
        IDataHandler dataHandler = DataManager.Instance;
        _initializer.Initialize(dataHandler);
    }
}
