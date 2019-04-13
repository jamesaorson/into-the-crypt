extends KinematicBody2D

const UP = { vector = Vector2(0, -1), inputName = "player_move_up" }
const DOWN = { vector = -UP.vector, inputName = "player_move_down" }
const LEFT = { vector = Vector2(-1, 0), inputName = "player_move_left" }
const RIGHT = { vector = -LEFT.vector, inputName = "player_move_right" }

var velocity = Vector2()
const WALKING_SPEED = 1000
const SPRINTING_SPEED = 3000
var friction = 0.9

var isSprinting = false

func _ready():
	set_physics_process(true)

func _physics_process(delta):
	get_input()
	move_and_slide(velocity * delta)

func destroy():
	queue_free()

func get_input():
	if Input.is_action_just_pressed("sprint"):
		isSprinting = true
	if Input.is_action_just_released("sprint"):
		isSprinting = false
	var speed = SPRINTING_SPEED if isSprinting else WALKING_SPEED
	
	if Input.is_action_pressed(self.UP.inputName):
		velocity += self.UP.vector * Input.get_action_strength(self.UP.inputName) * speed
	if Input.is_action_pressed(self.DOWN.inputName):
		velocity += self.DOWN.vector * Input.get_action_strength(self.DOWN.inputName) * speed
	if Input.is_action_pressed(self.LEFT.inputName):
		velocity += self.LEFT.vector * Input.get_action_strength(self.LEFT.inputName) * speed
	if Input.is_action_pressed(self.RIGHT.inputName):
		velocity += self.RIGHT.vector * Input.get_action_strength(self.RIGHT.inputName) * speed

	velocity *= friction

func set_crypt_seed_text():
	var cryptSeedString = "Crypt Seed: " + str(crypt_globals.cryptSeed)
	var labelNode = get_node("/root/PlayerKinematicBody2D/CryptSeedLayer/CryptSeedLabel")
	labelNode.text = cryptSeedString
	labelNode.update()