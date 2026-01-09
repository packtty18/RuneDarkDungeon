using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private SceneDataSO _TargetScene;
    [SerializeField]
    private float _delay = 0;

    private bool _isTransitioning = false;
    public void TransitionToScene()
    {
        if (_isTransitioning) return;
        if (_TargetScene != null)
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
        _TargetScene.LoadScene();
    }

}
