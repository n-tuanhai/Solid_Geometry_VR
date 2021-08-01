using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WhiteboardPen : VRTK_InteractableObject
{
	public Whiteboard whiteboard;
	private RaycastHit _touch;
	private bool _lastTouch;
	private Quaternion _lastTouchAngle;
	private VRTK_ControllerReference _controller;
	public float vibrationStr = 0.05f;
	private int _penSize;

	[SerializeField] private GameObject penTip;
	private CustomizePen _customizePen;
	private Rigidbody _rigidbody;

	//Start is called before the first frame update
	void Start()
	{
		this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
		_customizePen = GetComponentInChildren<CustomizePen>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	//Update is called once per frame
	protected override void Update()
	{
		base.Update();
		
		float tipHeight = penTip.transform.localScale.y;
		Vector3 tip = penTip.transform.position;

		if (_lastTouch)
		{
			tipHeight *= 1.2f;
		}

		//if pen tip is touching something
		if (Physics.Raycast(tip, transform.up, out _touch, tipHeight))
		{
			if (!(_touch.collider.CompareTag("Whiteboard")))
			{
				return;
			}
			this.whiteboard = _touch.collider.GetComponent<Whiteboard>();
			this.whiteboard.SetColor(_customizePen.GetColor());
			VRTK_ControllerHaptics.TriggerHapticPulse(_controller, vibrationStr);
			
			_penSize = _customizePen.GetPenSize();
			this.whiteboard.SetPenSize(_penSize, _penSize);
			this.whiteboard.SetTouchPosition(_touch.textureCoord.x, _touch.textureCoord.y);
			this.whiteboard.ToggleTouch(true);
			//Debug.Log(touch.distance);

			if (!_lastTouch)
			{
				_lastTouch = true;
			}
		}
		else
		{
			_lastTouch = false;
			_rigidbody.freezeRotation = false;
			this.whiteboard.ToggleTouch(false);
		}

		if (_lastTouch)
		{
			_rigidbody.freezeRotation = true;
		}
	}

	public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
	{
		base.Grabbed(currentGrabbingObject);
		_controller = VRTK_ControllerReference.GetControllerReference(currentGrabbingObject.gameObject);
	}
}
