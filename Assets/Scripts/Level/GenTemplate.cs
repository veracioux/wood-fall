using UnityEngine;
using System.Collections;

public class GenTemplate : MonoBehaviour
{

    public float height;
    public Transform[] axes, hingeNails, nails, saws, stars;
    public Transform revive;

    void Start()
    {
        for (int i = 0; i < axes.Length; i++)
        {
            GameObject axe;
            axe = Instantiate(LevelGenerator.axe);
            axe.transform.parent = transform;
            axe.transform.Translate(axes[i].position);
            axe.transform.Rotate(axes[i].eulerAngles);
            axe.transform.localScale = axes[i].localScale;
            /*LevelGenerator.axes[LevelGenerator.axeIndex].transform.parent = transform;
            LevelGenerator.axes[LevelGenerator.axeIndex].transform.position = axes[i].position;
            LevelGenerator.axes[LevelGenerator.axeIndex].transform.eulerAngles = axes[i].eulerAngles;
            LevelGenerator.axes[LevelGenerator.axeIndex].transform.localScale = axes[i].localScale;*/
            GameObject hinge = Instantiate(LevelGenerator.hingeNail);
            hinge.transform.parent = transform;
            hinge.transform.position = hingeNails[i].position;
            hinge.transform.eulerAngles = hingeNails[i].eulerAngles;
            hinge.transform.localScale = hingeNails[i].localScale;
            axe.GetComponent<HingeJoint2D>().connectedBody = hinge.GetComponent<Rigidbody2D>();
            /*LevelGenerator.hingeNails[LevelGenerator.hingeNailIndex].transform.parent = transform;
            LevelGenerator.hingeNails[LevelGenerator.hingeNailIndex].transform.position = hingeNails[i].position;
            LevelGenerator.hingeNails[LevelGenerator.hingeNailIndex].transform.eulerAngles = hingeNails[i].eulerAngles;
            LevelGenerator.hingeNails[LevelGenerator.hingeNailIndex].transform.localScale = hingeNails[i].localScale;

            LevelGenerator.axes[LevelGenerator.axeIndex].SetActive(true);
            LevelGenerator.axes[LevelGenerator.axeIndex].GetComponent<HingeJoint2D>().connectedBody = LevelGenerator.hingeNails[LevelGenerator.hingeNailIndex].GetComponent<Rigidbody2D>();
            LevelGenerator.axes[LevelGenerator.axeIndex].GetComponent<HingeJoint2D>().enabled = true;
            LevelGenerator.axeIndex++;
            LevelGenerator.axeIndex %= 15;
            LevelGenerator.hingeNailIndex++;
            LevelGenerator.hingeNailIndex %= 15; */

        }
        for (int i = 0; i < nails.Length; i++)
        {
            GameObject nail = Instantiate(LevelGenerator.nail);
            nail.transform.parent = transform;
            nail.transform.Translate(nails[i].position);
            nail.transform.Rotate(nails[i].eulerAngles);
            nail.transform.localScale = nails[i].localScale;
            /*LevelGenerator.nails[LevelGenerator.nailIndex].transform.parent = transform;
            LevelGenerator.nails[LevelGenerator.nailIndex].transform.Translate(nails[i].position);
            LevelGenerator.nails[LevelGenerator.nailIndex].transform.Rotate(nails[i].eulerAngles);
            LevelGenerator.nails[LevelGenerator.nailIndex].transform.localScale = nails[i].localScale;
            LevelGenerator.nailIndex++;
            LevelGenerator.nailIndex %= 15;*/
        }
        for (int i = 0; i < saws.Length; i++)
        {
            GameObject saw = Instantiate(LevelGenerator.saw);
            saw.transform.parent = transform;
            saw.transform.Translate(saws[i].position);
            saw.transform.Rotate(saws[i].eulerAngles);
            saw.transform.localScale = saws[i].localScale;
            /*LevelGenerator.saws[LevelGenerator.sawIndex].transform.parent = transform;
            LevelGenerator.saws[LevelGenerator.sawIndex].transform.Translate(saws[i].position);
            LevelGenerator.saws[LevelGenerator.sawIndex].transform.Rotate(saws[i].eulerAngles);
            LevelGenerator.saws[LevelGenerator.sawIndex].transform.localScale = saws[i].localScale;
            LevelGenerator.sawIndex++;
            LevelGenerator.sawIndex %= 15;*/
        }
        if (stars.Length > 0)
        {
            int j = Random.Range(0, stars.Length);
            GameObject star = Instantiate(LevelGenerator.star);
            star.transform.parent = transform;
            star.transform.Translate(stars[j].position);
            /*LevelGenerator.stars[LevelGenerator.starIndex].transform.parent = transform;
            LevelGenerator.stars[LevelGenerator.starIndex].transform.Translate(stars[j].position);
            LevelGenerator.starIndex++;
            LevelGenerator.starIndex %= 15;*/
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Level.reviver = revive;
            Statistics.UpdateDistanceAchievements(Settings.difficulty, Level.level.distance);
            Statistics.UpdateStarAchievements(Settings.difficulty, Level.level.stars);
        }
    }
}
