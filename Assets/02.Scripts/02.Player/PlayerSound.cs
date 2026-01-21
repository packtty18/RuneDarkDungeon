using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    private ESoundType[] _footSteps;

    [SerializeField]
    private ESoundType[] _yells;
    public void OnSoundPlay(ESoundType soundType)
    {
        SoundManager.Instance.Play(soundType, transform.position);
    }

    public void OnBasicAttackSound()
    {
        OnSoundPlay(ESoundType.Player_BasicAttack);
    }
    public void OnDashAttackSound()
    {
        OnSoundPlay(ESoundType.Player_DashAttack);
    }
    public void OnFinisherAttackGroundSound()
    {
        OnSoundPlay(ESoundType.Player_FinisherAttack);
    }

    public void OnFootSound()
    {
        OnSoundPlay(_footSteps[Random.Range(0, _footSteps.Length-1)]) ;
    }

    public void OnYell()
    {
        OnSoundPlay(_yells[Random.Range(0, _yells.Length - 1)]);
    }

    public void OnLand()
    {
        OnSoundPlay(ESoundType.Player_Land);
    }

}
