using UnityEngine;
using System.Collections;

public class TimedTrigger : MonoBehaviour, IAttackTrigger
{
    [SerializeField] private float delay = 1f;
    private EnemyAttackObject _owner;
    [SerializeField] private bool _autoDestory;
    public void Initialize(EnemyAttackObject owner, AttackObjectDataSO data)
    {
        _owner = owner;
        StartCoroutine(TriggerRoutine());
    }

    private IEnumerator TriggerRoutine()
    {
        yield return new WaitForSeconds(delay);
        _owner.ExecuteEffects(transform.position);

        if (_autoDestory)
        {
            Util.ObjectDestroy(gameObject);
        }
    }
}
