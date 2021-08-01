using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizePen : MonoBehaviour
{
	public float red, green, blue;
	public int penSize = 10;

	private Color color;

	// Start is called before the first frame update
	void Start()
	{
		color = new Color(red, green, blue);
	}

	public void RedChange(float value) {
		this.red = value;
		SetColor();
	}
	public void GreenChange(float value)
	{
		this.green = value;
		SetColor();
	}
	public void BlueChange(float value)
	{
		this.blue = value;
		SetColor();
	}
	public void PenSizeChange(float value)
	{
		this.penSize = (int)value;
	}

	private void SetColor() {
		color = GetComponent<Renderer>().sharedMaterial.color = new Color(red, green, blue);
	}

	public Color GetColor() {
		return this.color;
	}

	public int GetPenSize()
	{
		return this.penSize;
	}
}
