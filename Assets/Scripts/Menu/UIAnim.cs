using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnim : MonoBehaviour {

    public float step, time;
    public Button[] disable;
    float target;
    bool animating;

	// Use this for initialization
	void Start () {
        target = transform.eulerAngles.z;
        animating = false;
	}
	
    public void Next()
    {
        target = transform.eulerAngles.z + step;
        if (disable != null)
            for (int i = 0; i < disable.Length; i++)
                disable[i].enabled = false;
        animating = true;
    }

    public void Finished()
    {
        if (disable != null)
            for (int i = 0; i < disable.Length; i++)
                disable[i].enabled = true;
        animating = false;
    }

	// Update is called once per frame
	void Update () {
        if (animating)
        {
            transform.eulerAngles -= new Vector3(0, 0, Time.unscaledDeltaTime / time * step);
            if (transform.eulerAngles.z <= target)
            {
                transform.eulerAngles = new Vector3(0, 0, target);
                Finished();
            }

        }

	}
}
