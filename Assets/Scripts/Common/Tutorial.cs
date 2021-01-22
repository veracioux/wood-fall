using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public int index;
    public static string data = "1111111";
    public GameObject[] items, dialogs;

    void Start()
    { 
        int id = Convert.ToInt32(data[index].ToString()) - 1;
        if (id >= dialogs.Length)
            gameObject.SetActive(false);
        Trigger(id);
    }

    public void Trigger(int id)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        PlayerPrefs.SetString("tut", data = data.Remove(index, 1).Insert(index, (id + 1).ToString()));
        if (id >= dialogs.Length)
        {
            gameObject.SetActive(false);
            return;
        }
        if (!items[id].activeSelf)
        {
            if (id > 0)
            dialogs[id - 1].SetActive(false);
            dialogs[id].SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        if (id > 0)
        {
            transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            dialogs[id - 1].SetActive(false);
        }
        items[id].transform.SetSiblingIndex(transform.GetSiblingIndex());
        dialogs[id].SetActive(true);
    }

    public void Disable()
    {
        Trigger(dialogs.Length);
    }

    public void DisableAll()
    {
        PlayerPrefs.SetString("tut", data = "9999999");
        gameObject.SetActive(false);
    }
}
