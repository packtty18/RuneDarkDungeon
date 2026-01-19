using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField]
    private List<UI_Popup> _popups;

    void Update()
    {
        foreach(var popup in _popups)
        {
            popup.InputCheck();
        }
    }
}
