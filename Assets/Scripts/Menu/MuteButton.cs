using UnityEngine;

public class MuteButton : MonoBehaviour {

    public GameObject other;

    void Start()
    {
        if (gameObject.name == "Mute")
            gameObject.SetActive(!AudioListener.pause);
        else
            gameObject.SetActive(AudioListener.pause);
    }

    public void Mute()
    {
        Settings.Refresh();
        gameObject.SetActive(false);
        other.SetActive(true);
    }
}
