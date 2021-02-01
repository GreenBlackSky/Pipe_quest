using System;
using System.Collections.Generic;
using UnityEngine;


public enum Direction {UP, RIGHT, DOWN, LEFT, STOP}

public class MovingHero : MonoBehaviour {
    public Animator animator;
    public Rigidbody body;

    public float speed;

    private Direction _direction;
    private Dictionary<Direction, string> _directionAnimation = new Dictionary<Direction, string> {
        {Direction.UP, "WalkBack"},
        {Direction.RIGHT, "WalkRight"},
        {Direction.DOWN, "WalkFront"},
        {Direction.LEFT, "WalkLeft"},
        {Direction.STOP, "Stop"},
    };

    // TODO sprite lighting and shadows
    // TODO smart camera movement

    void _updateDirection() {
        bool up = Input.GetKey(KeyCode.W);
        bool left = Input.GetKey(KeyCode.A);
        bool down = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        
        bool movingXnew = (left || right);
        bool movingYnew = (up || down);

        bool movingXcur = (_direction == Direction.LEFT || _direction == Direction.RIGHT);
        bool movingYcur = (_direction == Direction.UP || _direction == Direction.DOWN);
        // start moving
        if(_direction == Direction.STOP) {
            if(movingXnew && !movingYnew) {
                if(left) {
                    _direction = Direction.LEFT;
                }
                if(right) {
                    _direction = Direction.RIGHT;
                }
            } else if(!movingXnew && movingYnew) {
                if(up) {
                    _direction = Direction.UP;
                }
                if(down) {
                    _direction = Direction.DOWN;
                }
            }
        // stop moving
        } else if (!movingXnew && !movingYnew) {
            _direction = Direction.STOP;
        //change direction
        } else {
            if(left) {
                _direction = Direction.LEFT;
            }
            if(right) {
                _direction = Direction.RIGHT;
            }
            if(up) {
                _direction = Direction.UP;
            }
            if(down) {
                _direction = Direction.DOWN;
            }
        }
    }

    void _updateSpeed() {
        switch(_direction) {
            case Direction.LEFT:
                body.velocity = new Vector3(-speed, body.velocity.y, 0);
                break;
            case Direction.RIGHT:
                body.velocity = new Vector3(speed, body.velocity.y, 0);
                break;
            case Direction.UP:
                body.velocity = new Vector3(0, body.velocity.y, speed);
                break;
            case Direction.DOWN:
                body.velocity = new Vector3(0, body.velocity.y, -speed);
                break;
        }
    }

    void Start() {
        _direction = Direction.STOP;
    }

    void Update() {
        this._updateDirection();
        animator.SetTrigger(this._directionAnimation[_direction]);
        this._updateSpeed();
    }
}
