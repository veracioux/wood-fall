using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public float time;
    public GameObject playButton;
    public ParticleSystem particles;

	void Start()
    {
        StartCoroutine(StartControl());
	}

    IEnumerator StartControl()
    {
        yield return new WaitForSeconds(time - .3f);
        particles.gameObject.transform.position = new Vector3(0, 4, 0);
        particles.Play();
        yield return new WaitForSeconds(.3f);
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(playButton, pointer, ExecuteEvents.pointerClickHandler);
    }
	
	void Update () {
		
	}
}
