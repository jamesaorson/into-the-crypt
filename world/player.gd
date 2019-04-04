extends CSGBox

var velocity = 10
var forward = { vector = Vector3(0, 0, 1), inputName = "player_move_forward" }
var backward = { vector = -forward.vector, inputName = "player_move_backward" }
var left = { vector = Vector3(1, 0, 0), inputName = "player_move_left" }
var right = { vector = -left.vector, inputName = "player_move_right" }

var deadZone = 0.2

func _ready():
	set_process(true)

func _process(delta):
	if Input.is_action_pressed(self.forward.inputName):
		move(self.forward, self.velocity, delta)
	if Input.is_action_pressed(self.backward.inputName):
		move(self.backward, self.velocity, delta)
	if Input.is_action_pressed(self.left.inputName):
		move(self.left, self.velocity, delta)
	if Input.is_action_pressed(self.right.inputName):
		move(self.right, self.velocity, delta)

func move(direction, velocity, delta, deadZone = self.deadZone):
	var strength = Input.get_action_strength(direction.inputName)
	if abs(strength) > deadZone:
		translate(direction.vector * strength * velocity * delta)