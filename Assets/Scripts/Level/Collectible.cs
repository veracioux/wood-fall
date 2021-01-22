using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{

    int nChildren = 10;

    void OnCollect()
    {
        if (--nChildren == 0)
        {
            Level.OnCollect();
            gameObject.SetActive(false);
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            transform.parent.gameObject.GetComponent<Collectible>().OnCollect();
            gameObject.SetActive(false);
        }
    }
}
