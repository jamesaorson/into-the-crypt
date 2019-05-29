extends KinematicBody2D

onready var playerLightScene = load("res://player/player_light/player_light.tscn")
onready var debugInfoScene = load("res://player/debug_info/debug_info.tscn")

var weapon = null

var playerIndex = 0
var player = player_globals.players[self.playerIndex]
var canEnterCrypt = false
var canExitCrypt = false
var canTalkWithVillager = false
var villagerToTalkTo = null
var isTalking = false

###################
# Godot Functions #
###################

func _input(event):
	handle_unpolled_input(event)

func _physics_process(delta):
	handle_polled_input()
	player.velocity = move_and_slide(player.velocity)

func _process(delta):
	player.timeElapsed = OS.get_unix_time() - player.timeStart
	$ActionPrompt.visible = self.canEnterCrypt or self.canExitCrypt or self.canTalkWithVillager

func _ready():
	set_physics_process(true)
	set_process_input(true)
	create_light(self.playerIndex)
	create_weapon("sword")
	if OS.is_debug_build():
		create_debug_info()

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
	$Weapon.add_child(self.weapon)

func destroy():
	if player_globals.players[playerIndex].weapon != null:
		player_globals.players[playerIndex].weapon.queue_free()
		player_globals.players[playerIndex].weapon = null
	queue_free()

func flip_weapon(shouldBeFlipped):
	$AnimatedSprite.set_flip_h(shouldBeFlipped)
	self.weapon.flip_h()
	$Weapon.position.x = -$Weapon.position.x

func handle_polled_input():
	var movementMade = false
	if Input.is_action_just_pressed(input_globals.SPRINT):
		player.isSprinting = true
	elif Input.is_action_just_released(input_globals.SPRINT):
		player.isSprinting = false
	var speed = player.sprintingSpeed if player.isSprinting else player.walkingSpeed

	if Input.is_action_just_pressed(input_globals.PRIMARY_ATTACK) and player_globals.players[playerIndex].weapon != null:
		player_globals.players[playerIndex].weapon.attack()

	if Input.is_action_pressed(input_globals.UP):
		movementMade = true
		player.velocity += Vector2.UP * Input.get_action_strength(input_globals.UP) * speed
	if Input.is_action_pressed(input_globals.DOWN):
		movementMade = true
		player.velocity += Vector2.DOWN * Input.get_action_strength(input_globals.DOWN) * speed
	if Input.is_action_pressed(input_globals.LEFT):
		movementMade = true
		player.velocity += Vector2.LEFT * Input.get_action_strength(input_globals.LEFT) * speed
		if $AnimatedSprite.flip_h:
			flip_weapon(false)
	if Input.is_action_pressed(input_globals.RIGHT):
		movementMade = true
		player.velocity += Vector2.RIGHT * Input.get_action_strength(input_globals.RIGHT) * speed
		if not $AnimatedSprite.flip_h:
			flip_weapon(true)

	player.velocity = player.velocity.clamped(player.maxVelocity * speed)
	if not movementMade:
		player.velocity *= player_globals.friction

func handle_unpolled_input(event):
	if Input.is_action_just_pressed(input_globals.UI_ACCEPT):
		if self.canEnterCrypt:
			self.canEnterCrypt = false
			var villageNodes = get_tree().get_nodes_in_group("village")
			if villageNodes != null and villageNodes[0] != null:
				villageNodes[0].enter_crypt()
		elif self.canExitCrypt:
			var cryptNodes = get_tree().get_nodes_in_group("crypt")
			if cryptNodes != null and cryptNodes[0] != null:
				cryptNodes[0].exit_crypt()
		elif self.canTalkWithVillager and not self.isTalking:
			self.isTalking = true
			$DialogBox.talk(self, self.villagerToTalkTo)

func set_camera_zoom(zoomLevel):
	if zoomLevel != null:
		$Camera2D.zoom = zoomLevel

func set_player_index(newPlayerIndex):
	var tempTimeStart = player.timeStart
	var tempLightNode = player.lightNode
	var tempDebugInfo = player.debugInfo
	var tempInstance = player.instance
	if tempTimeStart == null:
		tempTimeStart = OS.get_unix_time()

	self.playerIndex = newPlayerIndex
	player = player_globals.players[newPlayerIndex]
	player.timeStart = tempTimeStart
	player.lightNode = tempLightNode
	player.debugInfo = tempDebugInfo
	player.instance = tempInstance

###################
# Signal Handlers #
###################

func _on_EnterCrypt_area_entered(area):
	self.canEnterCrypt = true

func _on_EnterCrypt_area_exited(area):
	self.canEnterCrypt = false

func _on_ExitCrypt_area_entered(area):
	self.canExitCrypt = true

func _on_ExitCrypt_area_exited(area):
	self.canExitCrypt = false

func _on_TalkWithPlayer_area_entered(area):
	self.canTalkWithVillager = true
	self.villagerToTalkTo = area

func _on_TalkWithPlayer_area_exited(area):
	self.canTalkWithVillager = false
	self.villagerToTalkTo = null
	self.isTalking = false
	$DialogBox.visible = false