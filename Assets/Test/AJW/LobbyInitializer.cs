using System.Collections.Generic;
using UnityEngine;

public class LobbyInitializer : MonoBehaviour
{
    [Header("Lobby 씬 전용 풀 설정")]
    [SerializeField] private List<PoolConfigBase> _poolConfigs;

    private void Start()
    {
        // 씬 로드 시 풀 생성
        PoolManager.Instance.CreatePoolsFromConfigs(_poolConfigs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
