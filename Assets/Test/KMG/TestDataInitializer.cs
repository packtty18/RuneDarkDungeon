using UnityEngine;

public class TestDataInitializer : MonoBehaviour
{
    [Header("연결 대상 UI")]
    [SerializeField] private UI_Initializer _initializer;

    private void Start()
    {
        IDataHandler dataHandler = RuneUser.Instance;
        _initializer.Initialize(dataHandler);
    }
}
