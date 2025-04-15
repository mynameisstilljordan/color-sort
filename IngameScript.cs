using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Lofelt.NiceVibrations;

public class IngameScript : MonoBehaviour
{
    private enum GameState {
        Ingame, EndGame
    }

    private GameState _gamestate;

    int _minimumSwipeDistance = Screen.height / 15; //the distance required to swipe in order for an input to be registered (15% of screen height)
    Vector2 _startTouchPos, _endTouchPos; //for the start and end touch position

    int _score; //the score counter
    TileScript _tS; //the tilescript reference
    TimerScript _t; //the timerscript reference

    [SerializeField] Canvas _endgameCanvas; //the end game canvas
    [SerializeField] TMP_Text _endgameScoreText, _endgameHighscoreText;

    // Start is called before the first frame update
    void Start()
    {
        _gamestate = GameState.Ingame; //set gamestate to ingame
        _t = GetComponent<TimerScript>(); //get the timerscript from the gameobject
        _tS = GetComponent<TileScript>(); //get the tilescript from the gameobject
    }

    // Update is called once per frame
    void Update() {
        //save the number of touches
        var _count = Input.touchCount;

        //if there is only 1 touch on the screen
        if (_count == 1 && _gamestate == GameState.Ingame) {
            var _touch = Input.GetTouch(0);

            //if the touch phase has began
            if (_touch.phase == TouchPhase.Began) {
                _startTouchPos = _touch.position; //save the starting position
            }

            //if the touch phase has ended
            if (_touch.phase == TouchPhase.Ended) {
                _endTouchPos = _touch.position; //save the ending position 

                //if the swipe was far enough to be considered a swipe
                if ((Mathf.Abs(_startTouchPos.y - _endTouchPos.y) > _minimumSwipeDistance)){
                    //if swipe was down
                    if (_startTouchPos.y > _endTouchPos.y) HandleInput(1);
                    //if swipe was upwards
                    else HandleInput(0); 
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A)) HandleInput(0);
        if (Input.GetKeyDown(KeyCode.S)) HandleInput(1);
    }

    //this method handles the end of the game
    public void EndGame() {
        _gamestate = GameState.EndGame; //set gamestate to end game
        _t.StopTimer();
        _score--; //this is to make up for the fact that a point is added as soon as the player swipes in a direction, regardless if it was the correct direction
        _endgameCanvas.enabled = true; //enable the endgame canvas
        CheckForAndUpdateHighscore(); //check for a highscore
        UpdateEndgameText(); //update the endgame text
        UpdateStats(); //update the stats
    }

    //this method increments the score
    void IncrementScore() {
        _score++; //increment the score
        _tS.UpdateTileNumber(_score); //update the number on the tiles to the current score
    }

    //this method handles the input from the player
    void HandleInput(int direction) {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        SoundManager.PlaySound("swipe"); //play the swipe sound
        _tS.SendTile(direction); //send a tile to end
        _tS.CycleTiles(); //cycle the deck
        IncrementScore(); //increment the score
        _t.RestartTimer(); //restart the timer
    }

    //this method is for the timerscript
    public void AdjustScore() {
        _score++; 
    }

    //when the continue button is pressed
    public void OnContinueButtonPressed() => SceneManager.LoadScene("menu"); //go to the menu scene

    //this method checks if there's a highscore and updates it if there is
    private void CheckForAndUpdateHighscore() {
        if (_score > PlayerPrefs.GetInt("highscore")) PlayerPrefs.SetInt("highscore", _score); //update the highscore depending on the score
    }

    //this method updates the endgame text
    private void UpdateEndgameText() {
        _endgameScoreText.text = "SCORE: " + _score; //update the score text
        _endgameHighscoreText.text = "BEST: " + PlayerPrefs.GetInt("highscore"); //update the highscore text
    }

    //this method updates the stats and saves them
    private void UpdateStats() {
        PlayerPrefs.SetInt("swipes", PlayerPrefs.GetInt("swipes",0) + _score); //save the swipes
        PlayerPrefs.SetInt("gamesPlayed", PlayerPrefs.GetInt("gamesPlayed",0) + 1); //save the number of games played
    }
}
