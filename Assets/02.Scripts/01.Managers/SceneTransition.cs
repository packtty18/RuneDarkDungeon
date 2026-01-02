using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private SceneDataSO _TargetScene;
    [SerializeField]
    private float _delay = 0;
    
    public void TransitionToScene()
    {
        if (_TargetScene != null)
        {
            StartCoroutine(TransitionWithDelay());
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
