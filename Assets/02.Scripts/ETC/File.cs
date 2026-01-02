using UnityEngine;
using System.IO;
using System.Text;

public class File
{
    private static readonly string s_saveFilePath;
    private static readonly string s_saveFileName = "Save.json";
    
    static File()
    {
        s_saveFilePath = Path.Combine(Application.persistentDataPath, s_saveFileName);
    }

    public static void Save(GameData data)
    {
        using (var stream = new FileStream(s_saveFilePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonUtility.ToJson(data);
                writer.Write(json);
            }
        }
        
        Debug.Log($"<color=green>[데이터 저장 성공]</color> {s_saveFilePath}");
    }

    public static void Load(GameData data)
    {
        if (!System.IO.File.Exists(s_saveFilePath)) return;

        string json = System.IO.File.ReadAllText(s_saveFilePath);
        
        JsonUtility.FromJsonOverwrite(json, data);
        
        Debug.Log("<color=cyan>[데이터 로드 성공]</color>");
        Debug.Log(data.GetSummary());
    }
}
