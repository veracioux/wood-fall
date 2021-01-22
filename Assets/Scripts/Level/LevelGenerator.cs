using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

    public static int seed = 0, themeID = 0;
    int last = 0;
    public static float minDistanceGenerated, distanceGenerated, minDistanceTrees, distanceTrees;
    public GameObject[] objects;
    public static ArrayList items = new ArrayList();
    public GameObject[] _background, _player, _axe, _saw, _tail, _hingeNail, _nail, _star, _meter;
    public static GameObject background, player, axe, saw, tail, hingeNail, nail, star, meter;
    public Color[] colors;
    public static Color color;

    void Awake()
    {
        minDistanceGenerated = 15;
        distanceGenerated = 10;
        minDistanceTrees = 40.872f;
        distanceTrees = -10.218f;
        background = _background[themeID];
        player = _player[themeID];
        axe = _axe[themeID];
        tail = _tail[themeID];
        saw = _saw[themeID];
        tail = _tail[themeID];
        hingeNail = _hingeNail[themeID];
        nail = _nail[themeID];
        star = _star[themeID];
        meter = _meter[themeID];
        color = colors[themeID];
    }

    void Start()
    {
        Random.InitState(seed);
        Random.seed = seed;
    }

    void Update()
    {
        if (!Level.player.dead || !Level.level.usedRevive || Level.level.reviving)
        {
            if (distanceGenerated + Level.player.transform.position.y <= minDistanceGenerated)
                GenerateNext();
        }
        if (distanceTrees + Level.player.transform.position.y < minDistanceTrees)
            GenerateTrees();
    }

    public static void Seed(int seed)
    {
        Random.seed = seed;
    }

    public void GenerateNext()
    {
        int t = Random.Range(0, objects.Length);
        Random.seed++;
        int i = t != last ? t : (t + 3) % objects.Length;
        Instantiate(objects[i]).transform.Translate(0, -distanceGenerated - objects[i].GetComponent<GenTemplate>().height / 2, 0);
        distanceGenerated += objects[i].GetComponent<GenTemplate>().height;
        last = i;
    }

    public void GenerateTrees()
    {
        Instantiate(meter).transform.Translate(0, -distanceTrees - 10f, 0);
        distanceTrees += 40.83f;
    }
}
