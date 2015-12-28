using System;
using System.IO;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Database;
using NoteList.DAL;

using Uri = Android.Net.Uri;
using Environment = System.Environment;

using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Android.Content.PM;

namespace NoteList
{
	[Activity (Label = "@string/EditNote",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]			
	public class NoteEditScreen : BaseActivity 
	{
		const int NotificationId = 999;
		const int PickImageId = 1000;
		const int PickDateId = 1001;
		const int PickTimeId = 1002;

		string selectedImageUri = string.Empty;
		int selectedPriority = 0;
		DateTime selectedDateTime;

		NoteItem note = new NoteItem();
		Button cancelDeleteButton;
		Button saveButton;
		EditText nameTextEdit;
		EditText descriptionTextEdit;
		ImageView imagePreview;
		Button pickImageButton;
		Spinner prioritySpinner;
		TextView dateDisplay;
		Button pickDate;
		TextView timeDisplay;
		Button pickTime;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


			//Find controls
			SetContentView(Resource.Layout.NoteEditScreen);
			nameTextEdit = FindViewById<EditText>(Resource.Id.NameText);
			descriptionTextEdit = FindViewById<EditText>(Resource.Id.NotesText);
			imagePreview = FindViewById<ImageView>(Resource.Id.NoteImage);
			saveButton = FindViewById<Button>(Resource.Id.SaveButton);
			pickImageButton = FindViewById<Button>(Resource.Id.PickImageButton);
			prioritySpinner = FindViewById<Spinner> (Resource.Id.PrioritySpinner);
			cancelDeleteButton = FindViewById<Button>(Resource.Id.CancelDeleteButton);
			dateDisplay = FindViewById<TextView> (Resource.Id.DateDisplay);
			timeDisplay = FindViewById<TextView> (Resource.Id.TimeDisplay);
			pickDate = FindViewById<Button> (Resource.Id.PickDate);
			pickTime = FindViewById<Button> (Resource.Id.PickTime);


			cancelDeleteButton.Text = Resources.GetText (Resource.String.Cancel);
			selectedDateTime = DateTime.Now;
			UpdateDisplay ();

			// Get note if exist
			int noteID = Intent.GetIntExtra("NoteID", 0);
			if(noteID > 0) {
				note = NoteItemManager.GetNote(noteID);

				nameTextEdit.Text = note.Name; 
				descriptionTextEdit.Text = note.Description;
				prioritySpinner.SetSelection(note.Priority);

				cancelDeleteButton.Text = Resources.GetText (Resource.String.Delete);

				selectedDateTime = note.ToDoDate;
				selectedImageUri = note.ImageUri;
				selectedPriority = note.Priority;

				UpdateDisplay ();
			}

			//Image button
			pickImageButton.Click += (sender, eventArgs) => 
			{
				Intent = new Intent();
				Intent.SetType("image/*");
				Intent.SetAction(Intent.ActionGetContent);
				StartActivityForResult(Intent.CreateChooser(Intent, Resources.GetText (Resource.String.SelectPicture)), PickImageId);
			};

			//DateTime
			pickDate.Click += delegate { ShowDialog (PickDateId); };
			pickTime.Click += delegate { ShowDialog (PickTimeId); };

			//Priority spinner
			prioritySpinner.ItemSelected += PrioritySpinner_ItemSelected;
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.priorities, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			prioritySpinner.Adapter = adapter;

			//Buttons clicks 
			cancelDeleteButton.Click += (sender, e) => CancelDelete ();
			saveButton.Click += (sender, e) => Save ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			BackgroundView = FindViewById<ScrollView> (Resource.Id.EditBackground);
			TextFields = new List<TextView> {
				FindViewById<TextView> (Resource.Id.EditText1),
				FindViewById<TextView> (Resource.Id.EditText2),
				FindViewById<TextView> (Resource.Id.EditText3),
				FindViewById<TextView> (Resource.Id.EditText4),
				FindViewById<TextView> (Resource.Id.EditText5),
				FindViewById<TextView> (Resource.Id.DateDisplay),
				FindViewById<TextView> (Resource.Id.TimeDisplay),
			};
			ApplyTheme ();
		}

		[Obsolete]
		protected override Dialog OnCreateDialog (int id)
		{
			switch (id) 
			{
			case PickDateId:
				return new DatePickerDialog (this, OnDateSet, selectedDateTime.Year, selectedDateTime.Month - 1, selectedDateTime.Day); 
			case PickTimeId:
				return new TimePickerDialog (this, OnTimeSet, selectedDateTime.Hour, selectedDateTime.Minute, false); 
			}
			return null;
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
			{
				var uri = data.Data;
				selectedImageUri = uri.ToString ();
				UpdateDisplay ();
			}
		}


		void OnDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			selectedDateTime = new DateTime (e.Date.Year, e.Date.Month, e.Date.Day, selectedDateTime.Hour, selectedDateTime.Minute, selectedDateTime.Second);
			UpdateDisplay ();
		}

