using System.Collections.Generic;
using UnityEngine;

public class BuffObject : MonoBehaviour
{
    //해당 오브젝트와 트리거 되면 버프 적용
   [SerializeField] private List<BuffSO> _buffs;

    private void OnEnable()
    {
        if(!SoundManager.IsExist())
        {
            return;
        }

        SoundManager.Instance.Play(ESoundType.Enemy_Boss_Buff, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBuff enemy))
        {
            ApplyTo(enemy);
        }
    }

    private void ApplyTo(EnemyBuff enemy)
    {
        foreach (BuffSO buff in _buffs)
        {
            enemy.ApplyBuff(buff);
        }
    }
}
