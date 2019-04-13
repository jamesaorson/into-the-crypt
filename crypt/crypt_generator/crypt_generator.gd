extends TileMap

onready var playerScene = load("res://player/player.tscn")

const FLOOR_TILE = crypt_generator_globals.FLOOR_TILE
const WALL_TILE = crypt_generator_globals.WALL_TILE

const CRYPT_SECTION_SIZE = crypt_generator_globals.CRYPT_SECTION_SIZE

var CRYPT_HEIGHT
const CRYPT_MAX_HEIGHT = crypt_generator_globals.CRYPT_MAX_HEIGHT
const CRYPT_MIN_HEIGHT = crypt_generator_globals.CRYPT_MIN_HEIGHT

var CRYPT_WIDTH 
const CRYPT_MAX_WIDTH = crypt_generator_globals.CRYPT_MAX_WIDTH
const CRYPT_MIN_WIDTH = crypt_generator_globals.CRYPT_MIN_WIDTH

const CORNERS             = crypt_generator_globals.CORNERS
const TOP_RIGHT_CORNER    = crypt_generator_globals.TOP_RIGHT_CORNER
const BOTTOM_RIGHT_CORNER = crypt_generator_globals.BOTTOM_RIGHT_CORNER
const BOTTOM_LEFT_CORNER  = crypt_generator_globals.BOTTOM_LEFT_CORNER
const TOP_LEFT_CORNER     = crypt_generator_globals.TOP_LEFT_CORNER

const HORIZONTAL_HALLWAYS = crypt_generator_globals.HORIZONTAL_HALLWAYS
const TOP_HORIZONTAL_HALLWAYS = crypt_generator_globals.TOP_HORIZONTAL_HALLWAYS
const BOTTOM_HORIZONTAL_HALLWAYS = crypt_generator_globals.BOTTOM_HORIZONTAL_HALLWAYS

const VERTICAL_HALLWAYS = crypt_generator_globals.VERTICAL_HALLWAYS
const LEFT_VERTICAL_HALLWAYS = crypt_generator_globals.LEFT_VERTICAL_HALLWAYS
const RIGHT_VERTICAL_HALLWAYS = crypt_generator_globals.RIGHT_VERTICAL_HALLWAYS

####################
# Helper Functions #
####################

func create_player(playerIndex):
	if playerIndex != null:
		var player = null
		if player_globals.players[playerIndex].instance != null:
			player = player_globals.players[playerIndex].instance
		else:
			player = playerScene.instance()
			player_globals.players[playerIndex].instance = player
			player.set_player_index(playerIndex)
			get_tree().root.add_child(player)
		player.set_player_index(playerIndex)
		var playerStartPosition = map_to_world(player_globals.players[playerIndex].startPosition)
		player.position.x = playerStartPosition.x
		player.position.y = playerStartPosition.y

func destroy():
	var playerNodes = get_tree().get_nodes_in_group("player")
	for playerNode in playerNodes:
		playerNode.destroy()
	queue_free()

func draw_crypt():
	for y in range(len(crypt_globals.crypt)):
		for x in range(len(crypt_globals.crypt[y])):
			set_cell(x, y, crypt_globals.crypt[y][x])

func generate_crypt():
	if crypt_globals.cryptSeed == null:
		randomize()
		crypt_globals.cryptSeed = randi()
	print("Generating crypt with seed ", crypt_globals.cryptSeed)
	seed(crypt_globals.cryptSeed)
	clear()
	crypt_generator_globals.CRYPT_HEIGHT = CRYPT_SECTION_SIZE * floor(rand_range(CRYPT_MIN_HEIGHT, CRYPT_MAX_HEIGHT))
	CRYPT_HEIGHT = crypt_generator_globals.CRYPT_HEIGHT
	crypt_generator_globals.CRYPT_WIDTH = CRYPT_SECTION_SIZE * floor(rand_range(CRYPT_MIN_WIDTH, CRYPT_MAX_WIDTH))
	CRYPT_WIDTH = crypt_generator_globals.CRYPT_WIDTH
	initalize_crypt_object()
	for y in range(0, CRYPT_HEIGHT, CRYPT_SECTION_SIZE):
		for x in range(0, CRYPT_WIDTH, CRYPT_SECTION_SIZE):
			var cryptSection = null
			
			if y == 0:
				if x == 0:
					cryptSection = TOP_LEFT_CORNER
				elif x == CRYPT_WIDTH - CRYPT_SECTION_SIZE:
					cryptSection = TOP_RIGHT_CORNER
				else:
					cryptSection = TOP_HORIZONTAL_HALLWAYS[floor(rand_range(1, len(TOP_HORIZONTAL_HALLWAYS))) - 1]
			elif y == CRYPT_HEIGHT - CRYPT_SECTION_SIZE:
				if x == CRYPT_WIDTH - CRYPT_SECTION_SIZE:
					cryptSection = BOTTOM_RIGHT_CORNER
				elif x == 0:
					cryptSection = BOTTOM_LEFT_CORNER
				else:
					cryptSection = BOTTOM_HORIZONTAL_HALLWAYS[floor(rand_range(1, len(BOTTOM_HORIZONTAL_HALLWAYS))) - 1]
			elif x == 0:
				cryptSection = LEFT_VERTICAL_HALLWAYS[floor(rand_range(1, len(LEFT_VERTICAL_HALLWAYS))) - 1]
			elif x == CRYPT_WIDTH - CRYPT_SECTION_SIZE:
				cryptSection = RIGHT_VERTICAL_HALLWAYS[floor(rand_range(1, len(RIGHT_VERTICAL_HALLWAYS))) - 1]
			else:
				var choice = rand_range(1, 10)
				if choice > 6:
					cryptSection = HORIZONTAL_HALLWAYS[floor(rand_range(1, len(HORIZONTAL_HALLWAYS))) - 1]
				else:
					cryptSection = VERTICAL_HALLWAYS[floor(rand_range(1, len(VERTICAL_HALLWAYS))) - 1]
			set_crypt_section(Vector2(x, y), cryptSection)
	draw_crypt()
	create_player(1)
	create_player(0)

func initalize_crypt_object():
	crypt_globals.crypt = []
	for y in range(CRYPT_HEIGHT):
		crypt_globals.crypt.append([])
		crypt_globals.crypt[y].resize(CRYPT_WIDTH)

func set_crypt_section(originPosition, cryptSection):
	for y in range(len(cryptSection)):
		var cryptRow = cryptSection[y]
		for x in range(len(cryptRow)):
			crypt_globals.crypt[y + originPosition.y][x + originPosition.x] = cryptRow[x]