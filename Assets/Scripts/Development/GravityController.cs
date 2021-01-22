using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityController : MonoBehaviour {

    //bool slowing = false;
    /*public GameObject title;
    public SpriteRenderer[] renderers;
    public Image[] images;*/
    bool OK = false, BAD = false;
    public GameObject[] marks;
    Vector2 grav = Vector2.zero;
    public Powerup powerup1, powerup2;
    float time = 0, timeDead = 0;

	void Start ()
    {
        StartControl();
	}

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }

	void Update ()
    {
        if (Level.player.dead)
        {
            Level.player.GetComponent<Rigidbody2D>().drag = 20;
            Level.player.GetComponent<Rigidbody2D>().gravityScale = 20;
            if (timeDead == 0)
                timeDead = Level.gameTime;
            if (Level.level.stars != 11)
                BAD = true;
            else if (Level.gameTime - timeDead >= 5)
                OK = true;
        }
        /*if (slowing && Time.timeScale > 0)
            Level.SetTimeScale(Time.timeScale - 0.5f * Time.unscaledDeltaTime >= 0 ? Time.timeScale - 0.75f * Time.unscaledDeltaTime : 0);
        if (Level.gameTime - Level.time < 20f)
            return;
        GameObject[] m = GameObject.FindGameObjectsWithTag("Finish");
        for (int i = 0; i < m.Length; i++)
        {
            m[i].GetComponent<SpriteRenderer>().color = new Color(m[i].GetComponent<SpriteRenderer>().color.r, m[i].GetComponent<SpriteRenderer>().color.g, m[i].GetComponent<SpriteRenderer>().color.b, m[i].GetComponent<SpriteRenderer>().color.a - 1.1f * Time.unscaledDeltaTime);
        }
        if (Level.gameTime - Level.time < 21f)
            return;

        Level.player.GetComponent<Rigidbody2D>().freezeRotation = true;
        Physics2D.gravity = new Vector2(0, -10);
        Level.SetTimeScale(1);
        title.SetActive(true);
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, renderers[i].color.a - 0.9f * Time.unscaledDeltaTime);
        for (int i = 0; i < images.Length; i++)
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, images[i].color.a + 0.9f * Time.unscaledDeltaTime);*/
    }

    void OnGUI()
    {
        if (OK)
        {
            GUIStyle gs = new GUIStyle();
            gs.fontSize = 100;
            gs.normal.textColor = Color.red;
            GUI.Label(new Rect(20, 20, 200, 200), "OK", gs);
        }
        else if (BAD)
        {
            GUIStyle gs = new GUIStyle();
            gs.fontSize = 100;
            gs.normal.textColor = Color.red;
            GUI.Label(new Rect(20, 20, 200, 200), "BAD", gs);
        }
        //GUI.Label(new Rect(20, 20, 200, 200), Level.gameTime.ToString());
    }

    public void StartControl()
    {
        StartCoroutine(method());
    }

    IEnumerator method()
    {
        yield return new WaitForSecondsRealtime(1f);
        Level.level.OnStartPlaying();
        Level.SetTimeScale(1f);
        bool pause = false;
        Level.tail.GetComponent<ConstantForce2D>().force = Level.tail.GetComponent<ConstantForce2D>().force * 22f / 25f;
        for (int i = 1; i < marks.Length; i++)
        {
            if (i == 12)
            {
                Level.level.Pause();
                yield return new WaitForSecondsRealtime(.4f);
                StartCoroutine(powerup2.Apply());
                yield return new WaitForSecondsRealtime(.6f);
                Level.level.Unpause();
            }
            else if (i == 30)
            {
                Level.level.Pause();
                yield return new WaitForSecondsRealtime(.4f);
                StartCoroutine(powerup1.Apply());
                yield return new WaitForSecondsRealtime(.6f);
                Level.level.Unpause();
            }
            else if (i == 65)
            {
                Level.tail.GetComponent<ConstantForce2D>().force = Level.tail.GetComponent<ConstantForce2D>().force * 1.2f;
            }
            Physics2D.gravity = Physics.gravity = Level.player.GetComponent<Rigidbody2D>().velocity = 22 * (marks[i].transform.position - Level.player.transform.position) / (marks[i].transform.position - Level.player.transform.position).magnitude;
            yield return new WaitForSeconds((marks[i].transform.position - Level.player.transform.position).magnitude / 22f);
        }
        //Level.player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -10, 0);
        /*Physics2D.gravity = new Vector2(-2, -9.7979f);
        yield return new WaitForSeconds(1.45f);
        Physics2D.gravity = new Vector2(8.66f, -5);
        yield return new WaitForSeconds(.7f);
        Physics2D.gravity = new Vector2(-6, -8f);
        yield return new WaitForSeconds(.95f);
        Physics2D.gravity = new Vector2(9.7979f, -2);
        yield return new WaitForSeconds(.48f);
        Physics2D.gravity = new Vector2(-3, -9.5393f);
        yield return new WaitForSeconds(.6f);
        Physics2D.gravity = new Vector2(9.1651f, -4);
        yield return new WaitForSeconds(.7f);
        Physics2D.gravity = new Vector2(-2f, -9.7979f);
        yield return new WaitForSeconds(.8f);
        Level.SetTimeScale(1f);
        Physics2D.gravity = new Vector2(-8f, -6);
        yield return new WaitForSeconds(.83f);
        Physics2D.gravity = new Vector2(8, -6);
        yield return new WaitForSeconds(.5f);
        Physics2D.gravity = new Vector2(-4, -9.16515f);
        yield return new WaitForSeconds(.5f);
        Physics2D.gravity = new Vector2(2, -9.7979f);
        yield return new WaitForSeconds(.2f);
        Physics2D.gravity = new Vector2(-3, -9.5393f);
        yield return new WaitForSeconds(.7f);
        Physics2D.gravity = new Vector2(-6, -8f);
        yield return new WaitForSeconds(.4f);
        Physics2D.gravity = new Vector2(3, -9.5393f);
        yield return new WaitForSeconds(.8f);
        Level.SetTimeScale(1.4f);
        Physics2D.gravity = new Vector2(9.16515f, -4f);
        yield return new WaitForSeconds(1f);
        Physics2D.gravity = new Vector2(-7.07f, -7.07f);
        yield return new WaitForSeconds(1.86f);
        Physics2D.gravity = new Vector2(8.4f, -5.42f);
        yield return new WaitForSeconds(1f);
        Physics2D.gravity = new Vector2(5f, -8.66f);
        yield return new WaitForSeconds(.8f);
        Physics2D.gravity = new Vector2(-2f, -9.7979f);
        yield return new WaitForSeconds(.85f);
        Physics2D.gravity = new Vector2(-6f, -8f);
        yield return new WaitForSeconds(.6f);
        Level.SetTimeScale(1.8f);
        Physics2D.gravity = new Vector2(5f, -8.66f);
        yield return new WaitForSeconds(.4f);
        Physics2D.gravity = new Vector2(-6f, -8f);
        yield return new WaitForSeconds(.9f);
        Physics2D.gravity = new Vector2(6f, -8f);
        yield return new WaitForSeconds(.63f);
        Physics2D.gravity = new Vector2(-5.7236f, -8.2f);
        yield return new WaitForSeconds(.5f);
        Physics2D.gravity = new Vector2(1f, -9.95f);
        yield return new WaitForSeconds(1.05f);
        Physics2D.gravity = new Vector2(-8.7f, -4.93f);
        yield return new WaitForSeconds(.55f);
        Physics2D.gravity = new Vector2(0, -10f);
        slowing = true;*/
    }
}
