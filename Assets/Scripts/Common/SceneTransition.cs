using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

    public int index;
    public bool animateTransition;
    bool isTransitioning = false;
    public Animator animator;
    public GameObject letters, letters1;
    public float time;

    void Start()
    {
        isTransitioning = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex != 4 && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
                Transition(0);
            else if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
        }
    }

    public void Transition()
    {
        Transition(-1);
    }

    public void Transition(int id = -1)
    {
        if (id == -1)
            id = index;
        if (id == 1 && letters != null)
        {
            letters1.SetActive(false);
            letters.SetActive(true);
        }
        if (animateTransition)
        {
            isTransitioning = true;
            GameObject p = GameObject.Find("Particles");
            if (p != null)
                p.SetActive(false);
            animator.SetTrigger("Hide");
            StartCoroutine(_Transition(id));
        }
        else
            Load(id);
    }

    IEnumerator _Transition(int id = -1)
    {
        yield return new WaitForSecondsRealtime(time);
        Load(id);
    }

    void Load(int id = -1)
    {
        if (id == -1)
            id = index;
        if (id == 0)
        {
            if (Settings.music != null)
            {
                Settings.music.Unpause();
            }
        }
        AudioListener.pause = false;
        Time.timeScale = 0;
        SceneManager.LoadScene(id);
    }

}
