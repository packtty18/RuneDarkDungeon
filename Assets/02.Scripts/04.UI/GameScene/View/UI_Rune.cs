using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rune : MonoBehaviour
{
    [SerializeField] private GameObject _qRune;
    [SerializeField] private GameObject _eRune;
    [SerializeField] private GameObject _rRune;
    [SerializeField] private Image _qRuneImage;
    [SerializeField] private Image _eRuneImage;
    [SerializeField] private Image _rRuneImage;

    private IReadOnlyEquipment _equipment;

    private void Start()
    {
        _equipment = DataManager.Instance.Equipment;
        _equipment.Subscribe(Refresh);

        Refresh();
    }

    private void OnDestroy()
    {
        _equipment.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        var qItem = _equipment.GetItem(ESkillSlot.Q);
        if (qItem == null)
        {
            _qRune.SetActive(false);
        }
        else
        {
            _qRune.SetActive(true);
            _qRuneImage.sprite = qItem.Icon;
        }
        
        var eItem = _equipment.GetItem(ESkillSlot.E);
        if (eItem == null)
        {
            _qRune.SetActive(false);
        }
        else
        {
            _qRune.SetActive(true);
            _qRuneImage.sprite = eItem.Icon;
        }
        
        var rItem = _equipment.GetItem(ESkillSlot.R);
        if (rItem == null)
        {
            _qRune.SetActive(false);
        }
        else
        {
            _qRune.SetActive(true);
            _qRuneImage.sprite = rItem.Icon;
        }
    }
}
