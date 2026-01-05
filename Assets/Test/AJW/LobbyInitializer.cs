using UnityEngine;

public class LobbyInitializer : MonoBehaviour
{
    [Header("Lobby 씬 전용 풀 설정")]
    [SerializeField] private PoolConfigBase _boxPoolConfig;

    private void Awake()
    {
        // 씬 로드 시 풀 생성
        PoolManager.Instance.CreatePoolFromConfig(_boxPoolConfig);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
