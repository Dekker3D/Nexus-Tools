﻿using LibNexus.Files.Extensions;
using System.Data;

namespace LibNexus.Files.ArchiveFiles;

public class ArchiveHeader
{
	private const string Magic = "AARC";
	private const uint Version = 2;
	public const uint Stride = 16;

	private readonly Stream _stream;
	private readonly long _position;

	private uint _files;
	private uint _filesPage;

	public uint Files
	{
		get => _files;

		set
		{
			if (_files == value)
				return;

			_stream.Position = _position + 8;
			_stream.WriteUInt32(value);

			_files = value;
		}
	}

	public uint FilesPage
	{
		get => _filesPage;

		set
		{
			if (_filesPage == value)
				return;

			_stream.Position = _position + 12;
			_stream.WriteUInt32(value);

			_filesPage = value;
		}
	}

	public ArchiveHeader(Stream stream)
	{
		_stream = stream;
		_position = _stream.Position;

		var aidx = _stream.ReadWord();
		var version = _stream.ReadUInt32();

		if (aidx != Magic)
			throw new DataException("ArchiveHeader: Invalid magic");

		if (version != Version)
			throw new DataException("ArchiveHeader: Invalid version");

		_files = _stream.ReadUInt32();
		_filesPage = _stream.ReadUInt32();
	}

	public static ArchiveHeader Create(Stream stream)
	{
		stream.WriteWord(Magic);
		stream.WriteUInt32(Version);
		stream.WriteUInt32(0);
		stream.WriteUInt32(0);

		stream.Position -= Stride;

		return new ArchiveHeader(stream);
	}
}
