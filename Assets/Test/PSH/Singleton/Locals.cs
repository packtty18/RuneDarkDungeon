using UnityEngine;

public class Locals : LocalSingleton<Locals>
{
    protected override void OnInit()
    {
        Debug.Log("Locals Initialized");
    }
}
