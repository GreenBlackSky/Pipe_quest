using System;
using System.Collections.Generic;
using UnityEngine;


public enum Direction {UP, RIGHT, DOWN, LEFT, STOP}

public class MovingHero : MonoBehaviour {
    public Animator animator;
    public Rigidbody body;

    public float speed;
    private bool _movingX;
    private bool _movingY;
    private Direction _direction;
    private Dictionary<Direction, string> _directionAnimation = new Dictionary<Direction, string> {
        {Direction.UP, "WalkFront"},
        {Direction.RIGHT, "WalkRight"},
        {Direction.DOWN, "WalkBack"},
        {Direction.LEFT, "WalkLeft"},
        {Direction.STOP, "Stop"},
    };

    // TODO sprite lighting and shadows
    // TODO smart camera movement

   Direction _getDirection(float Vx, float Vy) {
        if (_movingX) {
            if (Vx > 0) {
                return Direction.RIGHT;
            } else {
                return Direction.LEFT;
            }
        } else if (_movingY) {
            if (Vy > 0) {
                return Direction.DOWN;
            } else {
                return Direction.UP;
            }
        } else {
            return Direction.STOP;
        }
    }

    void _updateDirection(float Vx, float Vy) {
        bool movingXnew = (Math.Abs(Vx) > 0.5);
        bool movingYnew = (Math.Abs(Vy) > 0.5);

        // start moving
        if (!_movingX && !_movingY) {
            if (movingXnew && !movingYnew) {
                _movingX = true;
            } else if (!movingXnew && movingYnew) {
                _movingY = true;
            }
        // stop moving
        } else if (!movingXnew && !movingYnew) {
            _movingX = false;
            _movingY = false;
        //change direction
        } else if ((_movingX != movingXnew) && (_movingY != movingYnew)) {
            _movingX = !_movingX;
            _movingY = !_movingY;
        }
    }

    void Start() {
        _movingX = false;
        _movingY = false;
    }

    void Update() {
        float Vx = Input.GetAxis("Horizontal");
        float Vy = Input.GetAxis("Vertical");
        this._updateDirection(Vx, Vy);
        Direction dir = this._getDirection(Vx, Vy);
        animator.SetTrigger(this._directionAnimation[dir]);

        if (_movingX) {
            body.velocity = new Vector3(speed*Vx, body.velocity.y, 0);
        } else if (_movingY) {
            body.velocity = new Vector3(0, body.velocity.y, speed*Vy);
        }
    }
}
