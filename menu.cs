using Godot;
using System;
using System.IO.Ports;
using System.Linq;

public partial class menu : Control
{
	private ItemList _itemList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_itemList = GetNode<ItemList>("ItemList"); 
		var ports = SerialPort.GetPortNames();

		foreach (var port in ports)
		{
			_itemList.AddItem(port);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_button_pressed()
	{
		int? itemIdx = _itemList.GetSelectedItems().FirstOrDefault();

		if (itemIdx != null)
		{
			Globals.ComPort = _itemList.GetItemText(itemIdx.Value);
			GetTree().ChangeSceneToFile("res://node_3d.tscn");
		}
	}

}

