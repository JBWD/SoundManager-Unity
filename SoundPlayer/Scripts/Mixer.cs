using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace CCS.SoundPlayer
{
    [System.Serializable]
    public class Mixer
    {
        public MixerPlayer player = 0;
        public AudioMixer mixer;
        public string groupPrefix;
        public AudioMixerGroup[] mixerGroups { get; set; }
        private List<AudioSource> sources = new List<AudioSource>();
        private float currentVolume;
        private float startingVolume;
        private float sliderValue = 1;


        public Mixer(MixerPlayer player, AudioMixer mixer, string groupPrefix)
        {
            this.player = player;
            this.mixer = mixer;
            this.groupPrefix = groupPrefix;
        }

        
        /// <summary>
        /// Creates an AudioSource for every mixer group that as a child of the sent game object.
        /// </summary>
        /// <param name="gameObject"></param>
        public void InitializeMixer(GameObject gameObject)
        {
            mixer.GetFloat("MasterVolume", out startingVolume);
            foreach (AudioMixerGroup group in mixerGroups)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = group;
                sources.Add(source);
            }
        }
        

        /// <summary>
        /// Automatically adjusts master volume level of the mixer to avoid clipping. Runs in the Update function within the SoundManager.
        /// </summary>
        public void AutoAdjustMasterLevels()
        {
            int count = 0;
            foreach(AudioSource source in sources)
            {
                if (source.isPlaying)
                    count++;
            }
            if(count > 1)
            {
                AdjustMasterLevel(sliderValue,count * .03f);
            }
            else
            {
                AdjustMasterLevel(sliderValue);
            }
            
        }
        

        /// <summary>
        /// Changes the master mixer's volume level.
        /// </summary>
        public void AdjustMasterLevel(float volume)
        {
            sliderValue = volume;
            float currentVolume = startingVolume + Mathf.Log10(volume) * 20;
            if(currentVolume <-80)
            {
                currentVolume = -80;
            }
            mixer.SetFloat("MasterVolume", currentVolume);
        }

        private void AdjustMasterLevel(float volume, float adjustment)
        {
            sliderValue = volume;
            float currentVolume = startingVolume + Mathf.Log10(volume - adjustment) * 20;
            if (currentVolume < -80)
            {
                currentVolume = -80;
            }
            mixer.SetFloat("MasterVolume", currentVolume);
        }


        /// <summary>
        /// Adds an outside audiosouce that can be controlled by the SoundManager.
        /// </summary>
        /// <param name="source"></param>
        public void AddSource(AudioSource source)
        {
            if (source != null)
                sources.Add(source);
        }
        

        /// <summary>
        /// Returns a Mixer Group
        /// </summary>
        /// <returns></returns>
        public AudioMixerGroup GetMixerGroup()
        {
            if(mixerGroups.Length > 0)
            {
                return mixerGroups[0];
            }
            return null;
        }

        
        /// <summary>
        /// Plays a sound if a mixer is available at the selected pitch, use only if sounds are not commonly played on the mixer.
        /// Changing the pitch outside of the value 1 will result in slow or quicker playback.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        public void PlaySound(AudioClip sound, float pitch,bool enableLooping)
        {
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    source.loop = enableLooping;
                    source.pitch = pitch;
                    source.clip = sound;
                    source.Play();
                    break;
                }
            }
        }

        /// <summary>
        /// Combines the sounds within one mixer, tries to find the mixer with the closest pitch to play the sound.
        /// Changing the pitch outside of the value 1 will result in slow or quicker playback.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        public void PlayCombinedSound(AudioClip sound, float pitch)
        {
            //Checks to see if the source is not playing - automatically choses this option instead of stopping a sound.
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    source.pitch = pitch;
                    source.PlayOneShot(sound);
                    return;
                }

            }

            //Due to combining the sounds. The closest pitched player will be used.
            AudioSource sourceToUse = null;
            foreach (AudioSource source in sources)
            {
                if (sourceToUse == null || Mathf.Abs(sourceToUse.pitch - pitch) > Mathf.Abs(source.pitch - pitch))
                {
                    sourceToUse = source;
                }
            }
            if (sourceToUse == null)
                return;
            sourceToUse.PlayOneShot(sound);
        }


        /// <summary>
        /// Cancels the sound that is closest to ending and plays the new sound in its place.
        /// Changing the pitch outside of the value 1 will result in slow or quicker playback.
        /// Returns the AudioSource that is now currently playing the sent clip.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        public AudioSource OverridePlaySound(AudioClip sound, float pitch)
        {
            //Checks to see if the source is not playing - automatically choses this option instead of stopping a sound.
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    source.clip = sound;
                    source.pitch = pitch;
                    source.Play();
                    return source;
                }

            }

            //Due to all sources playing something - checks the sounds that is closest to ending and will stop that sound first.
            AudioSource sourceToUse = null;
            float time = 0;
            foreach (AudioSource source in sources)
            {
                if (sourceToUse == null || time < source.clip.length - source.time)
                {
                    sourceToUse = source;
                    time = source.clip.length - source.time;
                }
            }
            if (sourceToUse == null)
                return null;

            
            sourceToUse.Stop();
            sourceToUse.clip = sound;
            sourceToUse.pitch = pitch;
            sourceToUse.Play();
            return sourceToUse;
        }

        /// <summary>
        /// returns to the main thread within unity all of the sources that need to be muted overtime.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        /// <param name="overlayTime"></param>
        public AudioSource[] MuteAllOverlayPlaySound(AudioClip sound, float pitch)
        {
            //Setting our new sound to be playing on an empty or close to finished clip.
            AudioSource playingSource = OverridePlaySound(sound, pitch);
            List<AudioSource> sourcesToMute = new List<AudioSource>();
            foreach (AudioSource source in sources)
            {
                if (source != playingSource)
                {
                    sourcesToMute.Add(source);
                }
            }
            return sourcesToMute.ToArray();
        }

        /// <summary>
        /// returns to the main thread within unity all of the sources that need to be muted overtime.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        /// <param name="overlayTime"></param>
        public AudioSource[] MuteAllOverlayPlaySound(AudioClip sound, float pitch, bool enableLooping)
        {
            //Setting our new sound to be playing on an empty or close to finished clip.
            AudioSource playingSource = OverridePlaySound(sound, pitch);
            playingSource.loop = enableLooping;
            List<AudioSource> sourcesToMute = new List<AudioSource>();
            foreach (AudioSource source in sources)
            {
                if (source != playingSource)
                {
                    sourcesToMute.Add(source);
                    source.loop = false;
                }
            }
            return sourcesToMute.ToArray();
        }


        /// <summary>
        /// Pauses all sounds that are currently in the mixer.
        /// </summary>
        public void Pause()
        {
            foreach (AudioSource source in sources)
            {
                source.Pause();
            }
        }

        /// <summary>
        /// Resumes playing all sounds that are currently in the mixer
        /// </summary>
        public void Resume()
        {
            foreach (AudioSource source in sources)
            {
                source.UnPause();
            }
        }

        /// <summary>
        /// Changes the volume level off all sources
        /// </summary>
        public void AdjustVolume(float volumeLevel)
        {
            foreach (AudioSource source in sources)
            {
                source.volume = volumeLevel;
            }
        }

        /// <summary>
        /// mutes the audio for all sources.
        /// </summary>
        public void Mute()
        {
            foreach (AudioSource source in sources)
            {
                source.mute = true;
            }
        }

        /// <summary>
        /// unmutes the audio for all sources.
        /// </summary>
        public void UnMute()
        {
            foreach (AudioSource source in sources)
            {
                source.mute = false;
            }
        }

    }
}


