using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;

    [ContextMenu("ZoomIn")]
    public void ZoomToPlayer() {
        StartCoroutine(1f.Tweeng((p)=>transform.position=p, transform.position, new Vector3(player.position.x,player.position.y + 0.2f * player.localScale.x, -10)));
        StartCoroutine(1f.Tweeng((s)=>GetComponent<Camera>().orthographicSize=s, 5f, 2f));
    }

    [ContextMenu("ZoomOut")]
    public void ZoomOut() {
        StartCoroutine(.5f.Tweeng((p)=>transform.position=p, transform.position, new Vector3(0,0,-10)));
        StartCoroutine(.5f.Tweeng((s)=>GetComponent<Camera>().orthographicSize=s, 2f, 5f));
    }

    public void ZoomOutSlow() {
        StartCoroutine(2f.Tweeng((p)=>transform.position=p, transform.position, new Vector3(0,0,-10)));
        StartCoroutine(2f.Tweeng((s)=>GetComponent<Camera>().orthographicSize=s, 2f, 5f));
    }
}
