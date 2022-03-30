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

        List<RaycastHit2D> _raycastHits;
        GameplayInteractionState _gameplayInteractionState;
        Interactable _interactableBeingHeld;
        RaycastHit2D _raycastHit2D;
        Vector3 _holdOffset;
        Vector3 _interactableOriginalPosition;
        Vector3 _position;

        public Vector3 Position => _position;

        private void Start()
        {
            _raycastHits = new List<RaycastHit2D>();
            _gameplayInteractionState = GameplayInteractionState.NONE;
            _interactableBeingHeld = null;
        }

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            _position = Camera.main.ScreenToWorldPoint(mousePos);

            if(GameStateManager.INSTANCE.CurrentGameState == GameState.GAMEPLAY)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    onClickDuringGameplay.Invoke();

                    if(_gameplayInteractionState == GameplayInteractionState.NONE)
                    {
                        Ray screenPointToRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                        _raycastHit2D = Physics2D.Raycast(screenPointToRay.origin, screenPointToRay.direction, Mathf.Infinity, interactableLayerMask);

                        if (_raycastHit2D)
                        {
                            _interactableBeingHeld = _raycastHit2D.collider.gameObject.GetComponent<Interactable>();
                            _interactableOriginalPosition = _raycastHit2D.collider.gameObject.transform.position;
                            _holdOffset = _raycastHit2D.collider.gameObject.transform.position - _position;
                            _gameplayInteractionState = GameplayInteractionState.HOLDING_ITEM;
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if(_gameplayInteractionState == GameplayInteractionState.HOLDING_ITEM)
                    {
                        if(_interactableBeingHeld)
                        {
                            // Reset interactble item's position if not dropped successfully
                            if (!_interactableBeingHeld.Dropped())
                            {
                                _interactableBeingHeld.transform.position = _interactableOriginalPosition;
                            }
                            else
                            {
                                _interactableBeingHeld.Destroy();
                            }
                            _interactableBeingHeld = null;
                        }

                        _gameplayInteractionState = GameplayInteractionState.NONE;
                    }
                }

                if(_gameplayInteractionState == GameplayInteractionState.HOLDING_ITEM)
                {
                    _interactableBeingHeld.gameObject.transform.position = _position + _holdOffset;
                }
            }
            else if(GameStateManager.INSTANCE.CurrentGameState == GameState.DIALOGUE)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    onClickDuringDialogue.Invoke();
                }
            }
        }
    }
}
