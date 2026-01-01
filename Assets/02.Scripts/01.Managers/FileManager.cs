using UnityEngine;
using System.IO;
using System.Text;

public class FileManager : GlobalSingleton<FileManager>
{
    private GameData _data;
    
    private string _saveFilePath;
    private const string SaveFileName = "Save.json";
    
    protected override void OnInit()
    {
        _saveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        LoadGame();
    }

    public void SaveGame()
    {
        using (var stream = new FileStream(_saveFilePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonUtility.ToJson(_data);
                writer.Write(json);
            }
        }
        
        Debug.Log($"<color=green>[데이터 저장 성공]</color> {_saveFilePath}");
    }

    private void LoadGame()
    {
        if (File.Exists(_saveFilePath))
        {
            string json = File.ReadAllText(_saveFilePath);

            JsonUtility.FromJsonOverwrite(json, _data);
            
            Debug.Log("<color=cyan>[데이터 로드 성공]</color>");
            Debug.Log(_data.GetSummary());
        }
        else
        {
            _data = new GameData();
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
