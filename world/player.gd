extends CSGBox

var velocity = 10
var forward = { vector = Vector3(0, 0, 1), name = "forward" }
var backward = { vector = -forward.vector, name = "backward" }
var left = { vector = Vector3(1, 0, 0), name = "left" }
var right = { vector = -left.vector, name = "right" }

func _ready():
	set_process(true)

func _process(delta):
	if Input.is_action_pressed("player_move_forward"):
		move(forward, delta)
	if Input.is_action_pressed("player_move_backward"):
		move(backward, delta)
	if Input.is_action_pressed("player_move_left"):
		move(left, delta)
	if Input.is_action_pressed("player_move_right"):
		move(right, delta)

func move(direction, delta):
	print(direction.name)
	translate(direction.vector * velocity * delta)