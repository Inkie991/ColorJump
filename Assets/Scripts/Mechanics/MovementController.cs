using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    
    private bool _isLocked;
    private bool _isMoving;
    private bool _isGameStarted;
    private Vector2 _lastTouchPosition;
    private float desktopSpeed = 2000f;
    private Player _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _player.Died += (sender, args) => _isLocked = true;
        _player.Won += (sender, args) => _isLocked = true;
        _isLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLocked) return;
        
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UpdateMobile();
        }
        else
        {
            UpdateDesktop();
        }
    }

    void UpdateMobile()
    {
        if (Input.touchCount == 0) return;
        // TODO: FIXME
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
        
        Touch touch = Input.GetTouch(0);
        float rotationValue = touch.deltaPosition.x * speed * Time.deltaTime * -1;
        transform.rotation *= Quaternion.Euler(Vector3.up * rotationValue);

        if (!_isGameStarted && rotationValue > 0)
        {
            Managers.UI.ToggleSettings(false);
        }
    }

    void UpdateDesktop()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        CheckIsMoving();

        if (!_isMoving) return;

        CheckMovementDistance();
    }

    void CheckIsMoving()
    {
        _isMoving = Input.GetMouseButton(0);

        if (!_isGameStarted && _isMoving)
        {
            _isGameStarted = true;
            Managers.UI.ToggleSettings(false);
        }
    }

    void CheckMovementDistance()
    {
        float rotationValue = Input.GetAxis("Mouse X") * desktopSpeed * Time.deltaTime * -1;
        transform.rotation *= Quaternion.Euler(Vector3.up * rotationValue);
    }
}
