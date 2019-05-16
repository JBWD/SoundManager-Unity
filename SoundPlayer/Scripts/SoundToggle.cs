using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CCS.SoundPlayer
{
    [RequireComponent(typeof(Toggle))]
    public class SoundToggle : MonoBehaviour
    {
        private enum SoundToggleFunctions
        {
            Mute_Unmute,
            Pause_Resume,

        }


        private Toggle _toggle;
        [SerializeField]
        private SoundToggleFunctions function = SoundToggleFunctions.Mute_Unmute;
        [EnumFlags]
        [SerializeField]
        private MixerPlayer mixer = 0;
        [SerializeField]
        private bool reverse = false;

        // Start is called before the first frame update
        void Start()
        {
            _toggle = GetComponent<Toggle>();
            switch (function)
            {
                case SoundToggleFunctions.Mute_Unmute:
                    _toggle.onValueChanged.AddListener(delegate { MuteUnmute(); });
                    break;
                case SoundToggleFunctions.Pause_Resume:
                    _toggle.onValueChanged.AddListener(delegate { PauseResume(); });
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Communication function to tell what mixers need to be muted / unmuted.
        /// </summary>
        void MuteUnmute()
        {
            if (_toggle.isOn)
            {
                if (reverse)
                {
                    SoundManager.Instance.UnMute(mixer);
                }
                else
                {
                    SoundManager.Instance.Mute(mixer);
                }
            }
            else
            {
                if (reverse)
                {
                    SoundManager.Instance.Mute(mixer);
                }
                else
                {
                    SoundManager.Instance.UnMute(mixer);
                }
            }
        }

        /// <summary>
        /// Communication function to tell what mixers need to be paused / resumed.
        /// </summary>
        void PauseResume()
        {
            if (_toggle.isOn)
            {
                if (reverse)
                {
                    SoundManager.Instance.ResumeAudio(mixer);
                }
                else
                {
                    SoundManager.Instance.PauseAudio(mixer);
                }
            }
            else
            {
                if (reverse)
                {
                    SoundManager.Instance.PauseAudio(mixer);
                }
                else
                {
                    SoundManager.Instance.ResumeAudio(mixer);
                }
            }
        }


    }
}