using ProceduralMeshExploder;
using Sirenix.OdinInspector;
using UnityEngine;

public class PropObject : MonoBehaviour, IDamageable
{
    [SerializeField] private MeshExploder _exploder;
    [SerializeField] private ItemDropper _dropper;

    [SerializeField] private int _hitCount = 5;
    [SerializeField] private ESoundType _destroySound;
    [SerializeField] private ESoundType _hitSound;
    public ETeamType Team => ETeamType.Enemy;

    private void Awake()
    {
        _exploder = GetComponentInChildren<MeshExploder>();
        _dropper = GetComponentInChildren<ItemDropper>();
    }

    [Button]
    public void ApplyDamage(DamageData data)
    {
        _hitCount--;

        if(_hitCount <= 0)
        {
            SoundManager.Instance?.Play(_destroySound, transform.position);
            Collapse();
        }
        else
        {
            SoundManager.Instance?.Play(_hitSound, transform.position);
        }
    }

    private void Collapse()
    {
        if (_dropper != null)
        {
            _dropper.Drop();
        }
        _exploder.Explode();

        Util.ObjectDestroy(gameObject);
    }
}
