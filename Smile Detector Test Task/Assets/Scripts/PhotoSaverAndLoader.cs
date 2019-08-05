using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace SmileDetectorTestTask
{
    public class PhotoSaverAndLoader : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private GameObject _photoPrefab;
        [SerializeField] private Transform _parent;
#pragma warning restore 0649
        private string[] _files;
        private const string PHOTOS_FOLDER = "/Photos/";

        void Start()
        {
            string path = Application.dataPath + PHOTOS_FOLDER;

            if (Directory.Exists(path))
            {
                _files = System.IO.Directory.GetFiles(path, "*.png");
                LoadImages();
            }
        }

        private void LoadImages()
        {
            Texture2D tex = null;
            byte[] fileData;
            foreach (string texPath in _files)
            {

                if (File.Exists(texPath))
                {
                    fileData = File.ReadAllBytes(texPath);
                    tex = new Texture2D(2, 2);
                    tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                    GameObject test = Instantiate(_photoPrefab, _parent);
                    RawImage testRaw = test.GetComponent<RawImage>();
                    testRaw.texture = tex;
                }
            }
        }
        
        public void SavePhoto(byte[] photo)
        {

            Directory.CreateDirectory(Application.dataPath + PHOTOS_FOLDER);
            string name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            File.WriteAllBytes(Application.dataPath + PHOTOS_FOLDER + name, photo);
        }
    }

}

