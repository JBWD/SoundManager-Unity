# Sound Manager

### The sound manager is a complete 2D game sound management system.

### Features:
* Seemless music transitions
* Play 100s of sounds without clipping
* Call from any MonoBehaviour script
* Toggle, Slider and Button support
* Add and control new audio sources dynamically
* Easy integration into any existing project
* Automatic volume regulation
* Play sounds at desired locations with controlled volume and pitch
* Feel Free to add more Features!

### Integration

Due to the SoundManager using a singleton playing a sound is as easy as:

```
SoundManager.Instance.PlaySound(MixerPlayer.Music, 'musicAudioClip');
```

However we recommend the use of our more robust functions that allow for audio transitioning and overlapping. These will look like:

```
SoundManager.Instance.TransitionSound(MixerPlayer.Music, 'musicAudioClip', 'pitchLevel', 'transitionTime');

SoundManager.Instance.PlaySoundCombined(MixerPlayer.Music, 'musicAudioClip', 'pitchLevel');
```

### Toggle, Slider, and Button

Due to most menus using toggles, sliders and buttons the functionality was added in to control certain mixers automatically.

Under your associated GameObjects click 'AddComponent' and type in any of the following:

```
'SoundToggle'
'SoundSlider'
'SoundButton'
```

These will have built in functions that are applied on startup to control the corresponding mixers.

![alt text](https://user-images.githubusercontent.com/35278058/57865550-664a8a80-77c3-11e9-8a32-bcf2c783cbd7.PNG "SoundSlider")
