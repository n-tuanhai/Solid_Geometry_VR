using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WhiteboardEraser : VRTK_InteractableObject
{
    public Whiteboard whiteboard;
	public int eraseTipWidth = 25;
	public int eraseTipHeight = 80;
    private RaycastHit touch;
    private bool lastTouch;
	private Color eraseColor;
    private Quaternion lastTouchAngle;
    private VRTK_ControllerReference controller;
	private Texture2D texture;

	public float vibrationStr = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
		texture = this.whiteboard.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
		eraseColor = texture.GetPixels32()[0];
	}

	// Update is called once per frame

	void Update()
    {
        float tipHeight = transform.Find("EraseTip").localScale.y;
        Vector3 tip = transform.Find("EraseTip").transform.position;

		if (lastTouch)
        {
            tipHeight *= 1.1f;
        }

		//if pen tip is touching something
		if (Physics.Raycast(tip, transform.up, out touch, tipHeight))
		{
			if (!(touch.collider.CompareTag("Whiteboard")))
			{
				return;
			}
			this.whiteboard = touch.collider.GetComponent<Whiteboard>();
			this.whiteboard.SetPenSize(eraseTipWidth,eraseTipHeight);
			this.whiteboard.SetColor(eraseColor);
			this.whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
			this.whiteboard.ToggleTouch(true);

			if (!lastTouch)
			{
				lastTouch = true;
			}
		}
		else
		{
			lastTouch = false;
			GetComponent<Rigidbody>().freezeRotation = false;
			this.whiteboard.ToggleTouch(false);
		}

		if (lastTouch)
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
		public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
	{
		base.Grabbed(currentGrabbingObject);
		controller = VRTK_ControllerReference.GetControllerReference(currentGrabbingObject.gameObject);
	}
}
