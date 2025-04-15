using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<Tile> _tiles;
    [SerializeField] GameObject _tileObject;
    int _numberOfPooledTiles = 6;
    MMF_Player _mmf;

    void Awake() {
        Instance = this; //set the instance of the object pool to this
    }

    void Start() {
        _mmf = GameObject.FindGameObjectWithTag("FeedbacksHandler").GetComponent<MMF_Player>(); //find the gameobject with tag and get the mmf player 

        //the object pool
        _tiles = new List<Tile>();

        GameObject tile; //the temporary gameobject
        for (int i = 0; i < _numberOfPooledTiles; i++) { //for 3 tiles
            tile = Instantiate(_tileObject); //instantiate tile
            tile.SetActive(false); //deactivate tile
            _tiles.Add(new Tile(tile,_mmf)); //add the tile to queue
        }
    }

    //this method returns a tile
    public Tile GetTile() {
        for (int i = 0; i < _numberOfPooledTiles; i++) {
            if (!_tiles[i].TileObject.activeInHierarchy) return _tiles[i]; //if the current tile isnt active in the hierarchy, return it     
        }
        return null; //otherwise, return null
    }
}
