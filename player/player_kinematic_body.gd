extends KinematicBody2D

onready var playerLightScene = load("res://player/player_light/player_light.tscn")
onready var debugInfoScene = load("res://player/debug_info/debug_info.tscn")

onready var weaponSocketNode = $WeaponNode2D
var weapon = null

var playerIndex = 0
var player = player_globals.players[self.playerIndex]

var PLAYER_SPRINT = "sprint_" + str(self.playerIndex)

var PLAYER_PRIMARY_ATTACK = "primary_attack_" + str(self.playerIndex)

const UP = player_globals.UP
const DOWN = player_globals.DOWN
const LEFT = player_globals.LEFT
const RIGHT = player_globals.RIGHT

###################
# Godot Functions #
###################

func _physics_process(delta):
	get_input()
	move_and_slide(player.velocity * delta)

func _process(delta):
	player.timeElapsed = OS.get_unix_time() - player.timeStart
	if player.lightNode != null:
		player.lightNode.update_size(delta)

func _ready():
	set_physics_process(true)
	create_light(self.playerIndex)
	create_debug_info()
	create_weapon("sword")

####################
# Helper Functions #
####################

func create_debug_info():
	var debugInfo = player_globals.players[0].debugInfo
	if debugInfo == null:
		debugInfo = debugInfoScene.instance()
		add_child(debugInfo)
		player_globals.players[0].debugInfo = debugInfo

func create_light(playerIndex):
	var playerLight = player_globals.players[playerIndex].lightNode
	if playerLight == null:
		playerLight = playerLightScene.instance()
		add_child(playerLight)
		player_globals.players[playerIndex].lightNode = playerLight

func create_weapon(weaponName):
	print("sword")
	if player_globals.players[playerIndex].weapon != null:
		player_globals.players[playerIndex].weapon.queue_free()
		player_globals.players[playerIndex].weapon = null
	var weaponScene = null
	match weaponName:
	    "sword":
	        weaponScene = preload("res://weapon/sword/sword.tscn")
	if weaponScene == null:
		print("Something went wrong when making the weapon: ", weaponName)
		return
	self.weapon = weaponScene.instance()
	player_globals.players[playerIndex].weapon = self.weapon
	self.weaponSocketNode.add_child(self.weapon)

func destroy():
	if player_globals.players[playerIndex].weapon != null:
		player_globals.players[playerIndex].weapon.queue_free()
		player_globals.players[playerIndex].weapon = null
	queue_free()

func get_input():
	if Input.is_action_just_pressed(PLAYER_SPRINT):
		player.isSprinting = true
	if Input.is_action_just_released(PLAYER_SPRINT):
		player.isSprinting = false
	var speed = player.sprintingSpeed if player.isSprinting else player.walkingSpeed
	
	if Input.is_action_just_pressed(PLAYER_PRIMARY_ATTACK) and player_globals.players[playerIndex].weapon != null:
		player_globals.players[playerIndex].weapon.attack()
	
	var movementMade = false
	if Input.is_action_pressed(UP.inputName):
		movementMade = true
		player.velocity += UP.vector * Input.get_action_strength(UP.inputName) * speed
	if Input.is_action_pressed(DOWN.inputName):
		movementMade = true
		player.velocity += DOWN.vector * Input.get_action_strength(DOWN.inputName) * speed
	if Input.is_action_pressed(LEFT.inputName):
		movementMade = true
		player.velocity += LEFT.vector * Input.get_action_strength(LEFT.inputName) * speed
		if $AnimatedSprite.flip_h:
			$AnimatedSprite.set_flip_h(false)
			self.weapon.flip_h()
			$WeaponNode2D.position.x = -$WeaponNode2D.position.x
	if Input.is_action_pressed(RIGHT.inputName):
		movementMade = true
		player.velocity += RIGHT.vector * Input.get_action_strength(RIGHT.inputName) * speed
		if not $AnimatedSprite.flip_h:
			$AnimatedSprite.set_flip_h(true)
			self.weapon.flip_h()
			$WeaponNode2D.position.x = -$WeaponNode2D.position.x

	player.velocity = player.velocity.clamped(player.maxVelocity * speed)
	if not movementMade:
		player.velocity *= player_globals.friction

func set_player_index(newPlayerIndex):
	var tempTimeStart = player.timeStart
	var tempLightNode = player.lightNode
	var tempDebugInfo = player.debugInfo
	var tempInstance = player.instance
	if tempTimeStart == null:
		tempTimeStart = OS.get_unix_time()

	self.playerIndex = newPlayerIndex
	player = player_globals.players[newPlayerIndex]
	PLAYER_SPRINT = "sprint_" + str(newPlayerIndex)
	player.timeStart = tempTimeStart
	player.lightNode = tempLightNode
	player.debugInfo = tempDebugInfo
	player.instance = tempInstance