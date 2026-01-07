using System;
using UnityEngine;

[Serializable]
public class ComboData
{
    [Header("기본 정보")]
    public int ComboIndex;
    public float Damage;

    [Header("타이밍")]
    [Tooltip("다음 콤보 입력 가능 시간")]
    public float InputWindow;
}
