extends Node

var Player = load("res://models/Player.gd")

const UP = { vector = Vector2(0, -1), inputName = "player_move_up_0" }
const DOWN = { vector = -UP.vector, inputName = "player_move_down_0" }
const LEFT = { vector = Vector2(-1, 0), inputName = "player_move_left_0" }
const RIGHT = { vector = -LEFT.vector, inputName = "player_move_right_0" }

var friction = 0.85

var numberOfPlayers = 1

var players = [
	Player.new(
		null,
		Vector2(3, 2),
		2,
		3,
		Vector2(),
		7,
		1000,
		3000,
		false,
		0
	),
	Player.new(
		null,
		Vector2(6, 2),
		2,
		3,
		Vector2(),
		7,
		1000,
		3000,
		false,
		1
	)
]