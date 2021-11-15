------------------------------------------------------------------
GUI Animator for Unity UI 1.2.0
------------------------------------------------------------------

GUI Animator for Unity UI is a very easy-to-use UI animations tool. Not needing to learn too much, you can be professional of UI Motion Tween. Using it you can not only animate GUI, but also manages Audio and Particles.

GUI Animator for Unity UI categorizes tween functions into Inspector tab. You can easily navigate to config Translation, Rotation, Scale, Fading, Time, Delay, Easing,  UnityEvents and Audio with ease.

GUI Animator for Unity UI is compatible with all Canvas Render and Canvas Scale Modes. It supports individual Sprite and Atlased Sprite elements. GUI Animator for Unity UI also supports UnityEvents , lets you manipulate animations via scripts. 

Try free version at https://assetstore.unity.com/packages/tools/gui/gui-animator-free-58843
Full version at https://assetstore.unity.com/packages/tools/gui/gui-animator-for-unity-ui-28709

------------------------------------------------------------------
Documentations
------------------------------------------------------------------ 

	ï Manual, see Manual.chm file.
	ï Class reference, see ClassReference.chm file.

	Note Windows users can use Microsoft Edge or other browsers to open chm file, for macOS users may require CMH Reader (https://itunes.apple.com/us/app/chm-reader/id424182884?mt=12) which is free.

------------------------------------------------------------------
Release notes
------------------------------------------------------------------ 

	Version 1.2.0

		ï Added: Use UnityEvents system instead of Callback.
		ï Added: New 29 demo scenes.
		ï Added: New Manual.chm and ClassReference.chm files as documentations instead of "!How to.txt" file.
		ï Fixed: Total bounds calculation (bug in some cases).
		ï Fixed: Initial position bugs.
		ï Removed: Javascript demo scenes.
		ï Removed: Callback system, use UnityEvents instead.
		ï Change: Refactored some methods of GAui class.
		ï Updated: Demo scenes and sample scripts.
		ï Updated: GSui is using new ParticleSystem methods instead of deprecated methods of Unity 5.x.
		ï Unity 2017.3.1 or higher compatible.

	Version 1.1.5

		ï Updated: DOTween 1.1.555 or higher compatible.
		ï Updated: LEAN Tween 2.44 or higher compatible.
		ï Updated: HOTween 1.3.035 or higher compatible.
		ï Updated: iTween 2.0.7 or higher compatible.
		ï Unity 5.5.1 or higher compatible.

	Version 1.1.1

		ï Fixed; UI game object does not appear when Fade-Out is enabled as Fade-In is disabled.
		ï Fixed; UI game object appears too early when Fade-In has a delay time.

	Version 1.1.0

		ï Added; Position-Loop animation.
		ï Added; GSui.Instance.Reset(Transform trans) method to reset any GAui element via script.
		ï Added; 2 more basic demo scenes for Position Loop animation. See "GUI Animator for Unity UI\Demo Basic (CSharp)\Scenes" folder.
		ï Changed; Friendly Inspector tabs are rearranged.
		ï Fixed; Rotate Loop animation does not play or broken in some cases.
		ï Fixed; Fade-Children does not work or work with wrong color in some cases.
		ï Fixed; Friendly Inspector does not change value in some cases.
		ï Improved; Idle-animations performance.
		ï Improved; Friendly Inspector is more stable with SerializedProperty and serializedObject.
		ï Updated; demo scenes and sample scripts.
		ï Unity 5.4.0 or higher compatible.

	Version 1.0.5

		ï Added Rotate Loop animation into Idle-animations.
		ï Fixed sometimes Idle-animations are not played, if there is no In-animations settings.
		ï Improved Idle-animations performance.
		ï Added 4 scenes for basic animation demo.
		ï Updated demo scenes and sample scripts.

	Version 1.0.1

		ï Fixed issue, DontDestroyOnLoad is called in edit mode.
		ï Uses "SceneManager.LoadScene" instead of "Application.LoadLevel" in Unity 5.3.4.
		ï Uses "ParticleSystem.emission.enable" instead of "ParticleSystem.denableEmission" in Unity 5.3.4.
		ï Update sample scripts.
		ï GETween, GUIAnimator and GUIAnimatorEditor dlls are built on Unity 5.3.4p2 dlls (UnityEngine.dll, UnityEngine.UI.dll, UnityEditor.dll).

	Version 1.0.0
	
		ï Fixed GSui_Object always disappear from Hierarchy tab.
		ï Able to check the UI element; no animate yet, animating or animated. (See "Checking GUI Animator status" section in "!How to.txt" file)
		ï No longer need GSuiEditor script file, old version users have to delete GSuiEditor script that found in Scripts/Editor.
		ï Unity 4.7.1 or higher compatible.
		ï Unity 5.3.4 or higher compatible.

	Version 0.9.95
	
		ï Fixed GUID conflict with other packages.
		ï Update Demo scenes and sample scripts.
		ï Change refactor some classes and variables.
		ï Change rearrange folders.
		ï Unity 4.6.9 or higher compatible.
		ï Unity 5.3.2 or higher compatible.

	Version 0.9.93

		ï Fixed "Coroutine couldn't be started" error occurs when user sets the GameObject to active/inactive while GEAnim is animating.

	Version 0.9.92c

		ï Change Canvas UI Scale Mode in all demo scenes to Scale With Screen Size.
		ï Change Some parameters of GEAnim in Callback demo scene (only in Unity 5.2.0).
		ï Fixed wrong folder names.
		ï Supports multiple version of Unity; Unity 4.6.0 or higher, Unity 5.0.0 or higher, Unity 5.2.0 or higher.

	Version 0.9.92

		ï Fixed GETween has memory leak.
		ï Fixed Friendly Inspector has Texture2D memory leak when user saves the scene.
		ï Fixed Wrong ease type convertion when use with LeanTween.
		ï Update smaller of GETween.dll file size.
		ï Update speed up Friendly Inspector.
		ï Unity 5.2.0 or higher compatible.

	Version 0.9.91

		ï Add Show/Hide icon in Hierarchy tab (it can be set in Friendly Inspector).
		ï Add Icon alignment in Hierarchy tab.
		ï Change Camera clear flags to solid color in all demo scenes.
		ï Fixed GUIAnim has protection errors when it works with DOTween, HOTween, LeanTween.
		ï Fixed User can not drop GameObject to callbacks in Friendly Inspector.
		ï Update DOTween 1.0.750 or higher compatible.
		ï Update LeanTween 2.28 or higher compatible.
		ï Unity 5.1.3 or higher compatible.

	Version 0.9.9

		ï Add "960x600px"ù to all demo scene names.
		ï Add 9 demo scenes for Javascript developers.
		ï Add 11 sample scripts for Javascript developers.
		ï Add Friendly Inspector for GUIAnimSystem and GEAnimSystem elements.
		ï Add GEAnim has new 10 override functions; Anim_In_MoveComplete(), Anim_In_RotateComplete(), Anim_In_ScaleComplete(), Anim_In_FadeComplete(), Anim_In_AllComplete(), Anim_Out_MoveComplete(), Anim_Out_RotateComplete(), Anim_Out_ScaleComplete(), Anim_Out_FadeComplete(), Anim_Out_AllComplete().
		ï Add Hide animation parameters in Friendly Inspector unless it has been enabled.
		ï Add GUIAnim and GEAnim can play In-Animations on Start() function ("On Start"ù parameter in Inspector tab).
		ï Add An option to disable/destroy GameObject after In-Animations completed ("On In-anims Complete" parameter in Inspector tab).
		ï Add An option to disable/destroy GameObject after Out-Animations completed ("On Out-anims Complete" parameter in Inspector tab).
		ï Add After Delay Sound into each animation, this will let user play AudioClip at right time after the delay.
		ï Add GUIAnimSystem and GEAnimSystem has MoveInAll() and MoveOutAll() functions to play all GUI Animator elements in the scenes.
		ï Add GUI Animator item shows mini-icon at the right edge of row in Inspector tab.
		ï Add GUIAnimSystem will be add into the scene automatically when GUIAnim or GEAnim component is added into a GameObject.
		ï Change Rename "Demo"ù folder to "Demo (CSharp)".
		ï Fixed Remove rotation and scale issues of In-Animation.
		ï Fixed Sometimes error happens when select GUIAnimSystem or GEAnimSystem object in Hierarchy tab.
		ï Fixed Remove minor known issues.
		ï Fixed GETween is more smooth.
		ï Update Sample Callback scene.

	Version 0.9.8

		ï Add Callback system for Move, Rotate, Scale, Fade.
		ï Add Callback demo scene.
		ï Fixed GUIAnimaEditor and GEAnimEditor components have duplicated parameters.
		ï Fixed The same field name is serialized multiple times in the class.
		ï Fixed Inspector tab, sometimes focus on wrong control when animation tab is changed in Friendly Inspector mode.
		ï Update UI layout of Friendly Inspector.

	Version 0.9.6

		ï Update Unity 5.0.1 or higher compatible.
		ï Update Support latest version of LeanTween and DOTween.
		ï Update Friendly Inspector.
		ï Fixed Wrong Move and Rotate animations.
		ï Fixed Known issues in version 0.8.4.
		ï Unity 5.0.1 or higher compatible.

	Version 0.8.4

		ï Add Unity 5.x.x compatible.
		ï Add DOTween (HOTween v2) compatible.
		ï Add Works with all Unity Canvas Render Modes.
		ï Add Works with all Unity Canvas UI Scale Modes.
		ï Add User can separately test MoveIn/Idle/MoveOut animations.
		ï Add User can set Idle time for Auto animation.
		ï Add Rotation In/Out.
		ï Add Begin/End Sounds to animations.
		ï Add Friendly inspector, all animation are categorised into tabs.
		ï Add Friendly inspector, show/hide Easing graphs.
		ï Add Friendly inspector, show/hide help boxes.
		ï Fixed Bugs and known issues in 0.8.3.
		ï Update Demo scenes and scripts.
		ï Unity 4.6.0 or higher compatible.

	Version 0.8.3 (Initial version, released on Jan 31, 2015)

		ï Unity 4.5.0 or higher compatible.