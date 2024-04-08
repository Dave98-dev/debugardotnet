extends CharacterBody3D


const SPEED = 500
const JUMP_SPEED = 300
const mouse_sens = 0.04

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func _init():
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

var camera_anglev=0
func _input(event):
	if event is InputEventMouseMotion and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		rotate_y(deg_to_rad(-event.relative.x*mouse_sens))
		var changev=-event.relative.y*mouse_sens
		if camera_anglev+changev>-50 and camera_anglev+changev<50:
			camera_anglev+=changev
			$Camera3D.rotate_x(deg_to_rad(changev))


func _process(delta):	
	if Input.is_action_just_pressed("ui_cancel"):
		if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
			Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
		else:
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _physics_process(delta):
	# Add the gravity.
	#if not is_on_floor():
		#velocity.y -= gravity * delta

	# Handle jump.
	#if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		#velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	velocity.y = 0
	
	var input_dir = Input.get_vector("A", "D", "W", "S")
	var direction = (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	if direction:
		velocity.x = direction.x * SPEED
		velocity.z = direction.z * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.z = move_toward(velocity.z, 0, SPEED)
		
	
	if Input.is_action_pressed("space"):
		velocity.y = JUMP_SPEED
		
	if Input.is_action_pressed("shift"):
		velocity.y = -JUMP_SPEED

	move_and_slide()
