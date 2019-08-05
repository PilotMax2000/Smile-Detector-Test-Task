using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SmileDetectorTestTask
{
    public class PlayWebCamOnUI : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private RawImage _webCamRawImage;
#pragma warning restore 0649
        [SerializeField] [Range(3.0f, 10.0f)] private float _waitBeforeNextShot = 4f;
        private WebCamTexture _webCamTexture;
        private PhotoSender _photoSender;
        private bool _pause;


        private void Awake()
        {
            _photoSender = GetComponent<PhotoSender>();
        }

        private void Start()
        {
            _webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name, 1280, 720, 30);
            _webCamRawImage.texture = _webCamTexture;
            _webCamRawImage.material.mainTexture = _webCamTexture;
            _webCamTexture.Play();

            StartCoroutine(MakeNextShot());

        }

        public void SetPauseAfterSuccessSmileShot(bool setPause)
        {
            _pause = setPause;
            if(setPause == false)
            {
                StartCoroutine(MakeNextShot());
            }
        }

        private IEnumerator MakeNextShot()
        {
            while (_pause == false)
            {
                yield return new WaitForSeconds(_waitBeforeNextShot);
                SendPhotoToServer();
            }
        }



        private void SendPhotoToServer()
        {
            Texture2D texture = new Texture2D(_webCamRawImage.texture.width, _webCamRawImage.texture.height, TextureFormat.ARGB32, false);

            texture.SetPixels(_webCamTexture.GetPixels());
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();
            _photoSender.SendPhotoToServer(bytes);
        }

        private void SaveImage()
        {
            Texture2D texture = new Texture2D(_webCamRawImage.texture.width, _webCamRawImage.texture.height, TextureFormat.ARGB32, false);

            texture.SetPixels(_webCamTexture.GetPixels());
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();
            Directory.CreateDirectory(Application.dataPath + "/Photos");
            File.WriteAllBytes(Application.dataPath + "/Photos/testing.png", bytes);
        }
    }

}
