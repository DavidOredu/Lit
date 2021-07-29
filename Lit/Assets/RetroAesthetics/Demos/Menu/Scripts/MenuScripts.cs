﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroAesthetics.Demos {

	public class MenuScripts : MonoBehaviour {
		public SceneField loadingScene;
		public SceneField levelScene;

		public bool fadeInMenu = true;
		public bool fadeOutMenu = true;

		private RetroCameraEffect _cameraEffect;
		private AsyncOperation _loadingSceneAsync;

		void Start() {
			if (fadeInMenu) {
				_cameraEffect = GameObject.FindObjectOfType<RetroCameraEffect>();
				if (_cameraEffect != null) {
					_cameraEffect.FadeIn();
				}
			}
		}

		virtual public void StartLevel() {
			if (levelScene != null) {
				if (_cameraEffect != null) {
					if (loadingScene != null) {
						_loadingSceneAsync = SceneManager.LoadSceneAsync(loadingScene);
						if (_loadingSceneAsync == null) {
							Debug.LogWarning(string.Format(
								"Please add scene `{0}` to the built scenes in the Build Settings.",
								loadingScene.SceneName));
							return;
						}
						_loadingSceneAsync.allowSceneActivation = false; 
					}
				
					_cameraEffect.FadeOut(0.5f, LoadNextScene);
				} else {
					LoadNextScene();
				}
			} else {
				Debug.LogWarning("Level scene is not set.");
			}
		}

		private void LoadNextScene() {
			if (_loadingSceneAsync != null) {
				_loadingSceneAsync.allowSceneActivation = true;
			}
			SceneManager.LoadSceneAsync(levelScene);
		}
	}
}
