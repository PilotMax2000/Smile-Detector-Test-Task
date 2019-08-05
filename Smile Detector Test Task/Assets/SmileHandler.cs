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

        [Header("Messages")]
        [SerializeField] private string _smileDetectedMessage = "Wow, what a great smile!";
        [SerializeField] private string _onlyFaceDetectedMessage = "We detected your face, but not your smile";
#pragma warning restore 0649
        private int _animSmileTriggerHash = Animator.StringToHash("SmileDetected");

        public void CheckSmileThreshold(float smileValue)
        {
            if (smileValue > _smileValueThreshold)
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
            _mainUI.ShowMessage(_smileDetectedMessage);
            _reactionCharAnim.SetTrigger(_animSmileTriggerHash);
        }
    }

}
