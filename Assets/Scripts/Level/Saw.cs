using UnityEngine;

public class Saw : MonoBehaviour
{

    float bladeAngle = 13.166f;
    Vector2 lastPos;
    bool damage = true;

    void Start()
    {

    }


    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            /*if (Level.level.powerups[2].running)
            {
                damage = false;
                Level.level.powerups[2].running = false;
                return;
            }*/
            lastPos = collider.gameObject.transform.position;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            Level.player.particleSystem.gameObject.transform.position = collision.contacts[0].point;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player" && damage)
        {
            Vector2 delta = (Vector2)collider.gameObject.transform.position - lastPos;
            float angle = Mathf.Atan2(delta.y, delta.x) - (transform.eulerAngles.z + bladeAngle) * Mathf.Deg2Rad;
            float dmg = delta.magnitude * Mathf.Cos(angle) * Time.fixedDeltaTime * 3.5f;
            Level.ApplyDamage(Mathf.Abs(dmg), 1);
            lastPos += delta;
            if (!Sound.playingSaw)
                Sound.PlaySaw();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Settings.sound.audioSources[1].Pause();
            Sound.playingSaw = false;
        }
    }
}
