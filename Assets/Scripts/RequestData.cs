using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class ClientClass
{
    public bool isManager;
    public int id;
    public string label;
}

[System.Serializable]
public class DataClass
{
    public string address;
    public string name;
    public int points;
}

[System.Serializable]
public class RootObjectClass
{
    public List<ClientClass> clients;
    public List<DataKeyValuePair> data;
    public string label;
}

[System.Serializable]
public class DataKeyValuePair
{
    public string key;
    public DataClass value;
}

public class RequestData : MonoBehaviour
{
    private string url = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakeRequest());
    }

    IEnumerator MakeRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parsing JSON response
                string responseText = webRequest.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                
        
                RootObjectClass parsedData = JsonUtility.FromJson<RootObjectClass>(responseText);
                foreach (ClientClass client in parsedData.clients)
                {
                    Debug.Log($"Client ID: {client.id}, Label: {client.label}");
                }


                // Convert the list of DataKeyValuePair back to a dictionary
                Dictionary<string, DataClass> dataDictionary = new Dictionary<string, DataClass>();
                foreach (DataKeyValuePair kvp in parsedData.data)
                {
                    dataDictionary[kvp.key] = kvp.value;
                }

                // Now you can access the data using the dictionary
                foreach (var kvp in dataDictionary)
                {
                    string key = kvp.Key;
                    DataClass data = kvp.Value;
                    Debug.Log($"Data Key: {key}, Name: {data.name}, Address: {data.address}");
                }

               

                Debug.Log($"Label: {parsedData.label}");
            }
        }
    }

}
