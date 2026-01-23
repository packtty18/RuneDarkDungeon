using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyController enemy))
        {
            enemy.Dead();
            return;
        }

        Util.ObjectDestroy(other.gameObject);
    }
}
