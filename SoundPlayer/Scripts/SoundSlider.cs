using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CCS.SoundPlayer
{
    [RequireComponent(typeof(Slider))]
    public class SoundSlider : MonoBehaviour
    {
        private enum SoundSliderFunction
        {
            AdjustVolume,
        }


        private Slider _slider;
        [SerializeField]
        private SoundSliderFunction function = SoundSliderFunction.AdjustVolume;
        [EnumFlags]
        [SerializeField]
        private MixerPlayer mixer = 0;


        // Start is called before the first frame update
        void Start()
        {
            _slider = GetComponent<Slider>();
            if (SoundManager.Instance == null)
            {
                Debug.LogWarning("Please make sure there is a SoundManager script attached to a GameObject.");
                return;
            }
            switch (function)
            {
                case SoundSliderFunction.AdjustVolume:
                    _slider.onValueChanged.AddListener(delegate { AdjustVolume(); });
                    break;
                default:
                    break;
            }
        }



        public void AdjustVolume()
        {
            SoundManager.Instance.AdjustVolume(mixer, _slider.value);
        }

    }

}