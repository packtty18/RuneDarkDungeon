using Sirenix.OdinInspector;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawnManager _conntectedManager;
    [SerializeField] private Collider _collider;
    [SerializeField] private bool _isActivate = false;
    private bool _triggered;



    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _isActivate = false;

    }

    [Button]
    public void ActiveSwitch()
    {
        _collider.enabled = true;
        _isActivate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered || !_isActivate)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("[SpawnTrigger] Player entered");

        _conntectedManager.PlayCurrentPhase();

        _triggered = true;
    }
}
