extends TileMap

class_name CryptGeneratorNode

var Enemy : Resource  = load(utility.construct_model_path('Enemy'))

onready var playerScene : Resource = load(utility.construct_scene_path('player'))
onready var enemyScene : Resource  = load(utility.construct_scene_path('enemy'))

const FLOOR_TILES : Array = [2, 3, 4]
const WALL_TILES : Array = [1]

const FLOOR : int = 0
const WALL : int = 1
const EXIT : int = 2

const CRYPT_SECTION_SIZE : int = 8

var CRYPT_HEIGHT : int
const CRYPT_MAX_HEIGHT : int = 30
const CRYPT_MIN_HEIGHT : int = 10

var CRYPT_WIDTH : int
const CRYPT_MAX_WIDTH : int = 30
const CRYPT_MIN_WIDTH : int = 10

####################
# Helper Functions #
####################

func create_enemy(mapPosition : Vector2) -> void:
	var enemy : EnemyNode = null
	enemy = enemyScene.instance()
	add_child(enemy)
	var enemyPosition : Vector2 = map_to_world(Vector2(mapPosition.x, mapPosition.y))
	enemy.position.x = enemyPosition.x
	enemy.position.y = enemyPosition.y
	enemy.scale = Vector2(0.75, 0.75)
	var enemyModel : Enemy = Enemy.new(enemy, mapPosition, 2, 3)
	enemyModel.maxHealth = 2.0
	enemyModel.health = 2.0
	crypt_manager_globals.enemies[enemyModel.get_instance_id()] = enemyModel
	enemy.enemyModel = enemyModel

func create_player() -> void:
	var playerNode : PlayerNode = null
	playerNode = playerScene.instance()
	add_child(playerNode)
	playerNode.player.instance = playerNode
	playerNode.player.health = 20.0
	playerNode.player.maxHealth = 20.0
	playerNode.initialize_player()
	var playerPosition : Vector2 = Vector2(playerNode.player.position.x, playerNode.player.position.y)
	playerPosition.x += 2 * CRYPT_SECTION_SIZE
	playerPosition.y += 2 * CRYPT_SECTION_SIZE
	playerPosition = map_to_world(playerPosition)
	playerNode.position.x = playerPosition.x
	playerNode.position.y = playerPosition.y
	playerNode.scale = Vector2(0.75, 0.75)

	var exitNodes : Array = get_tree().get_nodes_in_group('crypt_exit')
	if exitNodes != null:
		for node in exitNodes:
			node.position = playerNode.position

func draw_crypt() -> void:
	clear()
	for y in range(len(crypt_manager_globals.crypt)):
		for x in range(len(crypt_manager_globals.crypt[y])):
			set_cell(x, y, crypt_manager_globals.crypt[y][x])

func generate_crypt() -> void:
	if crypt_manager_globals.cryptSeed < 0:
		randomize()
		crypt_manager_globals.cryptSeed = randi()

	print('Generating crypt with seed ', crypt_manager_globals.cryptSeed)
	seed(crypt_manager_globals.cryptSeed)
	CRYPT_HEIGHT = CRYPT_SECTION_SIZE * int(floor(
		rand_range(CRYPT_MIN_HEIGHT, CRYPT_MAX_HEIGHT)
	))
	CRYPT_WIDTH = CRYPT_SECTION_SIZE * int(floor(
		rand_range(CRYPT_MIN_WIDTH, CRYPT_MAX_WIDTH)
	))
	
	var hallways : Array = generate_crypt_hallways()
	var horizontal_hallways = hallways[0]
	var vertical_hallways = hallways[1]

	initalize_crypt_object()
	for y in range(CRYPT_SECTION_SIZE * 2, CRYPT_HEIGHT - (2 * CRYPT_SECTION_SIZE), CRYPT_SECTION_SIZE):
		for x in range(CRYPT_SECTION_SIZE * 2, CRYPT_WIDTH - ( 2 * CRYPT_SECTION_SIZE), CRYPT_SECTION_SIZE):
			var cryptSection : Array
			var choice : float = rand_range(1, 10)
			if choice > 6:
				cryptSection = horizontal_hallways[randi() % len(horizontal_hallways)]
			else:
				cryptSection = vertical_hallways[randi() % len(vertical_hallways)]
			set_crypt_section(Vector2(x, y), cryptSection)
	draw_crypt()

	create_player()
	var numberOfEnemies : int = int(rand_range(20, 50))
	for _i in range(numberOfEnemies):
		var position : Vector2 = Vector2(
			(
				floor(
					rand_range(
						3,
						(CRYPT_WIDTH / CRYPT_SECTION_SIZE) - 4
					)
				)
				+ 0.5
			)
			* CRYPT_SECTION_SIZE,
			(
				floor(
					rand_range(
						3,
						(CRYPT_HEIGHT / CRYPT_SECTION_SIZE) - 4
					)
				)
				+ 0.5
			)
			* CRYPT_SECTION_SIZE
		)
		create_enemy(position)

