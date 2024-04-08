using Godot;
using System;
using System.IO.Ports;

public partial class magnetometro : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private MeshInstance3D _magnet;
	private Camera3D _camera; 
	private SerialPort _serialPort;
	private Vector3 _prevPosition = new Vector3(0, 0, 0);
	private Vector3 _newPosition = new Vector3(0, 0, 0);

	[Export]
	public float _coefficenteInterpolazione = 4;
	public override void _Ready()
	{
		_magnet = GetNode("magnet") as MeshInstance3D;
		_camera = GetNode("Camera3D") as Camera3D;
		
		try
		{
			GD.Print("using comport " + Globals.ComPort);
			_serialPort = new SerialPort(Globals.ComPort, 9600, Parity.None, 8, StopBits.One);
			_serialPort.DtrEnable = true;
			_serialPort.RtsEnable = true;
			_serialPort.Open();
			_serialPort.DataReceived += serialPort_DataReceived;
		}
		catch(Exception e)
		{
			GD.Print($"ERRORE {e.Message}");
		}
	}

	private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		string data = _serialPort.ReadLine();
		GD.Print(data);
		try
		{

			var dataSplitted = data.Split(' ');

			string Xstring = dataSplitted[1];
			string Ystring = dataSplitted[3];
			string Zstring = dataSplitted[5];

			if (int.TryParse(Xstring, out int x) && int.TryParse(Ystring, out int y) && int.TryParse(Zstring, out int z))
			{
				_prevPosition = _newPosition;
				_newPosition = new Vector3(x, y, z);
				_t = 0f;

			}
		}
		catch(Exception ex)
		{
			GD.Print($"ERRORE {ex.Message}");
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//arduino invia una coordinata ogni 200 ms
	//
	private float _t = 0.0f;

	public override void _Process(double delta)
	{
		_t += (float)delta * _coefficenteInterpolazione;

		_magnet.GlobalPosition = _prevPosition.Lerp(_newPosition, _t);

	}
	public void _on_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://menu.tscn");
	}

}

