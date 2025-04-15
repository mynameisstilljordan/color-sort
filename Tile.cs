using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using MoreMountains.Feedbacks;

public class Tile
{
    public GameObject TileObject { get; set; }
    public int Position { get; set; }

    MMF_Player _mmF;

    SpriteRenderer _flash;

    private float _timeToShufffle = 0.15f; //the amount of time it takes for the tile to change its spot in the deck
    private float _timeToMove = 0.25f;  //the amount of time it takes for the tile to move

    Vector3 _bottom = new Vector3(0f, -0.3f, -0.5f);

    //the constructor of the tile class
    public Tile(GameObject tile, int position) {
        this.Position = position; //passing the position parameter
        this.TileObject = tile; //passing the tile parameter
        _flash = tile.transform.GetChild(2).GetComponent<SpriteRenderer>(); //get the flash sprite renderer
    }

    //the alternate constructor of the tile class
    public Tile(GameObject tile, MMF_Player mmF) {
        this.TileObject = tile;
        this._mmF = mmF; //the moremountains feedback reference
        _flash = tile.transform.GetChild(2).GetComponent<SpriteRenderer>(); //get the flash sprite renderer
    }

    //this method moves the tile to the desired position
    public void Move(Vector3 position) {
       TileObject.transform.DOMove(position, _timeToShufffle).OnUpdate(() => {
            if (this.Position == 0) { //if the new position is the bottom of the deck and this position was inherited while tweening
               DOTween.Kill(TileObject.transform); //kill the tween
               MoveInstant(_bottom); //instantly move to the bottom of the 
            }
        });        
    }

    //this method flashes the card white
    private void Flash() => _flash.DOFade(1f, 0.2f).OnComplete(FadeFlash); //update flash

    //this method returns the card to no color
    public void FadeFlash() => _flash.DOFade(0, 0);

    //this method instantly moves the tile to the desired position
    public void MoveInstant(Vector3 position) {
        TileObject.transform.position = position;
    }

    //this method moves the title to the desired position and then does the end animation
    public void MoveToEnd(Vector3 position) {
        TileObject.transform.DOMove(position, _timeToMove).OnComplete(CheckForCorrectPosition); //move to the end position then play end animation
    }

    private void CheckForCorrectPosition() {
        //if the tile is on the top half of the screen 
        if (TileObject.transform.position.y > 0) {
            if (!GameManager.Instance.PrimaryColor.Equals(GetColor())) SignalToEndGame(); //if the tile is in the wrong section, signal to end game
            else {
                PlayFeedback(); //play the feedback
                //SoundManager.PlaySound("point");
                Flash(); //flash white
            }
        }
        //if the tile is on the bottom half of the screen
        else {
            if (!GameManager.Instance.SecondaryColor.Equals(GetColor())) SignalToEndGame(); //if the tile is in the wrong section, signal to end game
            else {
                PlayFeedback(); //play the feedback
                //SoundManager.PlaySound("point");
                Flash(); //flash white
            }
        }
    }

    private void SignalToEndGame() => GameObject.FindGameObjectWithTag("GameManager").GetComponent<IngameScript>().EndGame();

    //this method updates the position to the given parameter
    public void UpdatePosition(int position) {
        Position = position; //set the new value of position
    }

    //remove object 
    public void RemoveObject() {
        TileObject.SetActive(false); //disable the tile
        FadeFlash(); //remove the flash opacity
    }

    //this method inherits the color from the given tile
    public void InheritColor(Color32 color) {
        TileObject.GetComponent<SpriteRenderer>().color = color;
    }

    //this method inherits the score from the given tile
    public void InheritScore(string score) {
        TileObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = score; //set text to given parameter
    }

    private Color32 GetColor() {
        return TileObject.GetComponent<SpriteRenderer>().color; //return the color of the sprite
    }

    //this method deactivates the gameobject associated with this tile
    public void Deactive() => TileObject.SetActive(false);

    //this method activates the gameobject associated with this tile
    public void Activate() => TileObject.SetActive(true);

    //this method triggers the MMF Player
    public void PlayFeedback() => _mmF.PlayFeedbacks(); //play the feedbacks
}
