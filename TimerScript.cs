using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimerScript : MonoBehaviour
{
    [SerializeField] GameObject _leftTimerMask, _rightTimerMask; //the layer masks
    float _screenWidth, _targetX, _time = 10f; //the screen width to account for all device sizes
    float _timeIncreaseIncrement = 0.01f; //the amount that the time speeds up after every swipe
    Sequence _sequence; //the dotween sequence for the masks 
    IngameScript _iGS; //get a reference to the ingame script
    // Start is called before the first frame update

    void Start() {
        //set the sequence to an instance of a sequence
        _sequence = DOTween.Sequence();
   
        _iGS = GetComponent<IngameScript>(); //get the reference of the ingame script from the same gameobject

        float _screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2; //the width of the screen
        _targetX = Camera.main.orthographicSize * Camera.main.aspect; //the target position of the timer bars

        //set the position of both masks to the edge of the screen
        _leftTimerMask.transform.position = new Vector2(-_screenWidth, 0);
        _rightTimerMask.transform.position = new Vector2(_screenWidth, 0);

        //set the scale of both masks
        _leftTimerMask.transform.localScale = new Vector2(_screenWidth, 10f);
        _rightTimerMask.transform.localScale = new Vector2(_screenWidth, 10f);

        //add the animation of the two masks to the seqence
        _sequence.Append(_leftTimerMask.transform.DOMoveX(-_targetX, _time));
        _sequence.Join(_rightTimerMask.transform.DOMoveX(_targetX, _time));

        _sequence.SetEase(Ease.Linear); //set a linear ease

        //when the sequence ends
        _sequence.OnComplete(() => {
            _iGS.AdjustScore(); //adjust for the decrement
            _iGS.EndGame(); //call for the end of the game
        });
    }

    //this method restarts the timer masks to their original positions
    public void RestartTimer() {
        if (_sequence.timeScale < 10f) _sequence.timeScale += _timeIncreaseIncrement; //increment the timescale by the time scale increment
        _sequence.Restart(); //restart the sequence
    }

    public void StopTimer() {
        _sequence.Pause();
    }

}
