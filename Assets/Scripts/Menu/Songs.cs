using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Songs : MonoBehaviour
{

    public static int songId = 0;
    public string[] songNames,
        artistNames,
        songYouTube,
        songSoundcloud,
        artistFacebook,
        NCSYouTube,
        NCSSoundcloud;
    public string NCSFacebook, NCSTwitter;
    public bool[] NCSRelease;
    public Text songName, artistName;
    public GameObject NCS;

	void Start()
    {
        songId = Settings.music.current;
        songName.text = songNames[songId];
        artistName.text = artistNames[songId];
        NCS.SetActive(NCSRelease[songId]);
    }
	
	void Update()
    {
        if (songId != Settings.music.current)
        {
            songId = Settings.music.current;
            songName.text = songNames[songId];
            artistName.text = artistNames[songId];
            NCS.SetActive(NCSRelease[songId]);
        }
	}

    public void Next()
    {
        songId++;
        songId %= songNames.Length;
        Settings.music.Next();
        songName.text = songNames[songId];
        artistName.text = artistNames[songId];
        NCS.SetActive(NCSRelease[songId]);
    }
    public void Prev()
    {
        songId += songNames.Length - 1;
        songId %= songNames.Length;
        Settings.music.Prev();
        songName.text = songNames[songId];
        artistName.text = artistNames[songId];
        NCS.SetActive(NCSRelease[songId]);
    }

    public void OpenSongYouTube()
    {
        Application.OpenURL("https://www.youtube.com/" + songYouTube[songId]);
    }

    public void OpenSongSoundcloud()
    {
        Application.OpenURL("https://soundcloud.com/" + songSoundcloud[songId]);
    }

    public void OpenArtistFacebook()
    {
        Application.OpenURL("https://www.facebook.com/" + artistFacebook[songId]);
    }

    public void OpenNCSYouTube()
    {
        Application.OpenURL("https://www.youtube.com/" + NCSYouTube[songId]);
    }

    public void OpenNCSSoundcloud()
    {
        Application.OpenURL("https://soundcloud.com/" + NCSSoundcloud[songId]);
    }

    public void OpenNCSFacebook()
    {
        Application.OpenURL("https://www.facebook.com/" + NCSFacebook);
    }
    public void OpenNCSTwitter()
    {
        Application.OpenURL("https://twitter.com/" +NCSTwitter);
    }

}
