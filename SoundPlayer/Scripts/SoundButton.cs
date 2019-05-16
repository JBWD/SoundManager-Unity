using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CCS.SoundPlayer
{
    [RequireComponent(typeof(Button))]
    public class SoundButton : MonoBehaviour
    {
        private enum ButtonSoundFunction
        {
            Mute,
            UnMute,
            Pause,
            Resume,
            PlaySound
        }

        private Button _button;
        [SerializeField]
        private ButtonSoundFunction function = ButtonSoundFunction.Mute;
        [SerializeField]
        private MixerPlayer mixer  = 0;
        [SerializeField]
        private AudioClip sound = null;

        public float lowerPitch =.5f;
        public float upperPitch = 1.25f;


        // Start is called before the first frame update
        void Start()
        {
            _button = GetComponent<Button>();

            if (SoundManager.Instance == null)
            {
                Debug.LogWarning("Please make sure there is a SoundManager script attached to a GameObject.");
                return;
            }
            switch (function)
            {
                case ButtonSoundFunction.Mute:
                    Mute();
                    break;
                case ButtonSoundFunction.UnMute:
                    UnMute();
                    break;
                case ButtonSoundFunction.Pause:
                    Pause();
                    break;
                case ButtonSoundFunction.Resume:
                    Resume();
                    break;
                case ButtonSoundFunction.PlaySound:
                    PlaySound();
                    break;
                default:
                    break;
            }
        }

        public void Mute()
        {
            _button.onClick.AddListener(() => SoundManager.Instance.Mute());
        }

        public void UnMute()
        {
            _button.onClick.AddListener(() => SoundManager.Instance.UnMute());
        }
        public void Pause()
        {
            _button.onClick.AddListener(() => SoundManager.Instance.PauseAudio());
        }
        public void Resume()
        {
            _button.onClick.AddListener(() => SoundManager.Instance.ResumeAudio());
            
        }
        public void PlaySound()
        {
           
            _button.onClick.AddListener(() => SoundManager.Instance.PlaySoundCombined(mixer, sound, Random.Range(lowerPitch, upperPitch)));
        }

        IEnumerator PlayOver()
        {
            yield return new WaitForSeconds(4);
            while (gameObject.activeInHierarchy) { 
                SoundManager.Instance.PlaySoundOverride(mixer, sound, Random.Range(lowerPitch, upperPitch));
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}