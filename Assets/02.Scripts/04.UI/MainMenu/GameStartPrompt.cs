using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneTransition))]
public class GameStartPrompt : MonoBehaviour
{
    
    private SceneTransition _transition;

    [SerializeField]
    private UI_Popup _loadDataPopup;
    

    [SerializeField]
    private Button _loadGameButton;

    [SerializeField]
    private Button _newGameButton;
   

    [SerializeField]
    private EGameKeyType _gameKey;

    private void Awake()
    {
        _transition = GetComponent<SceneTransition>();
    }
    private void Start()
    {
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
            if (!FileIO.Exists)
            {
                NewGame();
            }
            else
            {
                _loadDataPopup.Show();
            }
        }
    }

    private void OnDestroy()
    {
        _loadGameButton?.onClick.RemoveListener(LoadGame);
        _newGameButton?.onClick.RemoveListener(NewGame);
    }
}
