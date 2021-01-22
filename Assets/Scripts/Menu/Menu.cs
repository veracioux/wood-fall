using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public ParticleSystem touchParticles;
    public Camera camera;
    Vector2 rawGravity;

    void Start()
    {
        DisableAnalytics.disableAnalytics();
        rawGravity = new Vector2(0, -9.81f);
        Physics2D.gravity = rawGravity;
        Physics.gravity = rawGravity;
        Time.timeScale = 1;
        Time.fixedDeltaTime = .02f;
    }

	void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 v = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
            if (v.y / v.magnitude < Mathf.Sin(Mathf.PI / .64f))
                Physics2D.gravity = v * 10 / v.magnitude;
            else
                Physics2D.gravity = new Vector2(Mathf.Sign(v.x) * Mathf.Cos(Mathf.PI / .64f), Mathf.Sin(Mathf.PI / .64f)) * 10;
            Physics.gravity = Physics2D.gravity;
            if (touchParticles != null)
            {
                touchParticles.gameObject.transform.Translate(camera.ScreenToWorldPoint(Input.mousePosition) - touchParticles.gameObject.transform.position);
                touchParticles.Play();
            }
        }
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(100, 200, 100, 100), Settings.soundVolume.ToString());
    }

    public void OpenYouTube()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCUm4fykOgcBm7WN9srq5Glg");
    }

    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/woodfallgame/");
    }

    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/WoodFallGame");
    }

    public void OpenPolicy()
    {
        Application.OpenURL("https://wood-fall.flycricket.io/privacy.html");
    }
}