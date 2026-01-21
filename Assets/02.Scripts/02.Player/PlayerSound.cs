using UnityEngine;

public class PlayerSound : MonoBehaviour
{
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

    public void OnWalkSound()
    {
        OnSoundPlay(ESoundType.Player_Walk);
    }

}