func generate_crypt_hallways() -> Array:
	var horizontal_hallways : Array = []
	var vertical_hallways : Array = []
	var binaryLength : int = (CRYPT_SECTION_SIZE - 2) * 2
	var numberOfVariations : int = int(pow(2, binaryLength))
	
	for variation in range(numberOfVariations):
		horizontal_hallways.append([])
		horizontal_hallways[variation].resize(CRYPT_SECTION_SIZE)
		
		var binaryString : String = ''
		var binaryValue : int = variation
		for _digit in range(binaryLength):
			binaryString = str(binaryValue % 2) + binaryString
			binaryValue = binaryValue >> 1
		
		var horizontalVariation : Array = horizontal_hallways[variation]
		var halfBinaryLength : int = int(binaryLength / 2)
		for row in range(CRYPT_SECTION_SIZE):
			horizontalVariation[row] = []
			horizontalVariation[row].resize(CRYPT_SECTION_SIZE)
			
			if row == 0:
				horizontalVariation[row][0] = FLOOR
				horizontalVariation[row][CRYPT_SECTION_SIZE - 1] = WALL
				for i in range(1, halfBinaryLength + 1):
					horizontalVariation[row][i] = int(binaryString[i - 1])
			elif row == CRYPT_SECTION_SIZE - 1:
				horizontalVariation[row][0] = WALL
				horizontalVariation[row][CRYPT_SECTION_SIZE - 1] = WALL
				for i in range(1, halfBinaryLength + 1):
					horizontalVariation[row][i] = int(binaryString[i + halfBinaryLength - 1])
			else:
				for i in range(CRYPT_SECTION_SIZE):
					horizontalVariation[row][i] = FLOOR
					
		vertical_hallways.append([])
		vertical_hallways[variation].resize(CRYPT_SECTION_SIZE)

		var verticalVariation : Array = vertical_hallways[variation]
		for row in range(CRYPT_SECTION_SIZE):
			verticalVariation[row] = []
			verticalVariation[row].resize(CRYPT_SECTION_SIZE)
			if row == 0 or row == CRYPT_SECTION_SIZE - 1:
				for i in range(CRYPT_SECTION_SIZE):
					if i == 0 or i == CRYPT_SECTION_SIZE - 1:
						verticalVariation[row][i] = WALL
					else:
						verticalVariation[row][i] = FLOOR
			else:
				for i in range(CRYPT_SECTION_SIZE):
					if i != 0 and i != CRYPT_SECTION_SIZE - 1:
						verticalVariation[row][i] = FLOOR
				verticalVariation[row][0] = int(binaryString[(row - 1) * 2])
				verticalVariation[row][CRYPT_SECTION_SIZE - 1] = int(binaryString[((row - 1) * 2) + 1])
	
	return [horizontal_hallways, vertical_hallways]

func initalize_crypt_object() -> void:
	crypt_manager_globals.crypt = []
	for y in range(CRYPT_HEIGHT):
		crypt_manager_globals.crypt.append([])
		crypt_manager_globals.crypt[y].resize(CRYPT_WIDTH)
		for x in range(CRYPT_WIDTH):
			crypt_manager_globals.crypt[y][x] = WALL_TILES[randi() % len(WALL_TILES)]

func set_crypt_section(originPosition : Vector2, cryptSection : Array) -> void:
	for y in range(len(cryptSection)):
		var cryptRow : Array = cryptSection[y]
		for x in range(len(cryptRow)):
			var tile : int = cryptRow[x]
			if cryptRow[x] == FLOOR:
				tile = FLOOR_TILES[randi() % len(FLOOR_TILES)]
			elif cryptRow[x] == WALL:
				tile = WALL_TILES[randi() % len(WALL_TILES)]
			crypt_manager_globals.crypt[y + originPosition.y][x + originPosition.x] = tile