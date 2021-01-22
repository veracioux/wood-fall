using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{

    public GameObject arrow;
    bool side = true;

    void Start()
    {

    }

    void Update()
    {
        if (side)
            arrow.transform.eulerAngles = new Vector3(0, 0, arrow.transform.eulerAngles.z + 100 * Time.unscaledDeltaTime);
        else
            arrow.transform.eulerAngles = new Vector3(0, 0, arrow.transform.eulerAngles.z - 100 * Time.unscaledDeltaTime);
        if (arrow.transform.eulerAngles.z < 0 || arrow.transform.eulerAngles.z > 180)
        {
            arrow.transform.eulerAngles = new Vector3(0, 0, side ? 180 : 0);
            side = !side;
        }
    }
}
