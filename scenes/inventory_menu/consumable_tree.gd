extends InventoryTree

var rootTreeItem : TreeItem
var replenishingTreeItem : TreeItem
var buffTreeItem : TreeItem
var throwableTreeItem : TreeItem

var selectedItem : TreeItem

const CONSUME : int = 0
const EXIT : int = 1

onready var contextMenu : PopupMenu = $'..'/ContextMenu
onready var itemInfo : PanelContainer = $'..'/ItemInfo
onready var itemInfoLabel : RichTextLabel = $'..'/ItemInfo/RichTextLabel

####################
# Helper Functions #
####################

func update_tree(items : Dictionary):
	self.clear()
	self.rootTreeItem = self.create_item(self)
	self.hide_root = true
	self.columns = 2

	self.replenishingTreeItem = self.create_item(self.rootTreeItem)
	self.replenishingTreeItem.set_text(0, 'Replenishing')
	utility.add_items_to_tree(
		self,
		self.replenishingTreeItem,
		items[inventory_globals.ITEM_TYPE.REPLENISHING]
	)

	self.buffTreeItem = self.create_item(self.rootTreeItem)
	self.buffTreeItem.set_text(0, 'Buffs')
	utility.add_items_to_tree(
		self,
		self.buffTreeItem,
		items[inventory_globals.ITEM_TYPE.BUFF]
	)

	self.throwableTreeItem = self.create_item(self.rootTreeItem)
	self.throwableTreeItem.set_text(0, 'Throwables')
	utility.add_items_to_tree(
		self,
		self.throwableTreeItem,
		items[inventory_globals.ITEM_TYPE.THROWABLE]
	)

###################
# Signal handlers #
###################

func _on_Tree_item_rmb_selected(clickPosition : Vector2) -> void:
	self.contextMenu.clear()
	self.contextMenu.rect_position = self.rect_global_position + clickPosition
	self.contextMenu.visible = true
	self.contextMenu.add_item('Consume', CONSUME)
	self.contextMenu.add_item('Exit', EXIT)

func _on_ContextMenu_id_pressed(ID : int):
	match ID:
		CONSUME:
			print('consume')
		EXIT:
			self.contextMenu.visible = false

func _on_Tree_cell_selected():
	self.show_item_info(self.itemInfoLabel)