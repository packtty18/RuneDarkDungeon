using System;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    public GameObject Player { get; private set; }
    public PlayerStats Stats { get; private set; }
    public PlayerAttack Attack { get; private set; }

    public PlayerSkillCaster SkiilCaster { get; private set; }

    public event Action OnPlayerAssigned;

    public void Start()
    {
        if (_player != null)
        {
            SetPlayer(_player);
        }
    }
    public void SetPlayer(GameObject player)
    {
        Player = player;
        Stats = player.GetComponent<PlayerStats>();
        Attack = player.GetComponent<PlayerAttack>();
        SkiilCaster = player.GetComponent<PlayerSkillCaster>();

        OnPlayerAssigned?.Invoke();
    }

    public void Subscribe(Action action)
    {
        OnPlayerAssigned += action;
    }
    public void Unsubscribe(Action action)
    {
        OnPlayerAssigned -= action;
    }
}
