using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    [Title("Reference")]
    [SerializeField] private HitboxController _hitboxController;
    [SerializeField] private GameObject _HitPrefab;
    [SerializeField] private string _hitboxKey = "Main";

    [Title("Damage")]
    [SerializeField] private float _damage = 20f;

    [Title("Timing")]
    [SerializeField] private float _activeDuration = 1f;

    private float _timer;

    private bool _initted = false;

    private void Awake()
    {
        _hitboxController = GetComponentInChildren<HitboxController>();
    }

    [Button]
    public void Init(float damage, int size = 1)
    {
        transform.localScale = new Vector3(size, size, size);
        _timer = _activeDuration;
        _hitboxController.Activate("Main", damage);

        Instantiate(_HitPrefab,transform);

        _initted = true;
    }

    private void Update()
    {
        if (!_initted)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer > 0f)
        {
            return;
        }

        _hitboxController.DeactivateAll();
        _initted = false;
        StartCoroutine(Util.DestroyAfterTime(3f,gameObject));
    }

}
