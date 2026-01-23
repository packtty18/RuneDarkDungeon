using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    [Header("연결 대상 UI")]
    [SerializeField] private UI_Initializer _initializer;

    private void Start()
    {
        IDataHolder dataHolder = DataManager.Instance;
        _initializer.Initialize(dataHolder);
    }
}
