using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorRain : PoolSpawner
{
    [SerializeField] private EPoolType _meteor;
    
    public float m_startDelay;
    public int m_makeCount;
    public float m_makeDelay;
    public Vector3 m_randomPos;
    public Vector3 m_randomRot;
    public Vector3 m_randomScale;

    float m_Time;
    float m_Time2;
    float m_delayTime;
    float m_count;
    
    private void OnEnable()
    {
        m_Time = m_Time2 = Time.time;
        m_count = 0;
    }

    private void Update()
    {
        if (Time.time > m_Time + m_startDelay)
        {
            if (Time.time > m_Time2 + m_makeDelay && m_count < m_makeCount)
            {
                Vector3 m_pos = transform.position + GetRandomVector(m_randomPos); 
                Quaternion m_rot = transform.rotation * Quaternion.Euler(GetRandomVector(m_randomRot));
                
                GameObject m_obj = GetFromPool(_meteor);
                m_obj.transform.position = m_pos;
                m_obj.transform.rotation = m_rot;
                Vector3 m_scale = (m_obj.transform.localScale + GetRandomVector2(m_randomScale));
                    
                m_obj.transform.localScale = m_scale;

                m_Time2 = Time.time;
                m_count++;
            }
        }
    }

    #region Util
    private float GetRandomValue(float value)
    {
        return Random.Range(-value, value);
    }

    private float GetRandomValue2(float value)
    {
        return Random.Range(0, value);
    }

    private Vector3 GetRandomVector(Vector3 value)
    {
        Vector3 result;
        result.x = GetRandomValue(value.x);
        result.y = GetRandomValue(value.y);
        result.z = GetRandomValue(value.z);
        return result;
    }

    private Vector3 GetRandomVector2(Vector3 value)
    {
        Vector3 result;
        result.x = GetRandomValue2(value.x);
        result.y = GetRandomValue2(value.y);
        result.z = GetRandomValue2(value.z);
        return result;
    }
    #endregion
}
