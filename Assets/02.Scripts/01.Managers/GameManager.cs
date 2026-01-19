using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField]
    private UI_BasicAnimation _fadeBlack;
    [SerializeField]
    private float _fadeTime = 0.5f;

    [SerializeField]
    private float _gameOverDelay = 1f;

    private SceneTransition _transition;


    private void Start()
    {
        _fadeBlack?.Show();
        _fadeBlack?.FadeOut(_fadeTime);
        TryGetComponent<SceneTransition>(out _transition);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverDelay());
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(_gameOverDelay);
        _fadeBlack?.FadeIn(_fadeTime, DG.Tweening.Ease.InOutElastic).OnComplete(() =>
        {
            _transition?.TransitionToScene();
        });
    }
}
