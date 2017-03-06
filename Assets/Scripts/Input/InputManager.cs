using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public Human MainCharacter = null;
	public static InputManager Instance = null;
	public Texture2D selectedMouseTexture = null;
	void Awake()
	{
		Instance = this;

	}
	public void SetSelected()
	{
		var v = new Vector2 (selectedMouseTexture.width / 2, selectedMouseTexture.height / 2);
		Cursor.SetCursor(selectedMouseTexture, v, CursorMode.Auto);
	}
	public void SetUnSelected()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	void Update()
	{
		
	}
}
