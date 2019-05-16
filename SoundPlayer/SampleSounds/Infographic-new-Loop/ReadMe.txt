Author: Weston Glasser
Company: Card Castle Studio
Date: May 14, 2019
GIT Repository: 


Documentation Website:


How To Videos:


Quick Notes:

	- Setting up the Sound Manager

		The SoundManager is a prefab object that will be avaiable within the SoundPlayer/Prefab folder.

		It will have 2 Mixers associated within it: DestructionEvents and BackgroundMusic along with what player they are associated with.

		If more Mixer for your game are required increase the size of the Mixers array and follow the step below for creating a new AudioMixer.



	- Creating a new AudioMixer

		Each mixer will contain a group of children that are able to play, modify and combine sounds using AudioSources. 
		These will all go under a naming scheme with the same Prefix. This means that you can name each child anything you'd like
		but will need to keep the beginning of each of them the same.

		Example:
		- Explosion1
		- ExplosionSimple
		- ExplosionComplex
		- ExplosionSuperman

		The prefix of the 'Example' will be "Explosion" and when setting up the SoundManager will be the string that is used
		within the GroupPrefix textbox.



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
