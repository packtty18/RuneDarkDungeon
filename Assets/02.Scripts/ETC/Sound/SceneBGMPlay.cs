using UnityEngine;

public class SceneBGMPlay : MonoBehaviour
{
    [SerializeField]
    private ESoundType _soundType;


    private void Start()
    {
        SoundManager.Instance?.Play(_soundType);
    }
}
