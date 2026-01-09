using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [Header("슬롯 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    public IReadOnlyList<UI_Slot> Slots => _slots;
    
    [Header("슬롯 개수 설정")]
    [SerializeField] private int _minSlotCount;
    [SerializeField] private UI_Slot _slotPrefab;
    [SerializeField] private Transform _slotParent;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;

        SetSlotsIndex();
        Refresh();
        _inventory.Subscribe(Refresh);
    }
    
    private void OnDestroy()
    {
        _inventory?.Unsubscribe(Refresh);
    }
    
    private void SetSlotsIndex()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetIndex(i); 
        }
    }
    
    private void Refresh()
    {
        var items = _inventory.Items;
        // 1. 필요한 목표 슬롯 개수 계산
        int targetSlotCount = Mathf.Max(_minSlotCount, items.Count);

        // 2. 슬롯이 모자라면 추가 생성
        while (_slots.Count < targetSlotCount)
        {
            UI_Slot newSlot = Instantiate(_slotPrefab, _slotParent);
            newSlot.SetIndex(_slots.Count);
            _slots.Add(newSlot);
        }

        // 3. 슬롯 순회하며 데이터 채우기 및 활성/비활성 결정
        for (int i = 0; i < _slots.Count; i++)
        {
            // 현재 순번이 목표 개수보다 많으면 비활성화
            if (i >= targetSlotCount)
            {
                _slots[i].gameObject.SetActive(false);
                continue;
            }

            // 슬롯 활성화
            _slots[i].gameObject.SetActive(true);

            // 슬롯 뷰 설정
            if (i < items.Count)
            {
                var item = items[i];
                var data = _itemDB.GetSlotData(item); 
                _slots[i].SetItem(data);
            }
            else
            {
                // 아이템은 없지만 최소 슬롯 범위 안일 때 빈칸으로 표시
                _slots[i].Clear();
            }
        }
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
