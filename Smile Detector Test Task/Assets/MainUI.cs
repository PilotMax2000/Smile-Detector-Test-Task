using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _statusText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
