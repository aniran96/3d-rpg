using Godot;
using System;

public partial class Player : CharacterBody3D
{
	//node references
	private SpringArm3D _cameraSpringArm3DNode;
	// variables
	[ Export ]
	private float speed = 5.0f;
	[ Export ]
	private float jumpVelocity = 4.5f;
	[ Export ]
	private float _mouseSensitivity = 0.00075f; 

	private Vector2 _look = Vector2.Zero;	// stores the x and the y direction the player is trying to look at.

	public override void _Ready() 
	{
		_cameraSpringArm3DNode = GetNode<SpringArm3D>( "CameraSpringArm3D" );
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		FrameCameraRotation();
		Vector3 velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		if (Input.IsActionJustPressed( GameConstants.JUMP ) && IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}
		Vector2 inputDir = Input.GetVector( GameConstants.INPUT_LEFT, GameConstants.INPUT_RIGHT, GameConstants.INPUT_FORWARD, GameConstants.INPUT_BACK );
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
		}
		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if ( @event.IsActionPressed( GameConstants.MOUSE_FREE ) ) 
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		if ( Input.MouseMode == Input.MouseModeEnum.Captured ) 
		{
			if ( @event is InputEventMouseMotion mouseMotionEvent ) 
			{
				_look = -mouseMotionEvent.Relative * _mouseSensitivity;
				GD.Print( _look );
			}
		}
    }

	 private void FrameCameraRotation()
    {
		_cameraSpringArm3DNode.RotateY( _look.X );
		_look = Vector2.Zero;		
    }
}
