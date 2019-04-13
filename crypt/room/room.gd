extends TileMap

onready var playerScene = load("res://player/player.tscn")

const FLOOR_TILE = 0
const WALL_TILE = 1

const ROOM_HEIGHT = 18
const ROOM_WIDTH = 32

const PLAYER_START_POSITION = Vector2(3, 3)
const PLAYER_HEIGHT = 3
const PLAYER_WIDTH = 2

var playerStartPosition = map_to_world(PLAYER_START_POSITION)
var room = []

func generate_room():
	print("Room generation...")
	randomize()
	for y in range(ROOM_HEIGHT):
		self.room.append([])
		self.room[y].resize(ROOM_WIDTH)
		for x in range(ROOM_WIDTH):
			if (x >= PLAYER_START_POSITION.x and 
			    x < PLAYER_START_POSITION.x + PLAYER_WIDTH and 
				y >= PLAYER_START_POSITION.y and
				y < PLAYER_START_POSITION.y + PLAYER_HEIGHT):
				self.room[y][x] = FLOOR_TILE
			elif x == 0 or x == ROOM_WIDTH - 1 or y == 0 or y == ROOM_HEIGHT - 1:
				self.room[y][x] = WALL_TILE
			else:
				var tileChoice = rand_range(1, 10)
				if tileChoice > 2:
					self.room[y][x] = FLOOR_TILE
				else:
					self.room[y][x] = WALL_TILE
	draw_room()
	create_player()

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

func draw_room():
	for y in range(len(room)):
		for x in range(len(room[y])):
			set_cell(x, y, room[y][x])