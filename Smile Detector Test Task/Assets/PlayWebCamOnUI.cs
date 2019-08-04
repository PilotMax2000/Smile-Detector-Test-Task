﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SmileDetectorTestTask
{
    public class PlayWebCamOnUI : MonoBehaviour
    {
        public RawImage _webCamRawImage;
        private WebCamTexture _webCamTexture;
        private PhotoSender _photoSender;

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
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //SaveImage();

                Texture2D texture = new Texture2D(_webCamRawImage.texture.width, _webCamRawImage.texture.height, TextureFormat.ARGB32, false);

                texture.SetPixels(_webCamTexture.GetPixels());
                texture.Apply();

                byte[] bytes = texture.EncodeToPNG();
                _photoSender.SendPhotoToServer(bytes);
            }
        }

        private void SaveImage()
        {
            Texture2D texture = new Texture2D(_webCamRawImage.texture.width, _webCamRawImage.texture.height, TextureFormat.ARGB32, false);

            texture.SetPixels(_webCamTexture.GetPixels());
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();

            File.WriteAllBytes(Application.dataPath + "/Photos/testing.png", bytes);
        }
    }

}