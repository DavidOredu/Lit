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

	� Manual, see Manual.chm file.
	� Class reference, see ClassReference.chm file.

	Note Windows users can use Microsoft Edge or other browsers to open chm file, for macOS users may require CMH Reader (https://itunes.apple.com/us/app/chm-reader/id424182884?mt=12) which is free.

------------------------------------------------------------------
Release notes
------------------------------------------------------------------ 

	Version 1.2.0

		� Added: Use UnityEvents system instead of Callback.
		� Added: New 29 demo scenes.
		� Added: New Manual.chm and ClassReference.chm files as documentations instead of "!How to.txt" file.
		� Fixed: Total bounds calculation (bug in some cases).
		� Fixed: Initial position bugs.
		� Removed: Javascript demo scenes.
		� Removed: Callback system, use UnityEvents instead.
		� Change: Refactored some methods of GAui class.
		� Updated: Demo scenes and sample scripts.
		� Updated: GSui is using new ParticleSystem methods instead of deprecated methods of Unity 5.x.
		� Unity 2017.3.1 or higher compatible.

	Version 1.1.5

		� Updated: DOTween 1.1.555 or higher compatible.
		� Updated: LEAN Tween 2.44 or higher compatible.
		� Updated: HOTween 1.3.035 or higher compatible.
		� Updated: iTween 2.0.7 or higher compatible.
		� Unity 5.5.1 or higher compatible.

	Version 1.1.1

		� Fixed; UI game object does not appear when Fade-Out is enabled as Fade-In is disabled.
		� Fixed; UI game object appears too early when Fade-In has a delay time.

	Version 1.1.0

		� Added; Position-Loop animation.
		� Added; GSui.Instance.Reset(Transform trans) method to reset any GAui element via script.
		� Added; 2 more basic demo scenes for Position Loop animation. See "GUI Animator for Unity UI\Demo Basic (CSharp)\Scenes" folder.
		� Changed; Friendly Inspector tabs are rearranged.
		� Fixed; Rotate Loop animation does not play or broken in some cases.
		� Fixed; Fade-Children does not work or work with wrong color in some cases.
		� Fixed; Friendly Inspector does not change value in some cases.
		� Improved; Idle-animations performance.
		� Improved; Friendly Inspector is more stable with SerializedProperty and serializedObject.
		� Updated; demo scenes and sample scripts.
		� Unity 5.4.0 or higher compatible.

	Version 1.0.5

		� Added Rotate Loop animation into Idle-animations.
		� Fixed sometimes Idle-animations are not played, if there is no In-animations settings.
		� Improved Idle-animations performance.
		� Added 4 scenes for basic animation demo.
		� Updated demo scenes and sample scripts.

	Version 1.0.1

		� Fixed issue, DontDestroyOnLoad is called in edit mode.
		� Uses "SceneManager.LoadScene" instead of "Application.LoadLevel" in Unity 5.3.4.
		� Uses "ParticleSystem.emission.enable" instead of "ParticleSystem.denableEmission" in Unity 5.3.4.
		� Update sample scripts.
		� GETween, GUIAnimator and GUIAnimatorEditor dlls are built on Unity 5.3.4p2 dlls (UnityEngine.dll, UnityEngine.UI.dll, UnityEditor.dll).

	Version 1.0.0
	
		� Fixed GSui_Object always disappear from Hierarchy tab.
		� Able to check the UI element; no animate yet, animating or animated. (See "Checking GUI Animator status" section in "!How to.txt" file)
		� No longer need GSuiEditor script file, old version users have to delete GSuiEditor script that found in Scripts/Editor.
		� Unity 4.7.1 or higher compatible.
		� Unity 5.3.4 or higher compatible.

	Version 0.9.95
	
		� Fixed GUID conflict with other packages.
		� Update Demo scenes and sample scripts.
		� Change refactor some classes and variables.
		� Change rearrange folders.
		� Unity 4.6.9 or higher compatible.
		� Unity 5.3.2 or higher compatible.

	Version 0.9.93

		� Fixed "Coroutine couldn't be started" error occurs when user sets the GameObject to active/inactive while GEAnim is animating.

	Version 0.9.92c

		� Change Canvas UI Scale Mode in all demo scenes to Scale With Screen Size.
		� Change Some parameters of GEAnim in Callback demo scene (only in Unity 5.2.0).
		� Fixed wrong folder names.
		� Supports multiple version of Unity; Unity 4.6.0 or higher, Unity 5.0.0 or higher, Unity 5.2.0 or higher.

	Version 0.9.92

		� Fixed GETween has memory leak.
		� Fixed Friendly Inspector has Texture2D memory leak when user saves the scene.
		� Fixed Wrong ease type convertion when use with LeanTween.
		� Update smaller of GETween.dll file size.
		� Update speed up Friendly Inspector.
		� Unity 5.2.0 or higher compatible.

	Version 0.9.91

		� Add Show/Hide icon in Hierarchy tab (it can be set in Friendly Inspector).
		� Add Icon alignment in Hierarchy tab.
		� Change Camera clear flags to solid color in all demo scenes.
		� Fixed GUIAnim has protection errors when it works with DOTween, HOTween, LeanTween.
		� Fixed User can not drop GameObject to callbacks in Friendly Inspector.
		� Update DOTween 1.0.750 or higher compatible.
		� Update LeanTween 2.28 or higher compatible.
		� Unity 5.1.3 or higher compatible.

	Version 0.9.9

		� Add "960x600px"� to all demo scene names.
		� Add 9 demo scenes for Javascript developers.
		� Add 11 sample scripts for Javascript developers.
		� Add Friendly Inspector for GUIAnimSystem and GEAnimSystem elements.
		� Add GEAnim has new 10 override functions; Anim_In_MoveComplete(), Anim_In_RotateComplete(), Anim_In_ScaleComplete(), Anim_In_FadeComplete(), Anim_In_AllComplete(), Anim_Out_MoveComplete(), Anim_Out_RotateComplete(), Anim_Out_ScaleComplete(), Anim_Out_FadeComplete(), Anim_Out_AllComplete().
		� Add Hide animation parameters in Friendly Inspector unless it has been enabled.
		� Add GUIAnim and GEAnim can play In-Animations on Start() function ("On Start"� parameter in Inspector tab).
		� Add An option to disable/destroy GameObject after In-Animations completed ("On In-anims Complete" parameter in Inspector tab).
		� Add An option to disable/destroy GameObject after Out-Animations completed ("On Out-anims Complete" parameter in Inspector tab).
		� Add After Delay Sound into each animation, this will let user play AudioClip at right time after the delay.
		� Add GUIAnimSystem and GEAnimSystem has MoveInAll() and MoveOutAll() functions to play all GUI Animator elements in the scenes.
		� Add GUI Animator item shows mini-icon at the right edge of row in Inspector tab.
		� Add GUIAnimSystem will be add into the scene automatically when GUIAnim or GEAnim component is added into a GameObject.
		� Change Rename "Demo"� folder to "Demo (CSharp)".
		� Fixed Remove rotation and scale issues of In-Animation.
		� Fixed Sometimes error happens when select GUIAnimSystem or GEAnimSystem object in Hierarchy tab.
		� Fixed Remove minor known issues.
		� Fixed GETween is more smooth.
		� Update Sample Callback scene.

	Version 0.9.8

		� Add Callback system for Move, Rotate, Scale, Fade.
		� Add Callback demo scene.
		� Fixed GUIAnimaEditor and GEAnimEditor components have duplicated parameters.
		� Fixed The same field name is serialized multiple times in the class.
		� Fixed Inspector tab, sometimes focus on wrong control when animation tab is changed in Friendly Inspector mode.
		� Update UI layout of Friendly Inspector.

	Version 0.9.6

		� Update Unity 5.0.1 or higher compatible.
		� Update Support latest version of LeanTween and DOTween.
		� Update Friendly Inspector.
		� Fixed Wrong Move and Rotate animations.
		� Fixed Known issues in version 0.8.4.
		� Unity 5.0.1 or higher compatible.

	Version 0.8.4

		� Add Unity 5.x.x compatible.
		� Add DOTween (HOTween v2) compatible.
		� Add Works with all Unity Canvas Render Modes.
		� Add Works with all Unity Canvas UI Scale Modes.
		� Add User can separately test MoveIn/Idle/MoveOut animations.
		� Add User can set Idle time for Auto animation.
		� Add Rotation In/Out.
		� Add Begin/End Sounds to animations.
		� Add Friendly inspector, all animation are categorised into tabs.
		� Add Friendly inspector, show/hide Easing graphs.
		� Add Friendly inspector, show/hide help boxes.
		� Fixed Bugs and known issues in 0.8.3.
		� Update Demo scenes and scripts.
		� Unity 4.6.0 or higher compatible.

	Version 0.8.3 (Initial version, released on Jan 31, 2015)

		� Unity 4.5.0 or higher compatible.