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

var HORIZONTAL_HALLWAYS = crypt_generator_globals.HORIZONTAL_HALLWAYS

var VERTICAL_HALLWAYS = crypt_generator_globals.VERTICAL_HALLWAYS

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
		var playerStartPosition = Vector2(player_globals.players[playerIndex].startPosition.x, player_globals.players[playerIndex].startPosition.y)
		playerStartPosition.x += 2 * CRYPT_SECTION_SIZE
		playerStartPosition.y += 2 * CRYPT_SECTION_SIZE
		playerStartPosition = map_to_world(playerStartPosition)
		player.position.x = playerStartPosition.x
		player.position.y = playerStartPosition.y
		player_globals.players[playerIndex].timeStart = OS.get_unix_time()

func destroy():
	var playerNodes = get_tree().get_nodes_in_group("player")
	for playerNode in playerNodes:
		playerNode.destroy()
	for player in player_globals.players:
		player.instance = null
		player.debugInfo = null
		player.lightNode = null
	queue_free()

func draw_crypt():
	clear()
	for y in range(len(crypt_globals.crypt)):
		for x in range(len(crypt_globals.crypt[y])):
			set_cell(x, y, crypt_globals.crypt[y][x])

func generate_crypt():
	if crypt_globals.cryptSeed == null:
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
			var cryptSection = null
			var choice = rand_range(1, 10)
			if choice > 6:
				cryptSection = HORIZONTAL_HALLWAYS[randi() % len(HORIZONTAL_HALLWAYS)]
			else:
				cryptSection = VERTICAL_HALLWAYS[randi() % len(VERTICAL_HALLWAYS)]
			set_crypt_section(Vector2(x, y), cryptSection)
	draw_crypt()
	for playerIndex in range(player_globals.numberOfPlayers, 0, -1):
		create_player(playerIndex - 1)

func initalize_crypt_object():
	crypt_globals.crypt = []
	for y in range(CRYPT_HEIGHT):
		crypt_globals.crypt.append([])
		crypt_globals.crypt[y].resize(CRYPT_WIDTH)
		for x in range(CRYPT_WIDTH):
			crypt_globals.crypt[y][x] = WALL_TILE

func set_crypt_section(originPosition, cryptSection):
	for y in range(len(cryptSection)):
		var cryptRow = cryptSection[y]
		for x in range(len(cryptRow)):
			crypt_globals.crypt[y + originPosition.y][x + originPosition.x] = cryptRow[x]