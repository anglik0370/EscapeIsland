using UnityEngine;
using System.Collections;

public class ParticleDisplayer : MonoBehaviour
{
	[SerializeField]
	private Camera effectCam;

	public RectTransform imageTransform;

    private void Start()
    {
		//effectCam.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public void ResetPosition()
	{
		imageTransform.anchoredPosition = Vector2.zero;
	}

	public void MoveToPosition(Vector3 pos)
	{
		imageTransform.position = pos;
		//effectCam.transform.position = pos;
	}

}
