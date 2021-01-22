using UnityEngine;

public class Tail : MonoBehaviour
{


    public AudioClip cuttingClip;
    public AudioClip normalClip;

    void Start()
    {
        GetComponent<ConstantForce2D>().force *= Level.tailGravityModifier;
    }

    void Update()
    {
        if (transform.position.y > Level.player.transform.position.y + 25)
            Reset();
    }

    public virtual void Reset()
    {
        transform.position = new Vector3(0, Level.player.transform.position.y + 25);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == Level.player.name)
        {
            if (GetComponent<AudioSource>() != null)
                normalClip = GetComponent<AudioSource>().clip;
            if (cuttingClip == null)
                return;
            GetComponent<AudioSource>().clip = cuttingClip;
            GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == Level.player.name)
            Level.ApplyDamage(Time.fixedDeltaTime * 1.2f, 1);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == Level.player.name && !Level.player.dead)
        {
            if (normalClip == null)
                return;
            GetComponent<AudioSource>().clip = normalClip;
            GetComponent<AudioSource>().Play();
        }
    }
}
