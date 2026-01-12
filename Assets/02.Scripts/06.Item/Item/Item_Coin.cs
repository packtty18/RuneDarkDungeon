using UnityEngine;

public class Item_Coin : ItemBase
{
    protected override void OnCollected()
    {
        //데이터 추가
        Debug.Log("코인 획득");
    }
}
