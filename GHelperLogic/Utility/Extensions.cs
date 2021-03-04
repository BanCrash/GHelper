﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using NDepend.Path;
using Newtonsoft.Json;
using System.Linq;
using Optional;

namespace GHelperLogic.Utility
{
	public static class Extensions
	{
		public static (MemoryStream, MemoryStream) Duplicate(this Stream stream)
		{
			var firstCopy = new MemoryStream();
			var secondCopy = new MemoryStream();
			
			stream.CopyTo(firstCopy);
			stream.Position = 0;
			stream.CopyTo(secondCopy);

			firstCopy.Position = 0;
			secondCopy.Position = 0;
			
			return (firstCopy, secondCopy);
		}

		public static void CreateContainingDirectoryIfNeeded(this IFilePath filePath)
		{
			var file = new FileInfo(filePath.ToString()!);
			file.Directory?.Create();
		}

		public static Collection<JsonConverter<StoredType>> Store<StoredType>(this JsonConverterCollection jsonConverters)
		{
			Collection<JsonConverter<StoredType>> stored = new();
			
			for (int i = jsonConverters.Count - 1; i >= 0; i--)
			{
				JsonConverter converter = jsonConverters.ElementAt(i);
				if (converter is JsonConverter<StoredType> storedTypedConverter)
				{
					jsonConverters.RemoveAt(i);
					stored.Add(storedTypedConverter);
				}
			}

			return stored;
		}

		public static void Replace<StoredType>(this JsonConverterCollection jsonConverters, Collection<JsonConverter<StoredType>> storedConverters)
		{
			foreach (JsonConverter<StoredType> converter in storedConverters)
			{
				jsonConverters.Add(converter);
			}
		}

		public static Option<T> ToOption<T>(T? value)
		{
			if (value is null)
			{
				return Option.None<T>();
			}
			else
			{
				return Option.Some<T>(value);
			}
		}
		
		public static string DuplicateWithoutWhitespaces(this string source)
		{
			string duplicate = new (source);
			return string.IsNullOrEmpty(duplicate) ? duplicate : new string(duplicate.Where(x => !char.IsWhiteSpace(x)).ToArray());
		}
	}
}