using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField]
    private UI_BasicAnimation _fadeBlack;
    [SerializeField]
    private float _fadeTime = 0.5f;

    void Start()
    {
        _fadeBlack?.Show();
        _fadeBlack?.FadeOut(_fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
