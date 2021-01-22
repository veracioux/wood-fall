using UnityEngine;
using System.Collections;

public class CircularRotation : MonoBehaviour
{

	void Update()
    {
        transform.eulerAngles -= new Vector3(0, 0, 40 * Time.unscaledDeltaTime);
	}
}
