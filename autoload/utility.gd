extends Node

func construct_model_path(model : String) -> String:
	return 'res://models/{model}.gd'.format({'model': model})

func construct_scene_path(scene : String) -> String:
	return 'res://scenes/{scene}/{scene}.tscn'.format({'scene': scene})

func error_handled_scene_change(scene_path : String) -> void:
	var error_message : String = ''
	if scene_path == null:
		error_message += 'Scene Path was null. '

	if !error_message.empty():
		print(error_message)
	else:
		scene_path = construct_scene_path(scene_path)
		var error : int = get_tree().change_scene(scene_path)
		match error:
			OK:
				print('Successfully changed to {scene_path}.'.format({'scene_path': scene_path}))
			_:
				print('Failure in changing to {scene_path}: {error}.'.format(
						{
							'scene_path': scene_path,
							'error': error
						}
					)
				)

func add_items_to_tree(tree : Tree, root : TreeItem, items : Dictionary) -> void:
	for key in items.keys():
		var item : InventoryMenuItem = items[key]
		var subItem : TreeItem = tree.create_item(root, 0)
		subItem.set_text(
			0,
			item.name
		)
		subItem.set_text(
			1,
			'Quantity: {quantity}'.format({'quantity': item.quantity})
		)
		subItem.set_text_align(1, TreeItem.ALIGN_RIGHT)
		subItem.set_selectable(1, false)

		if item.get('isEquipped'):
			subItem.set_custom_bg_color(0, Color.green, true)