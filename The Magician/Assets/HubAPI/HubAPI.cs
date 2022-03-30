using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#else
using UnityEngine.EventSystems;
#endif

public class HubAPI : MonoBehaviour {

	/// <summary>
	/// Event that broadcasts when any of the three
	/// volume settings are changed (either on game
	/// start or from the pause menu)
	/// </summary>
	public static event Action OnVolumeChanged;
	/// <summary>
	/// Event that broadcasts when the game has been
	/// paused or resumed by the player. The provided
	/// boolean is TRUE if the game was paused, FALSE
	/// if the game was resumed.
	/// </summary>
	public static event Action<bool> OnGamePaused;

	/// <summary>
	/// Event that broadcasts when the user presses
	/// the Game Options button from the pause menu.
	/// Please note that you should call 'ResumePauseMenu'
	/// when the user is done setting your custom
	/// options.
	/// </summary>
	public static event Action OnGameOptionsOpened;

	/// <summary>
	/// Normalized (0-1) Sound volume provided by the pause
	/// menu and persists between plays.
	/// </summary>
	public static float SoundVolume {
		get { return m_singleton.m_settings.SoundVolume; }
	}
	/// <summary>
	/// Normalized (0-1) Music volume provided by the pause
	/// menu and persists between plays.
	/// </summary>
	public static float MusicVolume {
		get { return m_singleton.m_settings.MusicVolume; }
	}
	/// <summary>
	/// Normalized (0-1) Voice volume provided by the pause
	/// menu and persists between plays.
	/// </summary>
	public static float VoiceVolume {
		get { return m_singleton.m_settings.VoiceVolume; }
	}

	[SerializeField]
	private bool m_useCustomOptions = false;
	[SerializeField]
	private GameObject m_pauseCanvas = null;
	[SerializeField]
	private Slider m_sliderSound = null;
	[SerializeField]
	private Slider m_sliderMusic = null;
	[SerializeField]
	private Slider m_sliderVoice = null;
	[SerializeField]
	private Button m_buttonQuit = null;
	[SerializeField]
	private Button m_buttonResume = null;
	[SerializeField]
	private Button m_buttonGameOptions = null;

	private static HubAPI m_singleton;
	private Settings m_settings;
	private bool m_isPaused = false;
	private bool m_isInGameOptions = false;
#if ENABLE_INPUT_SYSTEM
	private bool m_wasEscapePressedLastFrame = false;
#endif


	public static void ResumePauseMenu() {
		if (m_singleton.m_isInGameOptions) {
			m_singleton.m_isInGameOptions = false;
			m_singleton.SetPaused(true, false);
		} else {
			Debug.LogWarning("The Game Options are not currently being " +
				"viewed. Make sure you only call 'ResumePauseMenu' when " +
				"you receive an 'OnGameOptions' opened event and the user " +
				"closes your custom options."
			);
		}
	}

	private void Awake() {
		if (m_singleton == null) {
			m_singleton = this;
			OnAwake();
		} else {
			Destroy(gameObject);
		}
	}

	private void OnAwake() {
		// make the API gameobject persistent
		DontDestroyOnLoad(gameObject);

		// load settings from json
		string path = GetSettingsPath();
		if (!File.Exists(path)) {
			File.WriteAllText(path,JsonUtility.ToJson(Settings.Default));
		}
		string json = File.ReadAllText(path);
		m_settings = JsonUtility.FromJson<Settings>(json);

		// add the proper input modules to the canvas
		// so that the event system can process input
#if ENABLE_INPUT_SYSTEM
		m_pauseCanvas.AddComponent<InputSystemUIInputModule>();
#else
		m_pauseCanvas.AddComponent<StandaloneInputModule>();
#endif

		// broadcast that the sound settings have been
		// changed by being loaded
		OnVolumeChanged?.Invoke();

	}

