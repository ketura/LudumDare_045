using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
  public SpriteRenderer Bounds;
  public LineRenderer SelectionBox;

  public float ZoomIncrement = 1;
	public float MinZoomSize = 5;
	public float MaxZoomSize = 10;
	public bool FlipX;
	public bool FlipY;
	public float PanSpeed = 0.1f;

  public float XOffset = 0;
  public float YOffset = 0;
  public float Z = -10.0f;

  private float WorldXOffset;
  private float WorldYOffset;

  [HideInInspector]
  public float PixelsToWorld;
  [HideInInspector]
  public float WorldToPixels;


  private Camera cam;

	//private float PanFactor = 0.1f;

	void Start()
	{
    cam = GetComponent<Camera>();
    PixelsToWorld = (Camera.main.orthographicSize / (Screen.height / 2.0f));
    WorldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
    Debug.Log("Pixels to World factor: " + PixelsToWorld);
  }

  public void SetCameraPosition(Vector2 pos)
  {
    UpdateOffsets();
    transform.position = new Vector3(pos.x - WorldXOffset/2, pos.y - WorldYOffset/2, Z);
    LimitCamera();
  }

	private void SizeCameraToFit()
	{
		Vector3 gridSize = Bounds.sprite.bounds.size;
		gridSize.x *= Bounds.transform.localScale.x;
		gridSize.y *= Bounds.transform.localScale.y;

		float factor = 1.0f;
		float smallest = Mathf.Min(gridSize.x, gridSize.y);
		if (smallest == gridSize.x)
			factor = cam.aspect;

		if (smallest < cam.orthographicSize * factor * 2)
			cam.orthographicSize = smallest / (2 * factor);
	}

  private void UpdateOffsets()
  {
    WorldXOffset = XOffset * PixelsToWorld * cam.orthographicSize;
    WorldYOffset = YOffset * PixelsToWorld * cam.orthographicSize;
  }

	void Update()
	{
		SizeCameraToFit();
		LimitCamera();
    float width = 0.005f * cam.orthographicSize;
    SelectionBox.SetWidth(width, width);
	}

	private void LimitCamera()
	{
    UpdateOffsets();
    Vector3 camPos = cam.transform.position;
		Vector3 gridPos = Bounds.gameObject.transform.position;
		Vector3 gridSize = Bounds.sprite.bounds.size;

		gridSize.x *= Bounds.transform.localScale.x;
		gridSize.y *= Bounds.transform.localScale.y;
    //gridSize.y += WorldYOffset;
    //gridSize.x += WorldXOffset;

    //Y top
    if (camPos.y + cam.orthographicSize > gridPos.y + gridSize.y / 2)
			camPos.y = gridPos.y + gridSize.y / 2 - cam.orthographicSize;
    //Y bottom
    if (camPos.y - cam.orthographicSize < (gridPos.y - gridSize.y / 2) - WorldYOffset)
      camPos.y = gridPos.y - gridSize.y / 2 + cam.orthographicSize - WorldYOffset;
    //X right
		if (camPos.x + cam.orthographicSize * cam.aspect > gridPos.x + gridSize.x / 2)
			camPos.x = gridPos.x + gridSize.x / 2 - cam.orthographicSize * cam.aspect;
    //X left
		if (camPos.x - cam.orthographicSize * cam.aspect < (gridPos.x - gridSize.x / 2) - WorldXOffset)
			camPos.x = gridPos.x - gridSize.x / 2 + cam.orthographicSize * cam.aspect - WorldXOffset;

		cam.transform.position = camPos;
	}

	public void Zoom(float steps)
	{
		//Debug.Log("Zooming: " + steps);
		cam.orthographicSize += ZoomIncrement * Mathf.Sign(steps) * -1;

		if (cam.orthographicSize < MinZoomSize)
			cam.orthographicSize = MinZoomSize;
		else if (cam.orthographicSize > MaxZoomSize)
			cam.orthographicSize = MaxZoomSize;


		SizeCameraToFit();
		LimitCamera();
	}

	public void Pan(Vector2 delta, Vector3 currentMousePos)
	{

		float pixelFactor = PanSpeed * cam.orthographicSize / cam.aspect;
    Vector2 factor = new Vector2(pixelFactor, pixelFactor);


		if (FlipX)
			factor.x *= -1;
		if (FlipY)
			factor.y *= -1;

		delta.x *= factor.x;
		delta.y *= factor.y;

		transform.position = transform.position + (Vector3)delta;
		LimitCamera();
	}
}
