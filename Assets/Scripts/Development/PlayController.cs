using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayController : MonoBehaviour
{

    public float time;
    public ParticleSystem particles;
    public GameObject playButton, next;

	void Start () {
        StartCoroutine(StartControl());
	}

    IEnumerator StartControl()
    {
        yield return new WaitForSeconds(time - 1.3f);
        particles.gameObject.transform.position = new Vector3(-7f, 2.3f, 0);
        particles.Play();
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(next, pointer, ExecuteEvents.pointerClickHandler);
        yield return new WaitForSeconds(1f);
        particles.gameObject.transform.position = new Vector3(0, 3, 0);
        particles.Play();
        yield return new WaitForSeconds(.3f);
        ExecuteEvents.Execute(playButton, pointer, ExecuteEvents.pointerClickHandler);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
