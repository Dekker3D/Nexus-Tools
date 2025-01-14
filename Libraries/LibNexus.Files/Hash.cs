﻿using System.Security.Cryptography;

namespace LibNexus.Files;

public readonly struct Hash : IEquatable<Hash>
{
	public const uint Length = 20;

	public byte[] Bytes { get; } = new byte[Length];

	public Hash(byte[] sha1)
	{
		Array.Copy(SHA1.HashData(sha1), Bytes, Length);
	}

	public bool Validate(byte[] data)
	{
		return Bytes.SequenceEqual(SHA1.HashData(data));
	}

	public override bool Equals(object? obj)
	{
		return obj is Hash fileId && Equals(fileId);
	}

	public bool Equals(Hash other)
	{
		return other.Bytes.SequenceEqual(Bytes);
	}

	public override int GetHashCode()
	{
		return Bytes.GetHashCode();
	}

	public static bool operator ==(Hash left, Hash right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Hash left, Hash right)
	{
		return !(left == right);
	}
}
