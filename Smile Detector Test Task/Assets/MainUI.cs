using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private string _defaultMessage = "Smile and wait!";
    // Start is called before the first frame update

    public void ShowSmileDetectionResult(double smileValue )
    {
        if(smileValue > 0.8)
        {
            _statusText.text = "Wow, what a great smile!";
        }
        else
        {
            _statusText.text = "We detected your face, but not your smile. Come on, we are wating!";
        }
    }

    public void ShowMessage(string message)
    {
        _statusText.text = message;
    }

    public void ShowMessage(InfoMessage m)
    {
        switch(m)
        {
            case InfoMessage.Default:
                _statusText.text = _defaultMessage;
                break;
        }
    }
}

public enum InfoMessage {Default}