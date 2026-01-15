using UnityEngine;

public class Item_Rune : ItemBase
{
    protected override void OnCollected()
    {
        //데이터 추가
        Debug.Log("룬 획득");
    }
}
