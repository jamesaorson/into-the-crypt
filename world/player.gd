extends KinematicBody

const FORWARD = { vector = Vector3(0, 0, 1), inputName = "player_move_forward" }
const BACKWARD = { vector = -FORWARD.vector, inputName = "player_move_backward" }
const LEFT = { vector = Vector3(1, 0, 0), inputName = "player_move_left" }
const RIGHT = { vector = -LEFT.vector, inputName = "player_move_right" }
const UP = { vector = Vector3(0, 1, 0), inputName = "player_move_up" }
const DOWN = { vector = -UP.vector, inputName = "player_move_down" }

var camera
var gravity = -9.8
var velocity = Vector3()

const SPEED = 6
const ACCELERATION = 3
const DE_ACCELERATION = 5

func _ready():
	set_process(true)
	camera = get_node("../Camera").get_global_transform()

func _physics_process(delta):
	var direction = Vector3()
	if (Input.is_action_pressed(self.FORWARD.inputName)):
		direction += camera.basis[1]
	if (Input.is_action_pressed(self.BACKWARD.inputName)):
		direction += -camera.basis[1]
	if (Input.is_action_pressed(self.LEFT.inputName)):
		direction += -camera.basis[0]
	if (Input.is_action_pressed(self.RIGHT.inputName)):
		direction += camera.basis[0]
	direction.y = 0
	direction = direction.normalized()

	self.velocity.y += delta * self.gravity
	var horizontalVelocity = self.velocity
	horizontalVelocity.y = 0
	
	var newPosition = direction * SPEED
	var acceleration = DE_ACCELERATION

	if (direction.dot(horizontalVelocity) > 0):
		acceleration = ACCELERATION
	horizontalVelocity = horizontalVelocity.linear_interpolate(newPosition, acceleration * delta)
	self.velocity.x = horizontalVelocity.x
	self.velocity.z = horizontalVelocity.z
	self.velocity = move_and_slide(self.velocity, self.UP.vector)