using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Powerup : MonoBehaviour
{

    public int id = 0; //0-slomo
    public float startTime, maxTime;
    //public static int powerupsUsed;
    public bool canUse = true, running = false, timed;
    public GameObject display;

    public void Start()
    {
        startTime = 0;
        maxTime = 25;
        //GetComponentInChildren<Text>().text = (PlayerData.powerups[id]).ToString();
    }

    public void Refresh()
    {
        if (timed && running)
            GetComponentInChildren<Image>(true).fillAmount = 1 - (Level.gameTime - startTime) / maxTime;
    }

    public void OnGUI()
    {
        //GUI.Label(new Rect(0, 200, 200, 200), (Time.timeScale).ToString());
    }

    public IEnumerator Apply()
    {
        if (canUse)
        {
            GetComponentInChildren<Text>().text = "0";
            GetComponentInChildren<Image>(true).gameObject.SetActive(true);
            GetComponentInChildren<Image>(true).fillAmount = 1;
            GetComponent<RawImage>().color = new Color(GetComponent<RawImage>().color.r, GetComponent<RawImage>().color.g, GetComponent<RawImage>().color.b, 1);
            canUse = false;
            running = true;
            if (display != null)
                display.transform.parent.gameObject.SetActive(true);
            switch (id)
            {
                case 0:
                    Level.SetTimeScale(0.6f);
                    Level.level.powerup1 = true;
                    if (Level.level.powerup2)
                        ((RectTransform)display.transform.parent).anchoredPosition = Level.level.powerupSlot2.anchoredPosition;
                    startTime = Level.gameTime;
                    maxTime = 15 / Level.gravityModifier;
                    display.SetActive(true);
                    yield return new WaitUntil(() => Level.gameTime > startTime + maxTime);
                    Level.level.powerup1 = false;
                    if (!Level.player.dead)
                    Level.SetTimeScale(1);
                    break;
                case 2:
                    Level.level.powerup2 = true;
                    if (!Level.level.powerup1)
                        ((RectTransform)display.transform.parent).anchoredPosition = Level.level.powerupSlot1.anchoredPosition;
                    yield return new WaitUntil(() => !running);
                    Level.level.powerup2 = false;
                    break;
                default:
                    yield return null;
                    break;
            }
            if (display != null)
                display.transform.parent.gameObject.SetActive(false);
            running = false;
            GetComponentInChildren<Image>(true).gameObject.SetActive(false);
            GetComponent<RawImage>().color = new Color(GetComponent<RawImage>().color.r, GetComponent<RawImage>().color.g, GetComponent<RawImage>().color.b, .4f);
        }
    }
}
