using UnityEngine;
using System.Collections;

public class TestGen : MonoBehaviour {

    public GameObject background, player, axe, saw, tail, hingeNail, nail, star, tree;

    void Start ()
    {
        LevelGenerator.background = background;
        LevelGenerator.player = player;
        LevelGenerator.axe = axe;
        LevelGenerator.tail = tail;
        LevelGenerator.saw = saw;
        LevelGenerator.tail = tail;
        LevelGenerator.hingeNail = hingeNail;
        LevelGenerator.nail = nail;
        LevelGenerator.star = star;
        LevelGenerator.meter = tree;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
