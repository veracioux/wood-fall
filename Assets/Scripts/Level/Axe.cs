using UnityEngine;

public class Axe : MonoBehaviour
{

    bool collided;
    public HingeJoint2D hinge;

    void Start()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && GetComponents<PolygonCollider2D>()[1].IsTouching(collision.collider) && !collided)
        {
            collided = true;
            Sound.PlayAxeChop();
            Level.player.particleSystem.gameObject.transform.position = collision.contacts[0].point;
            Level.ApplyDamage(0.5f);
        }
    }
}
