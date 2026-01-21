using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveDestroy : MonoBehaviour
{
    [Header("충돌 설정")]
    [SerializeField] private LayerMask _hitLayer;

    [Header("소리")]
    [SerializeField]
    private bool _sound;
    [SerializeField, EnableIf(nameof(_sound))]
    private ESoundType[] _sounds;


    [Header("카메라 쉐이크")]
    [SerializeField]
    private bool _cameraShake;
    [SerializeField, EnableIf(nameof(_cameraShake))]
    private float _intensity = 1f;
    [SerializeField, EnableIf(nameof(_cameraShake))]
    private float _duration = 1f;

    public GameObject m_gameObjectMain;
    public GameObject m_gameObjectTail;
    GameObject m_makedObject;
    public Transform m_hitObject;
    public float maxLength;
    public bool isDestroy;
    public float ObjectDestroyTime;
    public float TailDestroyTime;
    public float HitObjectDestroyTime;
    public float maxTime = 1;
    public float MoveSpeed = 10;
    public bool isCheckHitTag;
    public string mtag;
    public bool isShieldActive = false;
    public bool isHitMake = true;

    float time;
    bool ishit;
    float m_scalefactor;

    private void Start()
    {
        m_scalefactor = 1;//transform.parent.localScale.x;
        time = Time.time;
    }

    void LateUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed * m_scalefactor);
        if (!ishit)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength, _hitLayer))
                HitObj(hit);
        }

        if (isDestroy)
        {
            if (Time.time > time + ObjectDestroyTime)
            {
                MakeHitObject(transform);
                Destroy(gameObject);
            }
        }
    }

    void MakeHitObject(RaycastHit hit)
    {
        if (isHitMake == false)
            return;
        m_makedObject = Instantiate(m_hitObject, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
        m_makedObject.transform.parent = transform.parent;
        m_makedObject.transform.localScale = new Vector3(1, 1, 1);
    }

    void MakeHitObject(Transform point)
    {
        if (isHitMake == false)
            return;
        m_makedObject = Instantiate(m_hitObject, point.transform.position, point.rotation).gameObject;
        m_makedObject.transform.parent = transform.parent;
        m_makedObject.transform.localScale = new Vector3(1, 1, 1);
    }

    void HitObj(RaycastHit hit)
    {
        if (isCheckHitTag)
            if (hit.transform.tag != mtag)
                return;
        ishit = true;
        if(m_gameObjectTail)
            m_gameObjectTail.transform.parent = null;
        MakeHitObject(hit);

        if (isShieldActive)
        {
            ShieldActivate m_sc = hit.transform.GetComponent<ShieldActivate>();
            if(m_sc)
                m_sc.AddHitObject(hit.point);
        }
        if (_cameraShake)
        {
            HitShake();
        }
        if (_sound)
        {
            SoundManager.Instance.Play(_sounds[Random.Range(0, _sounds.Length - 1)], transform.position);
        }
        Destroy(this.gameObject);
        Destroy(m_gameObjectTail, TailDestroyTime);
        Destroy(m_makedObject, HitObjectDestroyTime);
    }

    void HitShake()
    {
        CameraManager.Instance?.CameraShake(_intensity, _duration);
    }
}
