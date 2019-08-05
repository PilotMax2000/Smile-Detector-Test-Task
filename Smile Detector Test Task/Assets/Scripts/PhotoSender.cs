using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace SmileDetectorTestTask
{
    public class PhotoSender : MonoBehaviour
    {

#pragma warning disable 0649
        [SerializeField] private SmileHandler _smileHandler;
#pragma warning restore 0649
        private const string SUBSCRIPTION_KEY = "53db09b4f6df468eadb20a52b7de7d5c";
        private const string BASE_URL = "https://northeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        private const string REQUEST_PARAMS = "?returnFaceAttributes=smile&recognitionModel=recognition_01&detectionModel=detection_01";

        public void SendPhotoToServer(byte[] photo)
        {
            StartCoroutine(SendPhoto(BASE_URL + REQUEST_PARAMS, photo, FaceDataParser));
        }

        private void FaceDataParser(byte[] data, byte[] photo)
        {
            string json = FormatJsonResult(data);
            if (string.IsNullOrEmpty(json))
            {
                _smileHandler.CheckSmileThreshold(-1);
                return;
            }
            else
            {
                FaceDetectionData faceData = JsonUtility.FromJson<FaceDetectionData>(json);
                if (faceData != null)
                {
                    _smileHandler.CheckSmileThreshold(faceData.faceAttributes.smile, photo);
                }
                else
                {
                    Debug.LogWarning("Problem with parsing the results!");
                }
            }
        }

        public static string FormatJsonResult(byte[] data)
        {
            string jsonResult = System.Text.Encoding.UTF8.GetString(data);
            jsonResult = jsonResult.TrimStart('[').TrimEnd(']');
            return jsonResult;
        }

        IEnumerator SendPhoto(string url, byte[] photo, Action<byte[],byte[]> callback)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = photo;
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
                callback(request.downloadHandler.data, photo);
            }
        }
    }
}
