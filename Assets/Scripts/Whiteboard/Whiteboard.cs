using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
	private int textureSize = 2048;
	private int penWidth = 10;
	private int penHeight = 10;
	private Texture2D texture;
	private Color[] colors;
	private bool touching, touchingLast;
	private float posX, posY;
	private float lastPosX, lastPosY; 

	// Start is called before the first frame update
	void Start()
	{
		Renderer renderer = GetComponent<Renderer>();
		this.texture = new Texture2D(textureSize, textureSize,TextureFormat.RGBA32, false, true);
		renderer.material.mainTexture = (Texture)texture;

	}

	// Update is called once per frame
	void Update()
	{
		int x = (int)(posX * textureSize - (penWidth / 2f));
		int y = (int)(posY * textureSize - (penHeight / 2f));

		if (touchingLast) {
			texture.SetPixels(x, y, penWidth, penHeight,colors);
			for (float t = 0.01f; t < 1.0f; t += 0.01f)
			{
				//interpolating between curPoint and last frame point for smooth stroke
				int lerpX = (int)Mathf.Lerp(lastPosX, (float)x, t);
				int lerpY = (int)Mathf.Lerp(lastPosY, (float)y, t);
				texture.SetPixels(lerpX, lerpY, penWidth, penHeight, colors);

			}
			texture.Apply();
		}

		this.lastPosX = (float)x;
		this.lastPosY = (float)y;
		this.touchingLast = touching;
	}

	public void ToggleTouch(bool touching) {
		this.touching = touching;
	}

	public void SetTouchPosition(float x, float y) {
		this.posX = x;
		this.posY = y;
	}
	public void SetColor(Color color) {
		this.colors = Enumerable.Repeat<Color>(color, penWidth * penHeight).ToArray<Color>();
	}

	public void SetPenSize(int width, int height) {
		this.penWidth = width;
		this.penHeight = height;
	}
}
