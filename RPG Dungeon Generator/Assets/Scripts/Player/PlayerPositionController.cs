using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

using CaptainCoder.Extensions;

namespace CaptainCoder.Dungeoneering
{
    public class PlayerPositionController : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<Facing> OnChangeFacing { get; private set; } = new();
        [SerializeField]
        private float _eyeLevel = 2.5f;
        [SerializeField]
        private Facing _facing = Facing.North;
        public Facing Facing
        {
            get => _facing;
            set
            {
                _facing = value;
                OnChangeFacing.Invoke(_facing);
            }
        }
        [SerializeField]
        private int _x;
        public int X
        {
            get => _x;
            set
            {
                _x = value;
                UpdatePosition();
            }
        }

        [SerializeField]
        private int _y;

        public int Y
        {
            get => _y;
            set
            {
                _y = value;
                UpdatePosition();
            }
        }

        protected void Awake()
        {
            UpdatePosition();
        }

        private void OnEnable()
        {
            Controls.MovementActions.Movement.started += Move;
            Controls.MovementActions.Rotate.started += Rotate;
        }

        private void OnDisable()
        {
            Controls.MovementActions.Movement.started -= Move;
            Controls.MovementActions.Rotate.started -= Rotate;
        }

        protected void OnValidate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            transform.position = new Vector3(_x * Grid.CellSize, _eyeLevel, _y * Grid.CellSize);
        }

        private void Move(CallbackContext context)
        {
            Vector2Int move = context.ReadValue<Vector2>().Ceil();
            Vector2Int translated = _facing switch
            {
                Facing.North => new Vector2Int(move.x, move.y),
                Facing.East => new Vector2Int(move.y, move.x),
                Facing.South => new Vector2Int(-move.x, -move.y),
                Facing.West => new Vector2Int(-move.y, -move.x),
                _ => throw new System.InvalidOperationException($"Invalid facing detected {_facing}"),
            };
            X += translated.x;
            Y += translated.y;
        }

        private void Rotate(CallbackContext context)
        {
            int input = (int)Mathf.Sign(context.ReadValue<float>());
            Facing = input == 1 ? _facing.RotateClockwise() : _facing.RotateCounterClockwise();
        }
    }
}