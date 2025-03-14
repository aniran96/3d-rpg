using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// variables
	[ Export ]
	private float speed = 5.0f;
	[ Export ]
	private float jumpVelocity = 4.5f;

	public override void _Ready() 
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
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

    public override void _UnhandledInput(InputEvent evt)
    {
        if ( evt.IsActionPressed( GameConstants.MOUSE_FREE ) ) 
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
    }
}
