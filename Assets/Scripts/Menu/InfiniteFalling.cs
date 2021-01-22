using UnityEngine;

public class InfiniteFalling : MonoBehaviour {

    Vector3 startPos;

    void Start ()
    {
        startPos = transform.position;
    }

	void Update () {
        if (transform.position.y < -40)
        {
            transform.position = startPos;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
	}
}
