extends KinematicBody2D

class_name PlayerNode

var playerModel : Player = player_globals.player
var canEnterCrypt : bool = false
var canExitCrypt : bool = false
var canTalkWithVillager : bool = false
var villagerToTalkTo : Area2D = null
var isTalking : bool = false

###################
# Godot Functions #
###################

func _input(event : InputEvent) -> void:
	handle_unpolled_input(event)

func _physics_process(delta : float) -> void:
	handle_polled_input()
	self.playerModel.velocity = move_and_slide(self.playerModel.velocity)

func _process(delta : float) -> void:
	self.playerModel.timeElapsed = OS.get_unix_time() - self.playerModel.timeStart
	$ActionPrompt.visible = self.canEnterCrypt or self.canExitCrypt or self.canTalkWithVillager
	if self.playerModel != null:
		self.playerModel.update()
	$UI/DebugInfo.update()

func _ready() -> void:
	set_physics_process(true)
	set_process_input(true)
	create_weapon(weapon_globals.AXE_NAME)

####################
# Helper Functions #
####################

func create_weapon(weaponName : String) -> void:
	self.playerModel.weaponNode = null
	var weaponScene : Resource = null
	match weaponName:
		weapon_globals.AXE_NAME:
			weaponScene = load(utility.construct_scene_path('axe'))
		weapon_globals.SWORD_NAME:
			weaponScene = load(utility.construct_scene_path('sword'))
	if weaponScene == null:
		print('Something went wrong when making the weapon: ', weaponName)
		return

	self.playerModel.weaponNode = weaponScene.instance()
	$Weapon.add_child(self.playerModel.weaponNode)

func damage(damageToTake : float) -> void:
	if self.playerModel != null:
		self.playerModel.damage(damageToTake)

func die() -> void:
	exit_crypt()

func exit_crypt() -> void:
	var cryptNodes : Array = get_tree().get_nodes_in_group('crypt')
	if cryptNodes != null and len(cryptNodes) > 0:
		cryptNodes[0].exit_crypt()

func flip_weapon(shouldBeFlipped : bool) -> void:
	$AnimatedSprite.set_flip_h(shouldBeFlipped)
	self.playerModel.weaponNode.flip_h()
	$Weapon.position.x = -$Weapon.position.x

func handle_polled_input() -> void:
	var movementMade : bool = false
	if Input.is_action_just_pressed(input_globals.SPRINT):
		self.playerModel.isSprinting = true
	elif Input.is_action_just_released(input_globals.SPRINT):
		self.playerModel.isSprinting = false
	var speed : float = self.playerModel.sprintingSpeed if self.playerModel.isSprinting else self.playerModel.walkingSpeed

	if Input.is_action_just_pressed(input_globals.PRIMARY_ATTACK) and self.playerModel.weaponNode != null:
		self.playerModel.weaponNode.attack()

	if Input.is_action_pressed(input_globals.UP):
		movementMade = true
		self.playerModel.velocity += Vector2.UP * Input.get_action_strength(input_globals.UP) * speed
	if Input.is_action_pressed(input_globals.DOWN):
		movementMade = true
		self.playerModel.velocity += Vector2.DOWN * Input.get_action_strength(input_globals.DOWN) * speed
	if Input.is_action_pressed(input_globals.LEFT):
		movementMade = true
		self.playerModel.velocity += Vector2.LEFT * Input.get_action_strength(input_globals.LEFT) * speed
		if $AnimatedSprite.flip_h:
			flip_weapon(false)
	if Input.is_action_pressed(input_globals.RIGHT):
		movementMade = true
		self.playerModel.velocity += Vector2.RIGHT * Input.get_action_strength(input_globals.RIGHT) * speed
		if not $AnimatedSprite.flip_h:
			flip_weapon(true)

	self.playerModel.velocity = self.playerModel.velocity.clamped(self.playerModel.maxVelocity * speed)
	if not movementMade:
		self.playerModel.velocity *= player_globals.friction

func handle_unpolled_input(event : InputEvent) -> void:
	if Input.is_action_just_pressed(input_globals.UI_ACCEPT):
		if self.canEnterCrypt:
			self.canEnterCrypt = false
			var villageNodes : Array = get_tree().get_nodes_in_group('village')
			if villageNodes != null and len(villageNodes) > 0:
				villageNodes[0].enter_crypt()
		elif self.canExitCrypt:
			exit_crypt()
		elif self.canTalkWithVillager and not self.isTalking:
			self.isTalking = true
			$UI/DialogBox.talk(self, self.villagerToTalkTo)
	if OS.is_debug_build() and Input.is_action_just_pressed(input_globals.TOGGLE_DEBUG):
		var debugNodes : Array = get_tree().get_nodes_in_group('debug_info')
		for debugNode in debugNodes:
			if debugNode.visible != null:
				debugNode.visible = not debugNode.visible

func initialize_player() -> void:
	if self.playerModel.timeStart < 0:
		self.playerModel.timeStart = OS.get_unix_time()

func set_camera_zoom(zoomLevel : Vector2) -> void:
	if zoomLevel != null:
		$Camera2D.zoom = zoomLevel

###################
# Signal Handlers #
###################

func _on_EnterCrypt_area_entered(area : Area2D):
	self.canEnterCrypt = true

func _on_EnterCrypt_area_exited(area : Area2D):
	self.canEnterCrypt = false

func _on_ExitCrypt_area_entered(area : Area2D):
	self.canExitCrypt = true

func _on_ExitCrypt_area_exited(area : Area2D):
	self.canExitCrypt = false

func _on_TalkWithPlayer_area_entered(area : Area2D):
	self.canTalkWithVillager = true
	self.villagerToTalkTo = area

func _on_TalkWithPlayer_area_exited(area : Area2D):
	self.canTalkWithVillager = false
	self.villagerToTalkTo = null
	self.isTalking = false
	$UI/DialogBox.visible = false

func _exit_tree():
	self.playerModel.weaponNode = null