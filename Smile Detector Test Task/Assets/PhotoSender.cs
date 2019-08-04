using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace SmileDetectorTestTask
{
    public class PhotoSender : MonoBehaviour
{

    [SerializeField] private MainUI _mainUI;
    // Replace <Subscription Key> with your valid subscription key.
    const string subscriptionKey = "53db09b4f6df468eadb20a52b7de7d5c";

    private const string BASE_URL =
        "https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect";
    private const string REQUEST_PARAMS = "?returnFaceAttributes=smile&recognitionModel=recognition_01&detectionModel=detection_01";
    

    private void Start() {
        //StartCoroutine(Upload());
        // StartCoroutine(Post("https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceAttributes=smile&recognitionModel=recognition_01&detectionModel=detection_01",
        // System.IO.File.ReadAllBytes(Application.dataPath + "/Photos/testing.png"), FaceDataParser));
    }

    public void SendPhotoToServer(byte[] photo)
    {
        StartCoroutine(SendPhoto(BASE_URL + REQUEST_PARAMS, photo, FaceDataParser));
    }

    // IEnumerator Upload()
    // {

    //     // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
    //     // formData.Add(new MultipartFormDataSection("field1=foo&field2=bar", System.IO.File.ReadAllBytes(Application.dataPath + "/Photos/testing.png"), "application/octet-stream"));

    //     // UnityWebRequest www = UnityWebRequest.Post("https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_01&returnRecognitionModel=false&detectionModel=detection_01", formData);
    //     // www.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
    //     // yield return www.SendWebRequest();

    //     // if (www.isNetworkError || www.isHttpError)
    //     // {
    //     //     Debug.Log(www.error);
    //     // }
    //     // else
    //     // {
    //     //     Debug.Log("Form upload complete!");
    //     // }

    //     // using (UnityWebRequest www = UnityWebRequest.Put("https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_01&returnRecognitionModel=false&detectionModel=detection_01",
    //     // System.IO.File.ReadAllBytes(Application.dataPath + "/Photos/testing.png")))
    //     // {
    //     //     //www.SetRequestHeader("Content-Type", "application/octet-stream");
    //     //     //www.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
    //     //     yield return www.SendWebRequest();

    //     //     if (www.isNetworkError)
    //     //     {
    //     //         Debug.Log(www.error);
    //     //     }
    //     //     else
    //     //     {
    //     //         Debug.Log(www.downloadHandler.text);
    //     //     }
    //     // }
    // }

    private void FaceDataParser(byte[] data)
    {
        string json = FormatJsonResult(data);
        if(string.IsNullOrEmpty(json))
        {
            Debug.Log("Wrong picture!");
            _mainUI.ShowMessage("<color=red>Face in not detected!");
            return;
        }
        else
        {
            FaceDetectionData faceData = JsonUtility.FromJson<FaceDetectionData>(json);
            if(faceData != null)
            {
                _mainUI.ShowSmileDetectionResult(faceData.faceAttributes.smile);
                Debug.Log(faceData.faceAttributes.smile);
            }
            else
            {
                //_mainUI.ShowMessage("Problem with parsing the results!");
                Debug.LogWarning("Problem with parsing the results!");
            }
            
        }
    }

    public static string FormatJsonResult(byte[] data)
        {
            string jsonResult = System.Text.Encoding.UTF8.GetString(data);
            //jsonResult = System.String.Format("{0}{1}{2}{3}{4}", "{\"", jsonWrapperKey, "\":", jsonResult, "}");
            jsonResult = jsonResult.TrimStart('[').TrimEnd(']');
            Debug.Log(jsonResult);
            return jsonResult;
        }

    IEnumerator SendPhoto(string url, byte[] bodyBytes, Action<byte[]> ParseResults)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = bodyBytes;
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        request.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

        yield return request.Send();

                    if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                ParseResults(request.downloadHandler.data);
            }
 
        Debug.Log("Status Code: " + request.downloadHandler.text);
    }
}

}
