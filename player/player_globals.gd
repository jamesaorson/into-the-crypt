extends Node

var Player = load("res://models/Player.gd")

const UP = Vector2(0, -1)
const DOWN = -UP
const LEFT = Vector2(-1, 0)
const RIGHT = -LEFT

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
		15,
		50,
		false,
		0
	)
]