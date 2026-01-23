using Unity.VisualScripting;
using UnityEngine;

public class OnTriggerTrigger : MonoBehaviour, IAttackTrigger
{
    private EnemyAttackObject _owner;
    [SerializeField] private bool _autoDestory;

    public void Initialize(EnemyAttackObject owner, AttackObjectDataSO data)
    {
        _owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        _owner.ExecuteEffects(transform.position);

        if(_autoDestory)
        {
            Util.ObjectDestroy(gameObject);
        }
    }
}
