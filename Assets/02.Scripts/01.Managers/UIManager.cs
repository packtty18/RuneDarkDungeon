using System.Collections.Generic;
using UnityEngine;

public class UIManager : GlobalSingleton<UIManager> 
{
    /*
     * UI는 씬 오브젝트를 직접 참조하지 않는다. 플레이어등 씬에서 직접 인스펙터로 참조하지 않기
     *  무조건 이벤트 기반, 옵저버로 구독할것. 타겟대상이 널이라면 Close하는 코드도 잘 적용
     * 씬 전환 시 Popup 모두 Close로 정리할것
     * 씬 전환시 각 씬에 맞는 HUD Open, 아닌건 Close
    */

    [Header("HUD")]
    [SerializeField] private Dictionary<string, HUDBase> _huds;

    [Header("Popup")]
    [SerializeField] private Dictionary<string, PUBase> _pus;


    // 씬 전환시 해당 씬에 맞는 UI상태 적용
    public void ApplySceneUIState(ESceneType state)
    {

        switch (state)
        {
            case ESceneType.MainMenu:
                break;
            case ESceneType.Gameplay:
               break;
            case ESceneType.Loading:
                break;
            case ESceneType.Lobby:
                break;
        }
    }
}
