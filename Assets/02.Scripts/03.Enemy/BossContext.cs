using System;
using UnityEngine;

public class BossContext : MonoBehaviour
{
    private GameObject _boss;

    public GameObject Boss { get; private set; }

    public event Action OnBossAssigned;

    public void Start()
    {
        if (_boss != null)
        {
            SetBoss(_boss);
        }
    }
    public void SetBoss(GameObject boss)
    {
        Boss = boss;

        OnBossAssigned?.Invoke();
    }

    public void Subscribe(Action action)
    {
        OnBossAssigned += action;
    }
    public void Unsubscribe(Action action)
    {
        OnBossAssigned -= action;
    }
}
