using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class TileScript : MonoBehaviour
{
    Tile[] _tiles;
    [SerializeField] MMF_Player _mmfPlayer; 
    MMF_ScaleShake _sS;

    //the vector array for the "deck" positions
    Vector3[] _tilePositions = new Vector3[] {
        new Vector3(0f, -0.3f, -0.5f), //bottom
        new Vector3(0f, 0f, -1f), //middle
        new Vector3(0f, 0.3f, -1.5f) //top
    };

    //the vector array for the end positions (where the tiles go when swiped)
    Vector3[] _endPositions = new Vector3[] {
        new Vector3(0f,3f,-2f), //top
        new Vector3(0f,-3f,-2f) //bottom
    };

    //create an empty array for the tile colors
    Color32[] _tileColors = new Color32[2];

    //deck tiles
    [SerializeField] GameObject _tileOne; 
    [SerializeField] GameObject _tileTwo;
    [SerializeField] GameObject _tileThree;

    // Start is called before the first frame update
    void Start(){
        _tileColors[0] = GameManager.Instance.PrimaryColor; //set the primary color
        _tileColors[1] = GameManager.Instance.SecondaryColor; //set the secondary color

        //randomize colors of starting tiles
        _tileOne.GetComponent<SpriteRenderer>().color = _tileColors[Random.Range(0, 2)];
        _tileTwo.GetComponent<SpriteRenderer>().color = _tileColors[Random.Range(0, 2)];
        _tileThree.GetComponent<SpriteRenderer>().color = _tileColors[Random.Range(0, 2)];

        _sS = _mmfPlayer.GetFeedbackOfType<MMF_ScaleShake>(); //get the reference to the scale shake
        _tiles = new Tile[] {new Tile(_tileOne,0), new Tile(_tileTwo,1),new Tile(_tileThree,2)}; //create tiles with the tile gameobjects

        CycleTilesInstant(); //this is needed so correct "sent" tile color is displayed at start
    }

    //this method cycles all the tiles
    public void CycleTiles() {
        foreach (Tile tile in _tiles) {
            var tempTilePos = tile.Position; //save the tile position temporarily
            if (tile.Position == 2) tile.InheritColor(_tileColors[Random.Range(0, 2)]); //pick a random color to become
            tile.UpdatePosition(GetNewPosition(tile.Position)); //set the tile's position to its next position
            if (tempTilePos == 2) tile.MoveInstant(_tilePositions[tile.Position]); //if the tile is on the top spot, move instantly to the back
            else tile.Move(_tilePositions[tile.Position]); //move to the position
        }
    }

    void CycleTilesInstant() {
        foreach (Tile tile in _tiles) {
            var tempTilePos = tile.Position; //save the tile position temporarily
            if (tile.Position == 2) tile.InheritColor(_tileColors[Random.Range(0, 2)]); //pick a random color to become
            tile.UpdatePosition(GetNewPosition(tile.Position)); //set the tile's position to its next position
            if (tempTilePos == 2) tile.MoveInstant(_tilePositions[tile.Position]); //if the tile is on the top spot, move instantly to the back
            else tile.MoveInstant(_tilePositions[tile.Position]); //move to the position
        }
    }
    //this method sends the tile in the given direction
    public void SendTile(int direction) {
        var tile = ObjectPool.Instance.GetTile(); //get a tile from the object pool
        _sS.TargetShaker = tile.TileObject.GetComponent<MMScaleShaker>(); //change the ss shaker target to the pooled object
        tile.TileObject.transform.position = _tilePositions[2]; //move the tile to top of the deck
        tile.InheritColor(GetTopTile().GetComponent<SpriteRenderer>().color); //transfer the color from the top tile to the sent tile
        tile.InheritScore(GetTopTile().transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text); //transfer the score from the top tile to the sent tile
        tile.TileObject.SetActive(true); //enable the tile
        tile.MoveToEnd(_endPositions[direction]); //move in the given direction
        tile.PlayFeedback(); //play the feedbacks
        StartCoroutine("DestroyTile", tile); //destroy the tile after 0.5 seconds
    }
    
    //destroy the tile after
    IEnumerator DestroyTile(Tile tile) {
        yield return new WaitForSeconds(0.5f); //wait for half a second
        tile.Deactive();
    }

    //this method takes a position integer as a parameter and returns the new position value
    private int GetNewPosition(int position) {
        if (position < 2) return position+1; //if the position is less than 2, return the next position
        else return 0; //otherwise, return 0
    }

    private void PlayFeedback() {
        _mmfPlayer.PlayFeedbacks();
    }

    //this method returns the top tile color
    private GameObject GetTopTile() {
        //for all the tiles
        foreach (Tile tile in _tiles) {
            //if on the top, return the tile
            if (tile.Position == 2) return tile.TileObject;
        }
        return null; //otherwise, return null
    }

    //this method updates the tile number to the parameter
    public void UpdateTileNumber(int number) {
        foreach (Tile tile in _tiles) tile.InheritScore(number.ToString()); //for each tile, change its number to the parameter
    }
}
