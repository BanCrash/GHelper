﻿using System;
using System.IO;
using GHelperLogic.IO;
using GHelperLogic.Utility;
using NDepend.Path;
using Optional;
using Optional.Unsafe;
using SixLabors.ImageSharp;

namespace GHelper.Service
{
	public static class GHubImageStorageService
	{
		public static class GHubImageCacheService
		{
			public static Option<IFilePath> SavePosterImage(Image poster)
			{
				string imageFileName = Guid.NewGuid().ToString("N");
				imageFileName += Properties.Resources.FileExtensionPNG;

				try
				{
					IFilePath destinationImageFilePath = GHelperLogic.Properties.Configuration.IconCacheDirectoryPath.GetChildFileWithName(imageFileName);
					using FileStream posterFileStream = new (path: destinationImageFilePath.ToString()!,
					                                         mode: FileMode.Create);
					poster.SaveAsPng(posterFileStream);
					return Option.Some(destinationImageFilePath);
				}
				catch (Exception)
				{
					LogManager.Log("Unabled to save custom poster image to AppData image cache");
					return Option.None<IFilePath>();
				}
			}
		}

		public static class GHubProgramDataImageStorageService
		{
			public static void SavePosterImage(Image poster, string imageFileName)
			{
				try
				{
					GHubProgramDataIO.DefaultApplicationImagesIO.SavePosterImage(image: poster, imageFileName: imageFileName);
				}
				catch (Exception)
				{
					LogManager.Log("Unabled to save custom poster image to ProgramData");
					throw new IOException();
				}
			}
		}

	}
}