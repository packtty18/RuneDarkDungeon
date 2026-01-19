using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneTransition))]
public class GameStartPrompt : MonoBehaviour
{
    
    private SceneTransition _transition;

    [SerializeField]
    private UIBasicAnimation _promptMassge;

    [SerializeField]
    private PopUpInputController _loadDataPopup;
    

    [SerializeField]
    private Button _loadGameButton;

    [SerializeField]
    private Button _newGameButton;
   

    [SerializeField]
    private EGameKeyType _gameKey;

    [SerializeField]
    private bool _fadeLoop = false;
    [SerializeField]
    private float _fadedTime = 0.3f;
    [SerializeField]
    private float _fadedOutValue = 0.3f;

    private void Awake()
    {
        _transition = GetComponent<SceneTransition>();
    }
    private void Start()
    {
        if (_fadeLoop && _promptMassge != null)
        {
            _promptMassge.FadeLoop(_fadedTime, _fadedOutValue);
        }

        if (_transition == null)
        {
            Debug.LogError("SceneTransition 컴포넌트를 찾을 수 없습니다.");
        }
        _loadGameButton?.onClick.AddListener(LoadGame);
        _newGameButton?.onClick.AddListener(NewGame);
    }

    private void NewGame()
    {
        DataManager.Instance.CreateNewGameData();
        _transition.TransitionToScene();
    }

    private void LoadGame()
    {
        _transition.TransitionToScene();
    }

    void Update()
    {
        if (InputManager.Instance.GetKeyDown(_gameKey))
        {
            _loadDataPopup.PopUp();
            if (!FileIO.Exists)
            {
                NewGame();
            }
            else
            {
                _loadDataPopup.PopUp();
            }
        }
    }

    private void OnDestroy()
    {
        _loadGameButton?.onClick.RemoveListener(LoadGame);
        _newGameButton?.onClick.RemoveListener(NewGame);
    }
}
