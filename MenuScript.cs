using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Lofelt.NiceVibrations;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject _statsPanel, _configPanel;
    int _highscore, _swipes, _gamesPlayed;
    [SerializeField] TMP_Text _topText, _bottomText, _middleLeftText, _middleRightText, _highscoreText, _swipeText, _gamesPlayedText;
    [SerializeField] Button _topButton, _bottomButton, _middleLeftButton, _middleRightButton, _sfxOn, _sfxOff, _vibrationOn, _vibrationOff, _data;
    int _primaryColor, _secondaryColor, _initialPrimaryColor, _initialSecondaryColor;

    // Start is called before the first frame update
    void Start()
    {
        _topButton.onClick.AddListener(OnPlayButtonPressed);
        _bottomButton.onClick.AddListener(OnThemeButtonPressed);
        _middleLeftButton.onClick.AddListener(OnStatsButtonPressed);
        _middleRightButton.onClick.AddListener(OnConfigButtonPressed);
        LoadStats();
        UpdateStatText();
        UpdateButtonStatus();
    }

    private void RemoveAllButtonListeners() {
        _topButton.onClick.RemoveAllListeners();
        _bottomButton.onClick.RemoveAllListeners();
        _middleLeftButton.onClick.RemoveAllListeners();
        _middleRightButton.onClick.RemoveAllListeners();
    }

    //this is called when the back button is pressed 
    void RevertColors() {
        GameManager.Instance.SetPrimaryColor(_initialPrimaryColor);
        GameManager.Instance.SetSecondaryColor(_initialSecondaryColor);
    }

    public void OnPlayButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        LoadScene("ingame"); //load the ingame scene
    }

    public void OnCloseButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        //disable the canvases
        _statsPanel.SetActive(false);
        _configPanel.SetActive(false); 
    }

    public void OnStatsButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        _statsPanel.SetActive(true);
    }

    public void OnConfigButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        _configPanel.SetActive(true);
    }

    void OnThemeButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        _primaryColor = GameManager.Instance.GetPrimaryColorIndex();
        _secondaryColor = GameManager.Instance.GetSecondaryColorIndex();
        _initialPrimaryColor = _primaryColor;
        _initialSecondaryColor = _secondaryColor;
        PlayButtonSound();
        RemoveAllButtonListeners(); //remove all button listeners
        _topButton.onClick.AddListener(OnPrimaryThemeButtonPressed);
        _bottomButton.onClick.AddListener(OnSecondaryThemeButtonPressed);
        _middleLeftButton.onClick.AddListener(OnCancelButtonPressed);
        _middleRightButton.onClick.AddListener(OnConfirmButtonPressed);
        _topText.text = "TAP TO\nCHANGE";
        _bottomText.text = "TAP TO\nCHANGE";
        _middleLeftText.text = "CANCEL";
        _middleRightText.text = "CONFIRM";
    }

    void OnPrimaryThemeButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        if (_primaryColor < GameManager.Instance.NumberOfColors()-1) _primaryColor++;
        else _primaryColor = 0;

        //if primary color is equal to secondary color
        if (_primaryColor == _secondaryColor) {
            //if primary is on last index
            if (_primaryColor == GameManager.Instance.NumberOfColors() - 1) _primaryColor = 0; //set primary color to first index
            else _primaryColor++; //increment the primary color index
        }

        GameManager.Instance.SetPrimaryColor(_primaryColor);
    }

    void OnSecondaryThemeButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        if (_secondaryColor < GameManager.Instance.NumberOfColors() - 1) _secondaryColor++;
        else _secondaryColor = 0;

        //if secondary color is equal to primary color
        if (_secondaryColor == _primaryColor) {
            //if secondary is on last index 
            if (_secondaryColor == GameManager.Instance.NumberOfColors() - 1) _secondaryColor = 0; //set secondary color to first index
            else _secondaryColor++; //increment the secondary color index
        }

        GameManager.Instance.SetSecondaryColor(_secondaryColor);
    }

    void OnCancelButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        RemoveAllButtonListeners();
        _topButton.onClick.AddListener(OnPlayButtonPressed);
        _bottomButton.onClick.AddListener(OnThemeButtonPressed);
        _middleLeftButton.onClick.AddListener(OnStatsButtonPressed);
        _middleRightButton.onClick.AddListener(OnConfigButtonPressed);
        _topText.text = "PLAY";
        _bottomText.text = "THEME";
        _middleLeftText.text = "STATS";
        _middleRightText.text = "CONFIG";
        RevertColors();
    }

    void OnConfirmButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayButtonSound();
        RemoveAllButtonListeners();
        _topButton.onClick.AddListener(OnPlayButtonPressed);
        _bottomButton.onClick.AddListener(OnThemeButtonPressed);
        _middleLeftButton.onClick.AddListener(OnStatsButtonPressed);
        _middleRightButton.onClick.AddListener(OnConfigButtonPressed);
        _topText.text = "PLAY";
        _bottomText.text = "THEME";
        _middleLeftText.text = "STATS";
        _middleRightText.text = "CONFIG";
        PlayerPrefs.SetInt("primaryColor", _primaryColor);
        PlayerPrefs.SetInt("secondaryColor", _secondaryColor);
    }

    private void LoadStats() {
        _highscore = PlayerPrefs.GetInt("highscore", 0);
        _swipes = PlayerPrefs.GetInt("swipes", 0);
        _gamesPlayed = PlayerPrefs.GetInt("gamesPlayed", 0);
    }

    private void UpdateStatText() {
        _highscoreText.text = _highscore.ToString();
        _swipeText.text = _swipes.ToString();
        _gamesPlayedText.text = _gamesPlayed.ToString();
    }

    private void PlayButtonSound() => SoundManager.PlaySound("button");

    void LoadScene(string scene) => SceneManager.LoadScene(sceneName: scene); //load the given scene

    public void OnSFXEnabledButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayerPrefs.SetInt("sfx", 1); //turn the vibration on
        UpdateButtonStatus();
        PlayButtonSound();
    }

    public void OnSFXDisabledButtonPressed() {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayerPrefs.SetInt("sfx", 0); //turn the vibration off
        UpdateButtonStatus();
        PlayButtonSound();
    }

    public void OnVibrationEnabledButtonPressed() {
        HapticController.hapticsEnabled = true; //enable haptics
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayerPrefs.SetInt("vibration", 1); //turn the vibration on
        UpdateButtonStatus();
        
        PlayButtonSound();
    }

    public void OnVibrationDisabledButtonPressed() {
        HapticController.hapticsEnabled = false; //disable haptics
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        PlayerPrefs.SetInt("vibration", 0); //turn the vibration off
        UpdateButtonStatus();
        PlayButtonSound();
    }

    //this method updates the status of all the buttons
    private void UpdateButtonStatus() {
        //if sfx is enabled
        if (PlayerPrefs.GetInt("sfx", 1) == 1) {
            _sfxOn.interactable = false;
            _sfxOff.interactable = true;
        }
        //if sfx is disabled
        else {
            _sfxOff.interactable = false;
            _sfxOn.interactable = true;
        }
        //if vibration is enabled
        if (PlayerPrefs.GetInt("vibration",1) == 1) {
            _vibrationOn.interactable = false;
            _vibrationOff.interactable = true;
        }
        //if vibration is disabled
        else {
            _vibrationOff.interactable = false;
            _vibrationOn.interactable = true;
        }
    }
}
