extends Area2D

func _on_Door_body_entered(body):
	print(body)
	print("Entered door")
	var room = get_tree().get_nodes_in_group("room")[0]
	room.free()
	
	var player = get_tree().get_nodes_in_group("player")[0]
	player.free()
	
	queue_free()
	var crypt = get_tree().get_nodes_in_group("crypt")[0]
	crypt.create_room()