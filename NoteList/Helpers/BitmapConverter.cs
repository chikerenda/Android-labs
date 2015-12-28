using System;
using Android.Content;
using Android.Graphics;
using Java.Nio;
using Uri = Android.Net.Uri;

namespace NoteList
{
	public static class BitmapConverter
	{
		public static byte[] BitmapToByteArray(Bitmap bitmap)
		{
			ByteBuffer byteBuffer = ByteBuffer.Allocate(bitmap.ByteCount);
			bitmap.CopyPixelsToBuffer(byteBuffer);
			byte[] bytes = byteBuffer.ToArray<byte>();
			return bytes;
		}

		public static Bitmap BitmapFromByteArray(byte[] bytes) 
		{
			//TODO
			return null;
		}

		public static Bitmap BitmapFromUri(Uri uri) 
		{
			//var inStream = ContentResolver.OpenInputStream(uri);
			//return Android.Graphics.BitmapFactory.DecodeStream (inStream);
			return null;
		}

		public static Bitmap BitmapFromUriString(String uri) 
		{
			return BitmapFromUri(Uri.Parse(uri));
		}
	}
}

