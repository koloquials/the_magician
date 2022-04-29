using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;
using Yarn.Unity.Editor;

namespace TheMagician
{
    public class MouseController : MonoBehaviour
    {
        [SerializeField] LayerMask interactableLayerMask;
        [SerializeField] UnityEvent onClickDuringGameplay;
        [SerializeField] UnityEvent onPickupItem;
        [SerializeField] UnityEvent onReleaseItem;
        [SerializeField] UnityEvent onClickDuringDialogue;

        enum GameplayInteractionState
        {
            NONE,
            HOLDING_ITEM
        }

        GameplayInteractionState _gameplayInteractionState;

        RaycastHit2D _raycastHit2D;

        Interactable _interactable;
        Interactable _interactableBeingHovered;
        Vector3 _holdOffset;
        Vector3 _position;

        public Vector3 Position => _position;

        public static MouseController INSTANCE;

        private void Awake()
        {
            if(!INSTANCE) INSTANCE = this;
        }

        private void Start()
        {
            _gameplayInteractionState = GameplayInteractionState.NONE;
            _interactable = null;
            GameStateManager.OnUnpause.AddListener(HandleHeldInteractableAfterUnpause);
        }

        private void OnDestroy()
        {
            GameStateManager.OnUnpause.RemoveListener(HandleHeldInteractableAfterUnpause);
        }

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            _position = Camera.main.ScreenToWorldPoint(mousePos);

            // Gameplay (interacting with objects) code
            if(GameStateManager.INSTANCE.CurrentGameState == GameState.GAMEPLAY)
            {
                Ray screenPointToRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                _raycastHit2D = Physics2D.Raycast(screenPointToRay.origin, screenPointToRay.direction, Mathf.Infinity, interactableLayerMask);

                // Check for glow
                if (_raycastHit2D && (_gameplayInteractionState == GameplayInteractionState.NONE))
                {
                    Interactable interactable = _raycastHit2D.collider.gameObject.GetComponent<Interactable>();

                    if (_interactableBeingHovered != interactable)
                    {
                        if(_interactableBeingHovered) _interactableBeingHovered.Unglow();
                        _interactableBeingHovered = interactable;
                        _interactableBeingHovered.Glow();
                    }
                }
                else
                {
                    if (_interactableBeingHovered)
                    {
                        _interactableBeingHovered.Unglow();
                        _interactableBeingHovered = null;
                    }
                }

                // Check for picking up
                if (Input.GetMouseButtonDown(0))
                {
                    onClickDuringGameplay.Invoke();

                    if(_gameplayInteractionState == GameplayInteractionState.NONE)
                    {
                        if (_raycastHit2D)
                        {
                            _interactable = _raycastHit2D.collider.gameObject.GetComponent<Interactable>();
                            if(_interactable.PickUp())
                            {
                                _holdOffset = _interactable.transform.position - _position;
                                _gameplayInteractionState = GameplayInteractionState.HOLDING_ITEM;
                                onPickupItem?.Invoke();
                                if (_interactableBeingHovered)
                                {
                                    _interactableBeingHovered.Unglow();
                                    _interactableBeingHovered = null;
                                }
                            }
                            else
                            {
                                _interactable = null;
                            }
                        }
                    }
                }

                // Check for releasing
                if (Input.GetMouseButtonUp(0))
                {
                    if(_gameplayInteractionState == GameplayInteractionState.HOLDING_ITEM)
                    {
                        if(_interactable)
                        {
                            // Reset interactble item's position if not dropped successfully
                            if (!_interactable.Dropped())
                            {
                                _interactable.ResetPositionAndRotation();
                            }
                            else
                            {
                                _interactable.Destroy();
                            }
                            _interactable = null;
                            onReleaseItem?.Invoke();
                        }

                        _gameplayInteractionState = GameplayInteractionState.NONE;
                    }
                }

                if(_gameplayInteractionState == GameplayInteractionState.HOLDING_ITEM)
                {
                    _interactable.gameObject.transform.position = _position + _holdOffset;
                }
            }
            else if(GameStateManager.INSTANCE.CurrentGameState == GameState.DIALOGUE) // Just click during dialogue to proceed
            {
                if (Input.GetMouseButtonDown(0))
                {
                    onClickDuringDialogue.Invoke();
                }
            }
        }

        // Function name says it all
        public void HandleHeldInteractableAfterUnpause()
        {
            if(GameStateManager.INSTANCE.CurrentGameState == GameState.GAMEPLAY && _gameplayInteractionState == GameplayInteractionState.HOLDING_ITEM)
            {
                if(_interactable && !Input.GetMouseButton(0)) // I guess the former is more of a double check
                {
                    // Reset interactble item's position if not dropped successfully
                    if (!_interactable.Dropped())
                    {
                        _interactable.ResetPositionAndRotation();
                    }
                    else
                    {
                        _interactable.Destroy();
                    }
                    _interactable = null;
                    onReleaseItem?.Invoke();
                    _gameplayInteractionState = GameplayInteractionState.NONE;
                }
            }
        }

        // Set in inspector, subscribed as an event
        public void ReleaseInteractable()
        {
            if (GameStateManager.INSTANCE.CurrentGameState == GameState.GAMEPLAY && _gameplayInteractionState == GameplayInteractionState.HOLDING_ITEM)
            {
                if (_interactable)
                {
                    _interactable = null;
                    _gameplayInteractionState = GameplayInteractionState.NONE;
                }
            }
        }
    }
}
