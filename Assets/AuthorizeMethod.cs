using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AuthorizeMethod : MonoBehaviour
{
    public Button authorizeButton; // Reference to the authorize button
    private string apiKey = "NjVkNDIyMjNmMjc3NmU3OTI5MWJmZGI0OjY1ZDQyMjIzZjI3NzZlNzkyOTFiZmRhYQ";
    public GameObject PopUpAuth;
    public GameObject PopUpAuthError;

    void Start()
    {
        GameObject.Find("AuthorizeButton").GetComponent<Button>().onClick.AddListener(Authorize);
        PopUpAuth.SetActive(false);
        PopUpAuthError.SetActive(false);
    }

    void Authorize()
    {
        StartCoroutine(Authorize_Coroutine());
    }
    IEnumerator Authorize_Coroutine()
    {
        string url = "http://20.15.114.131:8080/api/login";

        // Create JSON request body with API key
        string requestData = "{\"apiKey\": \"" + apiKey + "\"}";


        using (UnityWebRequest request = UnityWebRequest.Post(url, new List<IMultipartFormSection> { new MultipartFormDataSection(requestData) }))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                authorizeButton.gameObject.SetActive(false);
                PopUpAuthError.SetActive(true);
            }
            else
            {
                string jwtToken = request.downloadHandler.text;
                authorizeButton.gameObject.SetActive(false);
                PopUpAuth.SetActive(true); 
            }
        }
    }
}