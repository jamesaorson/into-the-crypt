extends TileMap

onready var playerScene = load("res://player/player.tscn")

const FLOOR_TILE = 0
const WALL_TILE = 1

const CRYPT_SECTION_SIZE = 4

var CRYPT_HEIGHT
const CRYPT_MAX_HEIGHT = 100
const CRYPT_MIN_HEIGHT = 30

var CRYPT_WIDTH
const CRYPT_MAX_WIDTH = 100
const CRYPT_MIN_WIDTH = 30

const PLAYER_START_POSITION = Vector2(3, 3)
const PLAYER_HEIGHT = 3
const PLAYER_WIDTH = 2

const CORNERS = [
	[
		[1, 1, 1, 1],
		[0, 0, 0, 1],
		[0, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 1],
		[0, 0, 0, 1],
		[1, 1, 1, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 0],
		[1, 0, 0, 0],
		[1, 1, 1, 1]
	],
	[
		[1, 1, 1, 1],
		[1, 0, 0, 0],
		[1, 0, 0, 0],
		[1, 0, 0, 1]
	]
]
const TOP_RIGHT_CORNER    = CORNERS[0]
const BOTTOM_RIGHT_CORNER = CORNERS[1]
const BOTTOM_LEFT_CORNER  = CORNERS[2]
const TOP_LEFT_CORNER     = CORNERS[3]

const HORIZONTAL_HALLWAYS = [
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 1, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 1, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 1, 1]
	],
	[
		[1, 0, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 1, 1]
	],
	[
		[1, 1, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 1, 1]
	],
	[
		[1, 1, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 1, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 0, 1]
	],
	[
		[1, 0, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 0, 1]
	],
	[
		[1, 1, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 0, 1]
	],
	[
		[1, 1, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 1, 1]
	],
	[
		[1, 0, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 1, 1]
	],
	[
		[1, 1, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 1, 1]
	],
	[
		[1, 1, 1, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 1, 1, 1]
	]
]
const TOP_HORIZONTAL_HALLWAYS = [
	HORIZONTAL_HALLWAYS[3],
	HORIZONTAL_HALLWAYS[7],
	HORIZONTAL_HALLWAYS[11],
	HORIZONTAL_HALLWAYS[15]
]
const BOTTOM_HORIZONTAL_HALLWAYS = [
	HORIZONTAL_HALLWAYS[12],
	HORIZONTAL_HALLWAYS[13],
	HORIZONTAL_HALLWAYS[14],
	HORIZONTAL_HALLWAYS[15]
]
const VERTICAL_HALLWAYS   = [
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 1],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 0],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[0, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 1],
		[0, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 0],
		[0, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 1],
		[0, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[1, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 1],
		[1, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 0],
		[1, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 1],
		[1, 0, 0, 0],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 0],
		[1, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[0, 0, 0, 1],
		[1, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 0],
		[1, 0, 0, 1],
		[1, 0, 0, 1]
	],
	[
		[1, 0, 0, 1],
		[1, 0, 0, 1],
		[1, 0, 0, 1],
		[1, 0, 0, 1]
	]
]
const LEFT_VERTICAL_HALLWAYS = [
	VERTICAL_HALLWAYS[10],
	VERTICAL_HALLWAYS[11],
	VERTICAL_HALLWAYS[14],
	VERTICAL_HALLWAYS[15]
]
const RIGHT_VERTICAL_HALLWAYS = [
	VERTICAL_HALLWAYS[5],
	VERTICAL_HALLWAYS[7],
	VERTICAL_HALLWAYS[13],
	VERTICAL_HALLWAYS[15]
]

var playerStartPosition = map_to_world(PLAYER_START_POSITION)
var crypt = []
var cryptSeed = -1

####################
# Helper Functions #
####################

func create_player():
	var player = null
	var playerNodes = get_tree().get_nodes_in_group("player")
	if len(playerNodes) != 0:
		player = playerNodes[0]
	else:
		player = playerScene.instance()
		get_tree().root.add_child(player)
	player.position.x = self.playerStartPosition.x
	player.position.y = self.playerStartPosition.y
	player.set_crypt_seed_text(self.cryptSeed)

func draw_crypt():
	for y in range(len(self.crypt)):
		for x in range(len(self.crypt[y])):
			set_cell(x, y, self.crypt[y][x])

func generate_crypt(cryptSeed):
	self.cryptSeed = cryptSeed
	print("Generating crypt with seed ", self.cryptSeed)
	seed(self.cryptSeed)
	clear()
	CRYPT_HEIGHT = CRYPT_SECTION_SIZE * floor(rand_range(CRYPT_MIN_HEIGHT, CRYPT_MAX_HEIGHT))
	CRYPT_WIDTH = CRYPT_SECTION_SIZE * floor(rand_range(CRYPT_MIN_WIDTH, CRYPT_MAX_WIDTH))
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
	create_player()

func initalize_crypt_object():
	self.crypt = []
	for y in range(CRYPT_HEIGHT):
		self.crypt.append([])
		self.crypt[y].resize(CRYPT_WIDTH)

func set_crypt_section(originPosition, cryptSection):
	for y in range(len(cryptSection)):
		var cryptRow = cryptSection[y]
		for x in range(len(cryptRow)):
			self.crypt[y + originPosition.y][x + originPosition.x] = cryptRow[x]