extends Area2D

func _on_Door_body_entered(body):
	print("Entered door")
	var crypts = get_tree().get_nodes_in_group("crypt")
	if len(crypts) == 0:
		print("There are no crypts...")
	else:
		crypts[0].create_room()