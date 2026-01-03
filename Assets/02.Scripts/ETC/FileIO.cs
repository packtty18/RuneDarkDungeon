using UnityEngine;
using System;
using System.IO;
using System.Text;

public class FileIO
{
    private static readonly string s_saveFilePath;
    private static readonly string s_saveFileName = "Save.json";
    private static readonly string s_securityKey = "SecretKeyForSave";
    private static readonly string s_hashedKey;

    static FileIO()
    {
        s_saveFilePath = Path.Combine(Application.persistentDataPath, s_saveFileName);
        s_hashedKey = AES.GetHash(s_securityKey);
    }

    public static void Save(GameData data)
    {
        
        using (var stream = new FileStream(s_saveFilePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonUtility.ToJson(data);
                
                string encrypted = AES.Encrypt(json, s_hashedKey);
                
                writer.Write(encrypted);
                
#if UNITY_EDITOR
                Debug.Log($"<color=green>[데이터 저장 성공]</color> {s_saveFilePath}");
                Debug.Log($"<color=green>[원본]</color> {json}");
                Debug.Log($"<color=yellow>[암호화됨]</color> {encrypted}");
#endif
            }
        }
    }

    public static void Load(GameData data)
    {
        if (!System.IO.File.Exists(s_saveFilePath)) return;

        string encrypted = System.IO.File.ReadAllText(s_saveFilePath);
        string decrypted = string.Empty;
        
        try
        {
            decrypted = AES.Decrypt(encrypted, s_hashedKey);
#if UNITY_EDITOR
            Debug.Log($"<color=cyan>[복호화됨]</color> {decrypted}");
#endif
        }
        catch (Exception)
        {
            Debug.Log("<color=red>[데이터 로드 실패]</color>");
            return;
        }
        
        JsonUtility.FromJsonOverwrite(decrypted, data);
        
        Debug.Log("<color=cyan>[데이터 로드 성공]</color>");
        Debug.Log(data.GetSummary());
    }
}
