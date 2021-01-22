using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpectrumData : MonoBehaviour
{

    Image[] lines;

    void Start()
    {
        lines = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 0; i < spectrum.Length / 8 && Settings.musicVolume > 0; i++)
        {
            float y = Mathf.Lerp(lines[i].transform.localScale.y, Mathf.Min(2 * spectrum[i] / Settings.music.audioSource.volume + .01f, .5f), 5 * Time.deltaTime);
            lines[i].transform.localScale = new Vector3(1, y, 1);
            lines[i + lines.Length / 2].transform.localScale = new Vector3(1, y, 1);
        }
    }
}
