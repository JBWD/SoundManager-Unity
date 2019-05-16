using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using CCS.EnumerationFlags;

namespace CCS.SoundPlayer
{
    public enum MixerPlayer
    {
        Explosions = 1,
        Instantiations = 2,
        Movement = 4,
        Music = 8,
        Interactions = 16,
        Dialog = 32
    }



    public class SoundManager : MonoBehaviour
    {

        #region Singleton
        public static SoundManager Instance;

        private void Singleton()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
        }

#endregion
        private Mixer[] mixers;
        private string[] mixerPrefixes=
            {"Explosion","Instantiation","Movement","Music","Interaction","Dialog" };
        private MixerPlayer[] mixerTypes =
            {MixerPlayer.Explosions,MixerPlayer.Instantiations,MixerPlayer.Movement,MixerPlayer.Music,MixerPlayer.Interactions,MixerPlayer.Dialog};
        private string[] mixerResourceNames =
            {"Explosions","Instantiations","Movement","Music","Interactions","Dialog" };

        /// <summary>
        /// Initializing the Sound Manager.
        /// </summary>
        private void Awake()
        {
            Singleton();
            mixers = new Mixer[mixerPrefixes.Length];
            for(int i = 0;i<mixerPrefixes.Length;i++)
            {
                mixers[i] = new Mixer(mixerTypes[i],(AudioMixer)Resources.Load("Mixers/"+mixerResourceNames[i]),mixerPrefixes[i]);

            }


            foreach (Mixer m in mixers)
            {
                m.mixerGroups = m.mixer.FindMatchingGroups(m.groupPrefix);
                
                m.InitializeMixer(gameObject);
                
            }

        }



        /// <summary>
        /// Returns the mixer that is associate with that player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Mixer GetMixer(MixerPlayer player)
        {
            foreach(Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {
                    return m;
                }
            }
            return null;
        }

        public void Update()
        {
            foreach(Mixer m in mixers)
            {
                m.AutoAdjustMasterLevels();
            }
        }

        /// <summary>
        /// Adds an outside audio source to the desired player to provide control over that source.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        public void AddAudioSourceToMixer(MixerPlayer player, AudioSource source)
        {
            foreach(Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {
                    m.AddSource(source);
                    return;
                }
            }
        }


        /// <summary>
        /// Plays a sound a designated location by creating a new audioSource, can be automatically added to a player for additional control.
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="location"></param>
        public void PlaySoundAtLocation(MixerPlayer player, AudioClip sound, Vector3 location)
        {
            GameObject obj = new GameObject();
            obj = Instantiate(obj, location, Quaternion.identity);
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound;
            Mixer m = GetMixer(player);
            if(m != null)
            {
                AudioMixerGroup group = m.GetMixerGroup();
                if(group != null)
                {
                    source.outputAudioMixerGroup = group;
                }
            }
            source.Play();
        }


        /// <summary>
        /// Plays the sound in an available mixer.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        public void PlaySound(MixerPlayer player, AudioClip sound)
        {
            PlaySound(player, sound, 1,false);
        }

        /// <summary>
        /// Plays the sound in an available mixer.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        public void PlaySound(MixerPlayer player, AudioClip sound,float pitch)
        {
            PlaySound(player, sound, pitch, false);
        }


        /// <summary>
        /// Plays the sound in an available mixer with the desired pitch.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        public void PlaySound(MixerPlayer player, AudioClip sound, float pitch,bool enableLooping)
        {
            foreach (Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {
                    m.PlaySound(sound, pitch,enableLooping);
                }
            }

        }
      
        /// <summary>
        /// Will cancel or close any existing sound based on the shortest time left in one of the mixers. Will also adjust the mixer to the desired pitch.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        public void PlaySoundOverride(MixerPlayer player, AudioClip sound, float pitch)
        {
            foreach (Mixer m in mixers)
            {

                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {
                    m.OverridePlaySound(sound, pitch);
                }
            }
        }


        /// <summary>
        /// Plays a Combined sound into one of the mixer groups, will try to find a group that is using the closest pitch.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        public void PlaySoundCombined(MixerPlayer player, AudioClip sound, float pitch)
        {
            foreach (Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {
                    m.PlayCombinedSound(sound, pitch);
                }
            }
        }


