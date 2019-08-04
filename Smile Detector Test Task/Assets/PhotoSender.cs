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
        private const string SUBSCRIPTION_KEY = "53db09b4f6df468eadb20a52b7de7d5c";
        private const string BASE_URL =     "https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        private const string REQUEST_PARAMS = "?returnFaceAttributes=smile&recognitionModel=recognition_01&detectionModel=detection_01";


        private void Start()
        {

        }

        public void SendPhotoToServer(byte[] photo)
        {
            StartCoroutine(SendPhoto(BASE_URL + REQUEST_PARAMS, photo, FaceDataParser));
        }

        private void FaceDataParser(byte[] data)
        {
            string json = FormatJsonResult(data);
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Face on photo was not detected");
                _mainUI.ShowMessage(InfoMessage.Default);
                return;
            }
            else
            {
                FaceDetectionData faceData = JsonUtility.FromJson<FaceDetectionData>(json);
                if (faceData != null)
                {
                    _mainUI.ShowSmileDetectionResult(faceData.faceAttributes.smile);
                    Debug.Log(faceData.faceAttributes.smile);
                }
                else
                {
                    _mainUI.ShowMessage("Problem with parsing the results!");
                    Debug.LogWarning("Problem with parsing the results!");
                }
            }
        }

        public static string FormatJsonResult(byte[] data)
        {
            string jsonResult = System.Text.Encoding.UTF8.GetString(data);
            jsonResult = jsonResult.TrimStart('[').TrimEnd(']');
            Debug.Log(jsonResult);
            return jsonResult;
        }

        IEnumerator SendPhoto(string url, byte[] bodyBytes, Action<byte[]> ParseResults)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = bodyBytes;
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/octet-stream");
            request.SetRequestHeader("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                ParseResults(request.downloadHandler.data);
            }
        }
    }
}
