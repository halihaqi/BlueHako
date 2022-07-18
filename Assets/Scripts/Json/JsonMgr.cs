using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public enum JsonType
{
    JsonUtlity,
    LitJson
}

public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }

    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        switch (type)
        {
            case JsonType.JsonUtlity:
                File.WriteAllText(path, JsonUtility.ToJson(data));
                break;
            case JsonType.LitJson:
                File.WriteAllText(path, JsonMapper.ToJson(data));
                break;
            default:
                break;
        }
    }

    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //先判断是否有默认数据文件
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            return new T();
        string jsonData = File.ReadAllText(path);
        switch (type)
        {
            case JsonType.JsonUtlity:
                return JsonUtility.FromJson<T>(jsonData);
            case JsonType.LitJson:
                return JsonMapper.ToObject<T>(jsonData);
            default:
                return default(T);
        }
    }

}
