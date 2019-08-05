using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileDetectorTestTask
{
    public class SmileHandler : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 1)] private float _smileValueThreshold = 0.6f;
#pragma warning disable 0649
        [SerializeField] private MainUI _mainUI;
        [SerializeField] private Animator _reactionCharAnim;
        [SerializeField] private PlayWebCamOnUI _webCam;
        [SerializeField] private float _waitAfterSmileDetected = 1.5f;

        [Header("Messages")]
        [SerializeField] private string _smileDetectedMessage = "Wow, what a great smile!";
        [SerializeField] private string _onlyFaceDetectedMessage = "We detected your face, but not your smile";
        [SerializeField] private string _smileAndWaitMessage = "Smile and wait for the result!";
        [SerializeField] private string _photoSavedMessage = "Photo was saved! Prepare for a new shot.";
#pragma warning restore 0649
        private int _animSmileTriggerHash = Animator.StringToHash("SmileDetected");
        private bool _smileDetectedSuccessfully;

        public void CheckSmileThreshold(float smileValue)
        {
            if(_smileDetectedSuccessfully)
            {
                return;
            }

            if(smileValue < 0)
            {
                _mainUI.ShowMessage(_smileAndWaitMessage);
            }
            else if (smileValue > _smileValueThreshold)
            {
                SmileWasDetected();
            }
            else
            {
                _mainUI.ShowMessage(_onlyFaceDetectedMessage);
            }
        }

        private void SmileWasDetected()
        {
            _smileDetectedSuccessfully = true;
            _webCam.SetPauseAfterSuccessSmileShot(_smileDetectedSuccessfully);
            StartCoroutine(MakePauseAfterSuccessfulDetection());
            _reactionCharAnim.SetTrigger(_animSmileTriggerHash);
        }

        private IEnumerator MakePauseAfterSuccessfulDetection()
        {
            _mainUI.ShowMessage(_smileDetectedMessage);
            yield return new WaitForSeconds(_waitAfterSmileDetected);
            _mainUI.ShowMessage(_photoSavedMessage);
            yield return new WaitForSeconds(_waitAfterSmileDetected);
            _mainUI.ShowMessage(_smileAndWaitMessage);
            yield return new WaitForSeconds(1.0f);
            _smileDetectedSuccessfully = false;
            _webCam.SetPauseAfterSuccessSmileShot(_smileDetectedSuccessfully);

        }
    }

}