        /// <summary>
        /// Stops all other mixer channel's sounds over the sent time and plays the AudioClip at the designated pitch.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        /// <param name="seconds"></param>
        public void TransitionSound(MixerPlayer player, AudioClip sound, float pitch, float seconds)
        {
            TransitionSound(player, sound, pitch, seconds, false);
        }

        /// <summary>
        /// Stops all other mixer channels sounds over the sent time and plays the AudioClip at the designated pitch and allows looping.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="pitch"></param>
        /// <param name="seconds"></param>
        public void TransitionSound(MixerPlayer player, AudioClip sound, float pitch, float seconds, bool enableLooping)
        {
            foreach (Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {

                    AudioSource[] sources = m.MuteAllOverlayPlaySound(sound, pitch, enableLooping);
                    foreach (AudioSource source in sources)
                    {
                        StartCoroutine(MuteSourceOverTime(source, seconds));
                    }

                }
            }
        }

        /// <summary>
        /// Slowly reduces an audio sources volume over the sent time. Will reset the volume to its initial value when complete and stops the clip.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        IEnumerator MuteSourceOverTime(AudioSource source, float seconds)
        {
            float startingVolume = source.volume;
            while (source.volume > .01f)
            {
                source.volume -= startingVolume / seconds * .1f;
                yield return new WaitForSeconds(.1f);
            }
            source.Stop();
            source.volume = startingVolume;
        }


        /// <summary>
        /// Mutes every mixer that is within the sound manager.
        /// </summary>
        public void Mute()
        {
            foreach (Mixer m in mixers)
            {

                m.Mute();
            }
        }

        /// <summary>
        /// Mutes the sent mixer only.
        /// </summary>
        /// <param name="player"></param>
        public void Mute(MixerPlayer player)
        {

            foreach (Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                    m.Mute();
            }
        }

        /// <summary>
        /// Unmutes all mixers that are within the sound manager.
        /// </summary>
        public void UnMute()
        {
            foreach (Mixer m in mixers)
            {

                m.UnMute();
            }
        }

        /// <summary>
        /// Unmutes the sent mixer.
        /// </summary>
        /// <param name="player"></param>
        public void UnMute(MixerPlayer player)
        {

            foreach (Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                    m.UnMute();
            }
        }

        /// <summary>
        /// Adjusts the volume of all of the mixers.
        /// </summary>
        /// <param name="volumeLevel"></param>
        public void AdjustMasterVolume(float volumeLevel)
        {
            
            foreach (Mixer m in mixers)
            {
                m.AdjustMasterLevel(volumeLevel);
            }
        }


        /// <summary>
        /// Adjusts the volume of the sent mixer.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="volumeLevel"></param>
        public void AdjustVolume(MixerPlayer player, float volumeLevel)
        {
            if (EnumFlags<MixerPlayer>.HasAllFlags(player, MixerPlayer.Dialog , MixerPlayer.Explosions , MixerPlayer.Instantiations , MixerPlayer.Interactions , MixerPlayer.Movement , MixerPlayer.Music))
            {
                AdjustMasterVolume(volumeLevel);
                return;
            }

            foreach (Mixer m in mixers)
            {
                if (EnumFlags<MixerPlayer>.HasFlag(m.player, player))
                {

                    m.AdjustVolume(volumeLevel);
                }

            }
        }



        /// <summary>
        /// Pauses the current music / sound for all mixers.
        /// </summary>
        public void PauseAudio()
        {
            foreach (Mixer m in mixers)
            {
                m.Pause();
            }
        }

        /// <summary>
        /// Pauses the current music / sound for the sent mixer.
        /// </summary>
        /// <param name="player"></param>
        public void PauseAudio(MixerPlayer player)
        {

            foreach (Mixer m in mixers)
            {
                if (m.player == player)
                {
                    m.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes the current music / sound for all the mixers.
        /// </summary>
        public void ResumeAudio()
        {
            foreach (Mixer m in mixers)
            {
                m.Resume();
            }
        }

        /// <summary>
        /// Resumes the current music / sound for the sent mixer.
        /// </summary>
        /// <param name="player"></param>
        public void ResumeAudio(MixerPlayer player)
        {

            foreach (Mixer m in mixers)
            {
                if (m.player == player)
                {
                    m.Resume();
                }
            }
        }
    }
}
