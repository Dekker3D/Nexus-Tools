using System.Xml;

namespace Nexus.Editor.Program;

public static class ProgramSettings
{
	private static List<string> lastProjects = new List<string>();
	public static void NoteProjectLoaded(string path)
	{
		lastProjects.Remove(path);
		lastProjects.Insert(0, path);
		if (lastProjects.Count > 10)
		{
			lastProjects.Remove(lastProjects.Last());
		}

		Save();
	}

	public static IReadOnlyList<string> GetLastProjects()
	{
		return lastProjects.AsReadOnly();
	}

	private static string AppDataPath { get => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EldanToolkit"); }
	private static string AppSettingsPath { get => Path.Join(AppDataPath, "AppSettings.xml"); }

	public static void Save()
	{
		if (!Directory.Exists(AppDataPath))
			Directory.CreateDirectory(AppDataPath);

		var doc = new XmlDocument();
		XmlNode root = doc.CreateElement("ProgramSettings");
		doc.AppendChild(root);
		XmlNode set = doc.CreateElement("Settings");
		root.AppendChild(set);

		XmlNode lpn = doc.CreateElement("LastProjects");
		root.AppendChild(lpn);
		foreach (var path in lastProjects)
		{
			XmlNode p = doc.CreateElement("Path");
			p.InnerText = path;
			lpn.AppendChild(p);
		}

		doc.Save(AppSettingsPath);
	}

	public static void Load()
	{
		if (!File.Exists(AppSettingsPath))
			return;

		var doc = new XmlDocument();
		doc.Load(AppSettingsPath);

		var lpn = doc.SelectSingleNode("/ProgramSettings/LastProjects");
		if (lpn != null)
		{
			lastProjects.Clear();
			foreach (XmlNode p in lpn.ChildNodes)
			{
				lastProjects.Add(p.InnerText);
			}
		}
	}
}
