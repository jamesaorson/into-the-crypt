extends KinematicBody2D

class_name PlayerNode

onready var player : Player = Player.new(
	null,
	Vector2(3, 2),
	2,
	3,
	Vector2(),
	7,
	15,
	50,
	false,
	20,
	20
)
export(float) var friction : float = 0.80
onready var canEnterCrypt : bool = false
onready var canExitCrypt : bool = false
onready var canTalkWithVillager : bool = false
onready var villagerToTalkTo : Area2D = null
onready var isTalking : bool = false

onready var inventoryScene : Resource = load(utility.construct_scene_path('inventory_menu'))

# Weapon scenes
onready var axeScene = load(utility.construct_scene_path('axe'))
onready var swordScene = load(utility.construct_scene_path('sword'))

var inventoryMenu : InventoryMenu = null
var shouldUpdateInventory : bool = false

###################
# Godot Functions #
###################

func _input(event : InputEvent) -> void:
	handle_unpolled_input(event)

func _physics_process(_delta : float) -> void:
	handle_polled_input()
	self.player.velocity = move_and_slide(self.player.velocity)

func _process(_delta : float) -> void:
	self.player.timeElapsed = OS.get_unix_time() - self.player.timeStart
	$ActionPrompt.visible = self.canEnterCrypt or self.canExitCrypt or self.canTalkWithVillager
	if self.player != null:
		self.player.update()
	$UI/DebugInfo.update(self.player)
	if self.shouldUpdateInventory:
		self.shouldUpdateInventory = false
		self.update_inventory()

func _ready() -> void:
	set_physics_process(true)
	set_process_input(true)
	equip_weapon(weapon_globals.AXE_NAME)

####################
# Helper Functions #
####################

func close_inventory() -> void:
	if self.inventoryMenu != null:
		self.inventoryMenu.queue_free()
		self.inventoryMenu = null

func damage(damageToTake : float) -> void:
	if self.player != null:
		self.player.damage(damageToTake)

func die() -> void:
	exit_crypt()

func equip_weapon(weaponName : String) -> void:
	var weapon : WeaponEquipmentItem = self.player.get_weapon(weaponName)
	if weapon != null:
		var weaponScene : Resource = null
		match weaponName:
			weapon_globals.AXE_NAME:
				weaponScene = self.axeScene
			weapon_globals.SWORD_NAME:
				weaponScene = self.swordScene
		if weaponScene == null:
			print('Something went wrong when making the weapon: ', weaponName)
			return

		var weaponNode : Area2D = weaponScene.instance()
		weaponNode.setup(weapon)
		if self.player.equip_weapon(weaponNode):
			$Weapon.add_child(self.player.weaponNode)
			self.shouldUpdateInventory = true
		else:
			print('Could not instance weapon ' + weaponName)
			weaponNode.queue_free()

func exit_crypt() -> void:
	var cryptNodes : Array = get_tree().get_nodes_in_group('crypt')
	if cryptNodes != null and len(cryptNodes) > 0:
		cryptNodes[0].exit_crypt()

func flip_weapon(shouldBeFlipped : bool) -> void:
	$AnimatedSprite.set_flip_h(shouldBeFlipped)
	self.player.weaponNode.flip_h()
	$Weapon.position.x = -$Weapon.position.x

func handle_polled_input() -> void:
	var movementMade : bool = false
	if Input.is_action_just_pressed(input_globals.SPRINT):
		self.player.isSprinting = true
	elif Input.is_action_just_released(input_globals.SPRINT):
		self.player.isSprinting = false
	var speed : float = self.player.sprintingSpeed if self.player.isSprinting else self.player.walkingSpeed

	if Input.is_action_just_pressed(input_globals.PRIMARY_ATTACK) and self.player.weaponNode != null:
		self.player.weaponNode.attack()

	if Input.is_action_pressed(input_globals.UP):
		movementMade = true
		self.player.velocity += Vector2.UP * Input.get_action_strength(input_globals.UP) * speed
	if Input.is_action_pressed(input_globals.DOWN):
		movementMade = true
		self.player.velocity += Vector2.DOWN * Input.get_action_strength(input_globals.DOWN) * speed
	if Input.is_action_pressed(input_globals.LEFT):
		movementMade = true
		self.player.velocity += Vector2.LEFT * Input.get_action_strength(input_globals.LEFT) * speed
		if $AnimatedSprite.flip_h:
			flip_weapon(false)
	if Input.is_action_pressed(input_globals.RIGHT):
		movementMade = true
		self.player.velocity += Vector2.RIGHT * Input.get_action_strength(input_globals.RIGHT) * speed
		if not $AnimatedSprite.flip_h:
			flip_weapon(true)

	self.player.velocity = self.player.velocity.clamped(self.player.maxVelocity * speed)
	if not movementMade:
		self.player.velocity *= self.friction

func handle_unpolled_input(_event : InputEvent) -> void:
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
	if Input.is_action_just_released(input_globals.INVENTORY_TOGGLE):
		if self.inventoryMenu == null:
			self.inventoryMenu = inventoryScene.instance()
			add_child(self.inventoryMenu)
			self.update_inventory()
		else:
			self.close_inventory()
	if Input.is_action_just_pressed(input_globals.PAUSE):
		if self.inventoryMenu != null:
			self.close_inventory()
		else:
			self.quit_to_main_menu()
	if OS.is_debug_build():
		if Input.is_action_just_pressed(input_globals.TOGGLE_DEBUG):
			var debugNodes : Array = get_tree().get_nodes_in_group('debug_info')
			for debugNode in debugNodes:
				if debugNode.visible != null:
					debugNode.visible = not debugNode.visible

func initialize_player() -> void:
	if self.player.timeStart < 0:
		self.player.timeStart = OS.get_unix_time()

func quit_to_main_menu() -> void:
	utility.error_handled_scene_change('main_menu')

func set_camera_zoom(zoomLevel : Vector2) -> void:
	if zoomLevel != null:
		$Camera2D.zoom = zoomLevel

func update_inventory() -> void:
	if self.inventoryMenu != null:
		self.inventoryMenu.update_inventory(self.player)

###################
# Signal Handlers #
###################

func _on_EnterCrypt_area_entered(_area : Area2D):
	self.canEnterCrypt = true

func _on_EnterCrypt_area_exited(_area : Area2D):
	self.canEnterCrypt = false

func _on_ExitCrypt_area_entered(_area : Area2D):
	self.canExitCrypt = true

func _on_ExitCrypt_area_exited(_area : Area2D):
	self.canExitCrypt = false

func _on_TalkWithPlayer_area_entered(area : Area2D):
	self.canTalkWithVillager = true
	self.villagerToTalkTo = area

func _on_TalkWithPlayer_area_exited(_area : Area2D):
	self.canTalkWithVillager = false
	self.villagerToTalkTo = null
	self.isTalking = false
	$UI/DialogBox.visible = false

func _exit_tree():
	self.player.weaponNode = null