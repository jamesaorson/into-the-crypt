extends TileMap

onready var playerScene = load("res://player/player.tscn")

const FLOOR_TILE = 0
const WALL_TILE = 1

const CRYPT_HEIGHT = 18
const CRYPT_WIDTH = 32

const PLAYER_START_POSITION = Vector2(3, 3)
const PLAYER_HEIGHT = 3
const PLAYER_WIDTH = 2

var playerStartPosition = map_to_world(PLAYER_START_POSITION)
var crypt = []

####################
# Helper Functions #
####################

func create_player():
	var player = null
	var playerNodes = get_tree().get_nodes_in_group("player")
	if len(playerNodes) != 0:
		player = playerNodes[0]
	else:
		print("Creating player at: ", world_to_map(self.playerStartPosition), " ", self.playerStartPosition)
		player = playerScene.instance()
		get_tree().root.add_child(player)
	player.position.x = self.playerStartPosition.x
	player.position.y = self.playerStartPosition.y

func draw_crypt():
	for y in range(len(self.crypt)):
		for x in range(len(self.crypt[y])):
			set_cell(x, y, self.crypt[y][x])

func generate_crypt():
	print("Generating crypt...")
	randomize()
	for y in range(CRYPT_HEIGHT):
		self.crypt.append([])
		self.crypt[y].resize(CRYPT_WIDTH)
		for x in range(CRYPT_WIDTH):
			if (x >= PLAYER_START_POSITION.x and 
			    x < PLAYER_START_POSITION.x + PLAYER_WIDTH and 
				y >= PLAYER_START_POSITION.y and
				y < PLAYER_START_POSITION.y + PLAYER_HEIGHT):
				self.crypt[y][x] = FLOOR_TILE
			elif x == 0 or x == CRYPT_WIDTH - 1 or y == 0 or y == CRYPT_HEIGHT - 1:
				self.crypt[y][x] = WALL_TILE
			else:
				var tileChoice = rand_range(1, 10)
				if tileChoice > 2:
					self.crypt[y][x] = FLOOR_TILE
				else:
					self.crypt[y][x] = WALL_TILE
	draw_crypt()
	create_player()