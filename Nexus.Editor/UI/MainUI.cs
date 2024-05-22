using Godot;

namespace Nexus.Editor.UI;

public partial class MainUI : Control
{
	[Export]
	private NodePath? fileMenuPath;
	[Export]
	private int recentProjectsItemIndex = 0;
	[Export]
	private NodePath? recentProjectsPath;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var fm = GetNode(fileMenuPath) as PopupMenu;
		var rp = GetNode(recentProjectsPath) as PopupMenu;
		if (fm != null && rp != null)
		{
			fm.SetItemSubmenu(recentProjectsItemIndex, recentProjectsPath?.ToString());
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		fileMenuPath?.Dispose();
		recentProjectsPath?.Dispose();
	}
}