		void OnTimeSet (object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			selectedDateTime = new DateTime (selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, e.HourOfDay, e.Minute, selectedDateTime.Second);
			UpdateDisplay ();
		}

		void PrioritySpinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			selectedPriority = e.Position;
		}

		void UpdateDisplay ()
		{
			dateDisplay.Text = selectedDateTime.ToString ("d");
			timeDisplay.Text = selectedDateTime.ToString ("T");
			imagePreview.SetImageURI (Uri.Parse (selectedImageUri));

		}

		void Save()
		{
			note.CreationDate = DateTime.Now;
			note.Name = nameTextEdit.Text;
			note.Description = descriptionTextEdit.Text;
			note.Priority = selectedPriority;
			note.ToDoDate = selectedDateTime;
			note.ImageUri = selectedImageUri;
			NoteItemManager.SaveNote(note);
			CreateNotification ();
			Finish();
		}

		void CancelDelete()
		{
			if (note.ID != 0) {
				NoteItemManager.DeleteNote(note.ID);
			}
			Finish();
		}

		void CreateNotification () 
		{
			Intent resultIntent = new Intent(this, typeof (NoteEditScreen));

			// Pass some values to SecondActivity:
			resultIntent.PutExtra ("NoteID", note.ID);

			// Construct a back stack for cross-task navigation:
			TaskStackBuilder stackBuilder = TaskStackBuilder.Create (this);
			stackBuilder.AddParentStack (Java.Lang.Class.FromType(typeof(NoteEditScreen)));
			stackBuilder.AddNextIntent (resultIntent);

			// Create the PendingIntent with the back stack:            
			PendingIntent resultPendingIntent = 
				stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);

			// Build the notification:
			NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
				.SetAutoCancel (true)                    											// Dismiss from the notif. area when clicked
				.SetContentIntent (resultPendingIntent)  											// Start 2nd activity when the intent is clicked.
				.SetWhen ((long)(note.ToDoDate.ToUniversalTime() - new DateTime
					(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds)						// When to display
				.SetContentTitle (Resources.GetText (Resource.String.NotificationHeader))      		// Set its title
				.SetContentText (note.Name); 														// The message to display.

			// Set icon
			switch (note.Priority)
			{
			case 0:
				builder.SetSmallIcon (Resource.Drawable.Minor);
				break;
			case 1:
				builder.SetSmallIcon (Resource.Drawable.Major);
				break;
			case 2:
				builder.SetSmallIcon (Resource.Drawable.Critical);
				break;
			}

			// Finally, publish the notification:
			NotificationManager notificationManager = 
				(NotificationManager)GetSystemService(Context.NotificationService);
			notificationManager.Notify(NotificationId, builder.Build());
		}



//		public static byte[] ReadToEnd(Stream stream)
//		{
//			long originalPosition = 0;
//
//			if(stream.CanSeek)
//			{
//				originalPosition = stream.Position;
//				stream.Position = 0;
//			}
//
//			try
//			{
//				byte[] readBuffer = new byte[4096];
//
//				int totalBytesRead = 0;
//				int bytesRead;
//
//				while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
//				{
//					totalBytesRead += bytesRead;
//
//					if (totalBytesRead == readBuffer.Length)
//					{
//						int nextByte = stream.ReadByte();
//						if (nextByte != -1)
//						{
//							byte[] temp = new byte[readBuffer.Length * 2];
//							Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
//							Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
//							readBuffer = temp;
//							totalBytesRead++;
//						}
//					}
//				}
//
//				byte[] buffer = readBuffer;
//				if (readBuffer.Length != totalBytesRead)
//				{
//					buffer = new byte[totalBytesRead];
//					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
//				}
//				return buffer;
//			}
//			finally
//			{
//				if(stream.CanSeek)
//				{
//					stream.Position = originalPosition; 
//				}
//			}
//		}
//
//		Android.Graphics.Bitmap GetBitmapFromBytes (byte[] bytes)
//		{
//			Android.Graphics.Bitmap imageBitmap = null;
//
//			using (var inStream = new MemoryStream(bytes))
//			{
//				imageBitmap = Android.Graphics.BitmapFactory.DecodeStream (inStream);
//			}
//
//			return imageBitmap;
//		}
//
//		string GetPathToImage(Uri uri)
//		{
//			string path = null;
//			// The projection contains the columns we want to return in our query.
//			string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
//			using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
//			{
//				if (cursor != null)
//				{
//					int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
//					cursor.MoveToFirst();
//					path = cursor.GetString(columnIndex);
//				}
//			}
//			return path;
//		}
//
//
//		void SaveImageToLocalstorage(string uri) 
//		{
//			var bytes = new byte[22];//Get bytes somehow
//			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//			string localFilename = note.ID + ".png";
//			string localPath = Path.Combine (documentsPath, localFilename);
//			File.WriteAllBytes (localPath, bytes); // writes to local storage
//		}
	}
}

