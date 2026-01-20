using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField]
    private UI_BasicAnimation _fadeBlack;
    [SerializeField]
    private float _defaultFadeTime = 0.5f;

    [SerializeField]
    private float _deathFadeTime = 2.0f;

    [SerializeField]
    private float _gameOverDelay = 2.0f;

    private SceneTransition _transition;


    private void Start()
    {
        _fadeBlack?.Show();
        _fadeBlack?.FadeOut(_defaultFadeTime);
        TryGetComponent<SceneTransition>(out _transition);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverDelay());
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(_gameOverDelay);
        _fadeBlack?.FadeIn(_deathFadeTime, DG.Tweening.Ease.OutElastic).OnComplete(() =>
        {
            _transition?.TransitionToScene();
        });
    }
}
