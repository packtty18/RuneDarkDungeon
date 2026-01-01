using UnityEngine;
using System.IO;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data;
    public GameData Data => _data;
    
    private string _saveFilePath;
    private const string SaveFileName = "Save.json";
    
    protected override void OnInit()
    {
        _saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        LoadGame();
    }

    private void SaveGame()
    {
        string json = JsonUtility.ToJson(_data);

        File.WriteAllText(_saveFilePath, json);
        
        Debug.Log($"게임 저장 완료: {_saveFilePath}");
    }

    private void LoadGame()
    {
        if (File.Exists(_saveFilePath))
        {
            string json = File.ReadAllText(_saveFilePath);

            _data = JsonUtility.FromJson<GameData>(json);
            
            Debug.Log("<color=cyan>[데이터 로드 성공]</color>");
            Debug.Log(_data.GetSummary());        }
        else
        {
            Debug.Log("저장된 데이터가 없어 새로 시작합니다.");
            _data = new GameData();
            
            // 테스트용 더미 데이터 추가
            AddTestData(); 
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool paused)
    {
        if (!paused) return;
        SaveGame();
    }

    // [테스트용] 더미 데이터 넣는 함수
    private void AddTestData()
    {
        _data.Gold = 1000;
        _data.Runes.Add(new RuneData(1, "테스트 룬1"));
        _data.Runes.Add(new RuneData(2, "테스트 룬2", 5));
        
        SaveGame();
    }
    
    // [테스트용] 데이터 초기화
    private void DeleteSaveData()
    {
        if (!File.Exists(_saveFilePath)) return;
        
        File.Delete(_saveFilePath);
        _data = new GameData();
        Debug.Log("세이브 데이터 삭제 완료");
    }
}
