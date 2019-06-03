extends TileMap

class_name CryptGeneratorNode

var Enemy : Resource  = load("res://models/Enemy.gd")

onready var playerScene : Resource = load("res://player/player.tscn")
onready var enemyScene : Resource  = load("res://enemy/enemy.tscn")

const FLOOR_TILES : Array = crypt_generator_globals.FLOOR_TILES
const WALL_TILES : Array = crypt_generator_globals.WALL_TILES

const CRYPT_SECTION_SIZE : int = crypt_generator_globals.CRYPT_SECTION_SIZE

var CRYPT_HEIGHT : int
const CRYPT_MAX_HEIGHT : int = crypt_generator_globals.CRYPT_MAX_HEIGHT
const CRYPT_MIN_HEIGHT : int = crypt_generator_globals.CRYPT_MIN_HEIGHT

var CRYPT_WIDTH : int
const CRYPT_MAX_WIDTH : int = crypt_generator_globals.CRYPT_MAX_WIDTH
const CRYPT_MIN_WIDTH : int = crypt_generator_globals.CRYPT_MIN_WIDTH

var HORIZONTAL_HALLWAYS : Array = crypt_generator_globals.HORIZONTAL_HALLWAYS

var VERTICAL_HALLWAYS : Array = crypt_generator_globals.VERTICAL_HALLWAYS

####################
# Helper Functions #
####################

func create_enemy(mapPosition : Vector2) -> void:
	var enemy : EnemyNode = null
	enemy = enemyScene.instance()
	get_tree().root.add_child(enemy)
	var enemyPosition : Vector2 = map_to_world(Vector2(mapPosition.x, mapPosition.y))
	enemy.position.x = enemyPosition.x
	enemy.position.y = enemyPosition.y
	enemy.scale = Vector2(0.75, 0.75)
	var enemyModel : Enemy = Enemy.new(enemy, mapPosition, 2, 3)
	enemyModel.maxHealth = 2.0
	enemyModel.health = 2.0
	crypt_globals.enemies[enemyModel.get_instance_id()] = enemyModel
	enemy.enemyModel = enemyModel

func create_player() -> void:
	var player : PlayerNode = null
	if player_globals.player.instance != null:
		player = player_globals.player.instance
	else:
		player = playerScene.instance()
		player_globals.player.instance = player
		add_child(player)
	player_globals.player.health = 20.0
	player_globals.player.maxHealth = 20.0
	player.initialize_player()
	var playerPosition : Vector2 = Vector2(player_globals.player.position.x, player_globals.player.position.y)
	playerPosition.x += 2 * CRYPT_SECTION_SIZE
	playerPosition.y += 2 * CRYPT_SECTION_SIZE
	playerPosition = map_to_world(playerPosition)
	player.position.x = playerPosition.x
	player.position.y = playerPosition.y
	player.scale = Vector2(0.75, 0.75)

	var exitNodes : Array = get_tree().get_nodes_in_group("crypt_exit")
	if exitNodes != null:
		for node in exitNodes:
			node.position = player.position

func destroy() -> void:
	var playerNodes : Array = get_tree().get_nodes_in_group("player")
	for node in playerNodes:
		node.destroy()
	player_globals.player.instance = null

	var enemyNodes : Array = get_tree().get_nodes_in_group("enemy")
	for node in enemyNodes:
		node.destroy()
	crypt_globals.enemies.clear()

	var cryptCanvasModulateNodes : Array = get_tree().get_nodes_in_group("crypt_canvas_modulate")
	for node in cryptCanvasModulateNodes:
		node.free()

	queue_free()

func draw_crypt() -> void:
	clear()
	for y in range(len(crypt_globals.crypt)):
		for x in range(len(crypt_globals.crypt[y])):
			set_cell(x, y, crypt_globals.crypt[y][x])

func generate_crypt() -> void:
	if crypt_globals.cryptSeed < 0:
		randomize()
		crypt_globals.cryptSeed = randi()

	print("Generating crypt with seed ", crypt_globals.cryptSeed)
	seed(crypt_globals.cryptSeed)
	crypt_generator_globals.CRYPT_HEIGHT = CRYPT_SECTION_SIZE * floor(rand_range(CRYPT_MIN_HEIGHT, CRYPT_MAX_HEIGHT))
	CRYPT_HEIGHT = crypt_generator_globals.CRYPT_HEIGHT
	crypt_generator_globals.CRYPT_WIDTH = CRYPT_SECTION_SIZE * floor(rand_range(CRYPT_MIN_WIDTH, CRYPT_MAX_WIDTH))
	CRYPT_WIDTH = crypt_generator_globals.CRYPT_WIDTH

	initalize_crypt_object()
	for y in range(CRYPT_SECTION_SIZE * 2, CRYPT_HEIGHT - (2 * CRYPT_SECTION_SIZE), CRYPT_SECTION_SIZE):
		for x in range(CRYPT_SECTION_SIZE * 2, CRYPT_WIDTH - ( 2 * CRYPT_SECTION_SIZE), CRYPT_SECTION_SIZE):
			var cryptSection : Array
			var choice : float = rand_range(1, 10)
			if choice > 6:
				cryptSection = HORIZONTAL_HALLWAYS[randi() % len(HORIZONTAL_HALLWAYS)]
			else:
				cryptSection = VERTICAL_HALLWAYS[randi() % len(VERTICAL_HALLWAYS)]
			set_crypt_section(Vector2(x, y), cryptSection)
	draw_crypt()

	create_player()
	var numberOfEnemies : int = rand_range(20, 50)	
	for i in range(numberOfEnemies):
		var position : Vector2 = Vector2((floor(rand_range(3, (CRYPT_WIDTH / CRYPT_SECTION_SIZE) - 4)) + 0.5) * CRYPT_SECTION_SIZE, 
										 (floor(rand_range(3, (CRYPT_HEIGHT / CRYPT_SECTION_SIZE) - 4)) + 0.5) * CRYPT_SECTION_SIZE)
		create_enemy(position)

func initalize_crypt_object() -> void:
	crypt_globals.crypt = []
	for y in range(CRYPT_HEIGHT):
		crypt_globals.crypt.append([])
		crypt_globals.crypt[y].resize(CRYPT_WIDTH)
		for x in range(CRYPT_WIDTH):
			crypt_globals.crypt[y][x] = WALL_TILES[randi() % len(WALL_TILES)]

func set_crypt_section(originPosition : Vector2, cryptSection : Array) -> void:
	for y in range(len(cryptSection)):
		var cryptRow : Array = cryptSection[y]
		for x in range(len(cryptRow)):
			var tile : int = cryptRow[x]
			if cryptRow[x] == crypt_generator_globals.FLOOR:
				tile = FLOOR_TILES[randi() % len(FLOOR_TILES)]
			elif cryptRow[x] == crypt_generator_globals.WALL:
				tile = WALL_TILES[randi() % len(WALL_TILES)]
			crypt_globals.crypt[y + originPosition.y][x + originPosition.x] = tile