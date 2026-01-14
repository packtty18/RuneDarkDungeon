using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff : MonoBehaviour
{
    protected EnemyController _controller;
    [ShowInInspector] private readonly List<ActiveBuff> _activeBuffs = new();

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    public virtual void Init()
    {
        ClearAllBuffs();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        for (int i = _activeBuffs.Count - 1; i >= 0; i--)
        {
            if (_activeBuffs[i].IsEternal)
                continue;

            _activeBuffs[i].RemainTime -= delta;

            if (_activeBuffs[i].RemainTime <= 0f)
            {
                RemoveBuffInternal(_activeBuffs[i]);
            }
        }
    }

    public void ApplyBuff(BuffSO buff)
    {
        StatModifier modifier = default;
        modifier = new StatModifier(buff.Value, buff.ModType);
        _controller.Stat.GetValue(buff.TargetStat).AddModifier(modifier);
        _activeBuffs.Add(new ActiveBuff(buff, modifier));
    }

    public void RemoveBuff(BuffSO buff)
    {
        for (int i = _activeBuffs.Count - 1; i >= 0; i--)
        {
            if (_activeBuffs[i].Data == buff)
            {
                RemoveBuffInternal(_activeBuffs[i]);
            }
        }
    }

    public void ClearAllBuffs()
    {
        for (int i = _activeBuffs.Count - 1; i >= 0; i--)
        {
            RemoveBuffInternal(_activeBuffs[i]);
        }
    }

    private void RemoveBuffInternal(ActiveBuff buff)
    {
        _controller.Stat.GetValue(buff.Data.TargetStat).RemoveModifier(buff.Modifier);
        _activeBuffs.Remove(buff);
    }

    private class ActiveBuff
    {
        public BuffSO Data;
        public bool IsEternal;
        public float RemainTime;
        public StatModifier Modifier;

        public ActiveBuff(BuffSO data, StatModifier modifier)
        {
            Data = data;
            IsEternal = !data.HasDuration;
            RemainTime = data.Duration;
            Modifier = modifier;
        }
    }
}
