using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Color32 PrimaryColor { get; set; } //the primary theme color
    public Color32 SecondaryColor { get; set; } //the secondary theme color

    private SpriteRenderer _topPanel, _bottomPanel;

    Color32[] _allColors = new Color32[] {
        new Color32(255, 64, 64, 255), //red 
        new Color32(0, 127, 255, 255), //blue
        new Color32(127, 255, 255, 255), //light blue
        new Color32(255, 89, 255, 255), //lime 
        new Color32(255, 141, 0, 255), //orange
        new Color32(250, 133, 255, 255), //pink
        new Color32(255, 0, 115, 255), //magenta
        new Color32(134, 0, 255, 255), //purple
        new Color32(255, 225, 0, 255), //yellow
        new Color32(46, 46, 46, 255), //grey
        new Color32(253, 218, 158, 255) //tan
    };

    private void Awake() {
        if (Instance == null) Instance = this; //if instance is null, make this the instance
        else Destroy(gameObject); //otherwise, destroy this to avoid duplicates
        DontDestroyOnLoad(this); //dont destroy this on load
    }

    private void Start() {
        PrimaryColor = _allColors[PlayerPrefs.GetInt("primaryColor",0)];
        SecondaryColor = _allColors[PlayerPrefs.GetInt("secondaryColor",1)];
        _topPanel = GameObject.FindGameObjectWithTag("PrimaryColor").GetComponent<SpriteRenderer>();
        _topPanel.color = PrimaryColor;
        _bottomPanel = GameObject.FindGameObjectWithTag("SecondaryColor").GetComponent<SpriteRenderer>();
        _bottomPanel.color = SecondaryColor;
    }

    public void SetPrimaryColorToNextColor() {

    }


    //set the primary color
    public void SetPrimaryColor(int color) {
        PrimaryColor = _allColors[color];
        _topPanel.color = PrimaryColor;
    }

    //set the secondary color
    public void SetSecondaryColor(int color) {
        SecondaryColor = _allColors[color];
        _bottomPanel.color = SecondaryColor;
    }

    public int GetPrimaryColorIndex() {
        for(int i = 0; i < _allColors.Length; i++) {
            if (PrimaryColor.Equals(_allColors[i])) return i;
        }
        return 0;
    }

    public int GetSecondaryColorIndex() {
        for (int i = 0; i < _allColors.Length; i++) {
            if (SecondaryColor.Equals(_allColors[i])) return i;
        }
        return 0;
    }

    public int NumberOfColors() {
        return _allColors.Length;
    }
}
