using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStats _playerStats;
    private EPlayerState _currentState;

    public PlayerStats PlayerStats => _playerStats;
    public EPlayerState CurrentState => _currentState;
    
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case EPlayerState.Idle:

                break;
            case EPlayerState.Walk:
 
                break;
            case EPlayerState.Run:
  
                break;
            case EPlayerState.Jump:

                break;
            default:
                break;
        }


    }

    public void SetState(EPlayerState newState)
    {
        _currentState = newState;
    }
}
