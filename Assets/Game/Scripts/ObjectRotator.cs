using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour
{

	private float _sensitivity;
	private Vector3 _mouseReference;
	private Vector3 _mouseOffset;
	private Vector3 _rotation;
	private bool _isRotating;
	public GameObject objectRotate;
	public Camera uiCamera;

	void Start ()
	{
		_sensitivity = 0.1f;
		_rotation = Vector3.zero;
	}

	void Update ()
	{
		if (_isRotating) {
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

			// apply rotation
			_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

			// rotate
			objectRotate.transform.Rotate (_rotation);

			// store mouse
			_mouseReference = Input.mousePosition;
		}

		if (Input.GetMouseButtonDown (0)) {

			_isRotating = true;
			_mouseReference = Input.mousePosition;

		}

		if (Input.GetMouseButtonUp (0)) {
			_isRotating = false;
		}
	}
		

}