extends TileMap

onready var doorScene = load("res://crypt/door/door.tscn")
onready var playerScene = load("res://player/player.tscn")

const OPEN_TILE = 0
const WALL_TILE = 1
const DOOR_TILE = 0

const ROOM_HEIGHT = 18
const ROOM_WIDTH = 32

const PLAYER_START_POSITION = Vector2(3, 3)
const PLAYER_HEIGHT = 3
const PLAYER_WIDTH = 2

var DOOR_TILEMAP_POSITION = Vector2(ROOM_WIDTH - 1, ROOM_HEIGHT / 2)
const DOOR_HEIGHT = 2

var doorPosition = map_to_world(DOOR_TILEMAP_POSITION)
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
				self.room[y][x] = OPEN_TILE
			elif (x >= DOOR_TILEMAP_POSITION.x - PLAYER_WIDTH and
				  x <= DOOR_TILEMAP_POSITION.x and
				  y >= DOOR_TILEMAP_POSITION.y and
				  y < DOOR_TILEMAP_POSITION.y + DOOR_HEIGHT):
				self.room[y][x] = DOOR_TILE
			elif x == 0 or x == ROOM_WIDTH - 1 or y == 0 or y == ROOM_HEIGHT - 1:
				self.room[y][x] = WALL_TILE
			else:
				var tileChoice = rand_range(1, 10)
				if tileChoice > 2:
					self.room[y][x] = OPEN_TILE
				else:
					self.room[y][x] = WALL_TILE
	draw_room()
	create_player()
	create_door()

func create_door():
	var door = null
	var doorNodes = get_tree().get_nodes_in_group("door")
	if len(doorNodes) != 0:
		door = doorNodes[0]
	else:
		print("Creating door at: ", world_to_map(self.doorPosition), " ", self.doorPosition)
		door = doorScene.instance()
		get_tree().root.add_child(door)
	door.position.x = self.doorPosition.x
	door.position.y = self.doorPosition.y

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