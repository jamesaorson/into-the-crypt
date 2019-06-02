extends Node

#warning-ignore-all:unused_class_variable
var Player : Resource = load("res://models/Player.gd")

export(float) var friction : float = 0.80

var player : Player = Player.new(
	null,
	Vector2(3, 2),
	2,
	3,
	Vector2(),
	7,
	15,
	50,
	false,
	2,
	2
)