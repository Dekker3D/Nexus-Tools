using NUnit.Framework;

namespace LibNexus.Files.Tests;

// ReSharper disable once MemberCanBeFileLocal
public class FileSystemTests
{
	private static readonly string[] ExpectedFiles = ["Test1.txt"];
	private static readonly string[] ExpectedDirectories = ["Test1", "Test3"];

	[Test]
	public void TestReadWildStar()
	{
		const string baseDir = "C:/Users/Andre/RiderProjects/WildStar/Assets/";

		Validate($"{baseDir}Bootstrap/Bootstrap", $"{baseDir}Bootstrap/Bootstrap");
		Validate($"{baseDir}Bootstrap/Bootstrap", $"{baseDir}Bootstrap/Bootstrap");
		Validate($"{baseDir}Bootstrap/Launcher", $"{baseDir}Bootstrap/Launcher");
		Validate($"{baseDir}Bootstrap/LauncherData");
		Validate($"{baseDir}Patch/Patch", $"{baseDir}Bootstrap/Patch");
		////Validate($"{baseDir}Patch/Client", $"{baseDir}Bootstrap/Client");
		Validate($"{baseDir}Patch/Client64", $"{baseDir}Bootstrap/Client64");
		Validate($"{baseDir}Patch/ClientData");
		Validate($"{baseDir}Patch/ClientDataDE");
		Validate($"{baseDir}Patch/ClientDataEN");
		Validate($"{baseDir}Patch/ClientDataFR");
	}

	private static void Validate(string path, string? directory = null)
	{
		using var fileSystem = new FileSystem(path, directory);
		Console.WriteLine(path);
		Console.WriteLine("########################");
		Validate(fileSystem, string.Empty, true);
	}

	private static void Validate(FileSystem fileSystem, string path, bool recursive)
	{
		if (recursive)
		{
			foreach (var directory in fileSystem.ListDirectories(path))
				Validate(fileSystem, $"{path}/{directory}", recursive);
		}

		foreach (var file in fileSystem.ListFiles(path))
		{
			var valid = fileSystem.Validate($"{path}/{file}");

			Console.WriteLine($"- {file} " + (valid ? "ok" : "failed"));
		}
	}

	[Test]
	public void TestCreatePacked()
	{
		File.Delete("packed.index");
		File.Delete("packed.archive");

		Assert.Multiple(
			static () =>
			{
				FileSystem? fileSystem = null;
				Assert.That(() => fileSystem = new FileSystem("packed"), Throws.Nothing);

				if (fileSystem == null)
					return;

				Assert.That(File.Exists("packed.index"), Is.True);
				Assert.That(File.Exists("packed.archive"), Is.True);

				TestFileSystem(fileSystem);
			}
		);
	}

	[Test]
	public void TestCreateUnpacked()
	{
		File.Delete("unpacked.index");
		Directory.Delete("unpacked", true);

		Assert.Multiple(
			static () =>
			{
				FileSystem? fileSystem = null;
				Assert.That(() => fileSystem = new FileSystem("unpacked", "unpacked"), Throws.Nothing);

				if (fileSystem == null)
					return;

				Assert.That(File.Exists("unpacked.index"), Is.True);
				Assert.That(Directory.Exists("unpacked"), Is.True);

				TestFileSystem(fileSystem);
			}
		);
	}

	private static void TestFileSystem(FileSystem fileSystem)
	{
		Assert.That(() => fileSystem.Write("Test1.txt", "Hello world 1!"u8.ToArray(), DateTime.UtcNow), Throws.Nothing);
		Assert.That(() => fileSystem.Write("Test2.txt", "Hello world 2!"u8.ToArray(), DateTime.UtcNow), Throws.Nothing);

		Assert.That(() => fileSystem.Validate("Test1.txt"), Is.True);
		Assert.That(() => fileSystem.Validate("Test3.txt"), Is.False);

		Assert.That(() => fileSystem.CreateDirectory("Test1"), Throws.Nothing);
		Assert.That(() => fileSystem.CreateDirectory("Test2"), Throws.Nothing);

		Assert.That(() => fileSystem.Rename("Test2", "Test3"), Throws.Nothing);
		Assert.That(() => fileSystem.Rename("Test2.txt", "Test3/Test3.txt"), Throws.Nothing);

		Assert.That(() => fileSystem.ListFiles(string.Empty), Is.EqualTo(ExpectedFiles));
		Assert.That(() => fileSystem.ListDirectories(string.Empty), Is.EqualTo(ExpectedDirectories));

		Assert.That(() => fileSystem.Read("Test1.txt"), Is.EqualTo("Hello world 1!"u8.ToArray()));

		Assert.That(() => fileSystem.Delete("Test2"), Throws.Nothing);
		Assert.That(() => fileSystem.Delete("TestDirectory/Test2.txt"), Throws.Nothing);
	}
}
