Author: Weston Glasser
Company: Card Castle Studio
Date: May 14, 2019
GIT Repository: 
https://github.com/JBWD/SoundManager-Unity/edit/Master/SoundPlayer/ReadMe.txt

How To Videos:
N/A

Quick Notes:

	- Setting up the Sound Manager

		Drag the prefab from the prefab folder within the SoundPlayer folder and it will work automatically.


	- Using the asset for UI

		There are 3 scripts that can be added to the following UI Elements:
			- Slider
			- Button
			- Toggle

		The Slider uses the 'SoundSlider' script and requires a Slider component and can be found using the AddComponent within the inspector.
		It has build in function(s) such as Adjusting Volume.
		
		The Button uses the 'SoundButton' script and requires a Button component and can be found using the AddComponent within the inspector.
		It has built in function(s) such as Mute, Unmute, Pause, Resume, PlaySound.
			
		The Toggle uses the 'SoundToggle' script and requires a Toggle component and can be found using the AddComponent within the inspector.
		It has built in function(s) such as Mute/Unmute, Pause/Resume.

		All of these elements can be associated with each mixer independently or effect multiple mixers at the same time.


	- Playing Sounds in your game
	
		Music: For music we recommend using the TransitionSound(), this will allow only one musical piece to play at a time and provide clean transitions between the songs.
		
		Sounds: There are mixers made for the majority of sounds and can always be added to. Playing on a certain mixer is as easy as PlaySound(MixerPlayer.music); this allows for complete customization of volume levels. We also recommend using the PlaySoundCombined() which will allow for multiple gunshots or explosions to occur at the same time.

If you have any issue please let us know on the Github page above.
