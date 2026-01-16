using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private SceneDataSO _targetScene;

    private Button _button;

    [SerializeField]
    private float _delay = 0;

    private bool _isTransitioning = false;

    private void Start()
    {
        TryGetComponent<Button>(out  _button);
        if (_button != null)
        {
            _button.onClick.AddListener(TransitionToScene);
        }

    }
    public void TransitionToScene()
    {
        if (_isTransitioning) return;
        if (_targetScene != null)
        {
            _isTransitioning = true;
            StartCoroutine(TransitionWithDelay());
        }
        else
        {
            Debug.LogWarning("전환할 대상 씬(_targetScene)이 설정되지 않았습니다.", this);
        }
    }

    private IEnumerator TransitionWithDelay()
    {
        if (_delay > 0)
        {
            yield return new WaitForSeconds(_delay);
        }
        _targetScene.LoadScene();
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveAllListeners();
        }
    }

}
