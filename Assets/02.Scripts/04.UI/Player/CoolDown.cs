using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CoolDown : MonoBehaviour
{
    [SerializeField] 
    private PlayerSkillCaster _skillCaster;
    [SerializeField] 
    private List<Image> coolDownImages;

    void Start()
    {
        _skillCaster.OnCoolTimeChanged += CoolTimeUpdate;

        InitCoolTime();
    }


    private void CoolTimeUpdate(Dictionary<ESkillSlot, float> coolTimes, Dictionary<ESkillSlot, float> coolDowns)
    {
        foreach (ESkillSlot key in coolTimes.Keys)
        {
            fillCoolTime(key, coolTimes, coolDowns);
        }
    }

    private void fillCoolTime(ESkillSlot slot, Dictionary<ESkillSlot, float> coolTime, Dictionary<ESkillSlot, float> coolDown)
    {
        if (coolTime[slot] == 0) return;
        float value = coolDown[slot] / coolTime[slot];
        coolDownImages[(int)slot-1].fillAmount = Mathf.Lerp(0, 1, value);
    }

    private void InitCoolTime()
    {
        foreach (Image image in coolDownImages)
        {
            image.fillAmount = 0;
        }  
    }

    private void OnDestroy()
    {
        _skillCaster.OnCoolTimeChanged += CoolTimeUpdate;
    }
}
