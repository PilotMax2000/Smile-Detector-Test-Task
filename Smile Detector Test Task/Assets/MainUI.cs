using UnityEngine;
using TMPro;

namespace SmileDetectorTestTask
{
    public class MainUI : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private TextMeshProUGUI _statusText;
        // your code
#pragma warning restore 0649

        public void ShowMessage(string message)
        {
            _statusText.text = message;
        }
    }

}