	private void Update() {
#if ENABLE_INPUT_SYSTEM
		if (Keyboard.current.escapeKey.isPressed && !m_wasEscapePressedLastFrame) {
#else
		if (Input.GetKeyDown(KeyCode.Escape)) {
#endif
			// Pausing can only be toggled if we
			// are not currently viewing custom
			// options
			if (!m_isInGameOptions) {
				SetPaused(!m_isPaused);
			}
		}
#if ENABLE_INPUT_SYSTEM
		m_wasEscapePressedLastFrame = Keyboard.current.escapeKey.isPressed;
#endif
	}

	private void SetPaused(bool isPaused) {
		SetPaused(isPaused, true);
	}
	private void SetPaused(bool isPaused, bool invoke) {
		m_isPaused = isPaused;
		m_pauseCanvas.SetActive(m_isPaused);
		if (m_isPaused) {
			// make sliders match settings
			m_sliderSound.value = m_settings.SoundVolume;
			m_sliderMusic.value = m_settings.MusicVolume;
			m_sliderVoice.value = m_settings.VoiceVolume;

			// register pause menu listeners
			m_sliderSound.onValueChanged.AddListener(HandleSliderSoundChanged);
			m_sliderMusic.onValueChanged.AddListener(HandleSliderMusicChanged);
			m_sliderVoice.onValueChanged.AddListener(HandleSliderVoiceChanged);
			m_buttonQuit.onClick.AddListener(HandleButtonQuit);
			m_buttonResume.onClick.AddListener(HandleButtonResume);
			m_buttonGameOptions.gameObject.SetActive(m_useCustomOptions);
			m_buttonGameOptions.onClick.AddListener(HandleButtonGameOptions);
		} else {
			// write option changes to file
			File.WriteAllText(GetSettingsPath(), JsonUtility.ToJson(m_settings));

			// deregister pause menu listeners
			m_sliderSound.onValueChanged.RemoveListener(HandleSliderSoundChanged);
			m_sliderMusic.onValueChanged.RemoveListener(HandleSliderMusicChanged);
			m_sliderVoice.onValueChanged.RemoveListener(HandleSliderVoiceChanged);
			m_buttonQuit.onClick.RemoveListener(HandleButtonQuit);
			m_buttonResume.onClick.RemoveListener(HandleButtonResume);
			m_buttonGameOptions.onClick.RemoveListener(HandleButtonGameOptions);
		}
		// broadcast that the pause state has changed
		if (invoke) {
			OnGamePaused?.Invoke(m_isPaused);
		}
	}

	private string GetSettingsPath() {
		// todo: replace with a generated path provided
		//       on application launch
		string path = Application.persistentDataPath + "/settings.json";
		return path;
	}


	private void HandleSliderSoundChanged(float value) {
		m_settings.SoundVolume = value;
		OnVolumeChanged?.Invoke();
	}
	private void HandleSliderMusicChanged(float value) {
		m_settings.MusicVolume = value;
		OnVolumeChanged?.Invoke();
	}
	private void HandleSliderVoiceChanged(float value) {
		m_settings.MusicVolume = value;
		OnVolumeChanged?.Invoke();
	}
	private void HandleButtonQuit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
	private void HandleButtonResume() {
		SetPaused(false);
	}
	private void HandleButtonGameOptions() {
		m_isInGameOptions = true;
		SetPaused(false, false);
		OnGameOptionsOpened?.Invoke();
	}


	[Serializable]
	private class Settings {
		public static readonly Settings Default = new Settings() {
			SoundVolume = 0.8f,
			MusicVolume = 0.8f,
			VoiceVolume = 0.8f
		};
		public float SoundVolume { 
			get { return m_soundVolume; } 
			set { m_soundVolume = value; } 
		}
		public float MusicVolume {
			get { return m_musicVolume; }
			set { m_musicVolume = value; }
		}
		public float VoiceVolume {
			get { return m_voiceVolume; }
			set { m_voiceVolume = value; }
		}
		[SerializeField]
		private float m_soundVolume = 0.8f;
		[SerializeField]
		private float m_musicVolume = 0.8f;
		[SerializeField]
		private float m_voiceVolume = 0.8f;

	}
}