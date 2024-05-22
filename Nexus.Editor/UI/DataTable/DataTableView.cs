using Godot;
using System.Data;

namespace Nexus.Editor.UI.TableEditors;
public partial class DataTableView : GridContainer
{
	private DataTable? table;

	[Export]
	public int RowHeight { get; set; } = 18;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		table = new DataTable();
		table.Columns.Add("Test", typeof(string));
		table.Columns.Add("Test2", typeof(string));

		table.Rows.Add("Blah", "Blah");

		Refresh();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Refresh() // Just a dumb refresh. More optimized options can be built later.
	{
		foreach (var child in GetChildren())
		{
			child.Free();
		}

		if (table == null)
		{
			return;
		}

		Columns = table.Columns.Count;

		foreach (DataColumn column in table.Columns)
		{
			var label = new Label();

			AddChild(label);
			label.Text = column.ColumnName;
			label.CustomMinimumSize = new Vector2(0, RowHeight);
		}

		foreach (DataRow row in table.Rows)
		{
			foreach (var item in row.ItemArray)
			{
				var editBox = new TextEdit();

				AddChild(editBox);
				editBox.Text = item?.ToString() ?? string.Empty;
				editBox.CustomMinimumSize = new Vector2(0, RowHeight);
			}
		}
	}

	public void SetTable(DataTable table)
	{
		this.table = table;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		table?.Dispose();
	}
}
