using UnityEngine;
using UnityEngine.Events;

public class BattleManager : LocalSingleton<BattleManager>  
{
    //던전씬에서 사용할 매니저
    //플레이어를 생성하고 적의 생성을 판단하는 역할

    //플레이의 대한 모든 준비가 끝난다면 이벤트를 실행한다(ex. 첫번째 페이즈의 적 생성, 플레이어 조작 활성화 등)

    [SerializeField] private GameObject _player;

    [SerializeField] private UnityEvent _battleSetting; //전투환경 조정(플레이어 및 적 생성)
    [SerializeField] private UnityEvent _battleStart;   //플레이어 인풋 활성화 및 적 활성화)

    public Transform PlayerTransform => _player?.transform;

    protected override void OnInit()
    {
        base.OnInit();
        _battleSetting.Invoke();
        _battleStart.Invoke();
    }
}
