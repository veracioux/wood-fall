using UnityEngine;

public class Player : MonoBehaviour
{
    public bool dead;
    public ParticleSystem particleSystem;
    public CircleCollider2D offscreen;

    void Start()
    {
    }

    void FixedUpdate()
    {
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!dead && !collider.isTrigger && collider.gameObject.transform.position.y - transform.position.y > 0 && !offscreen.IsTouching(collider))
        {
            DestroyObject(collider.gameObject);
        }
    }
}
