using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using NoteList.DAL;
using System.Collections.Generic;
using Android.Content.PM;

namespace NoteList
{
	[Activity (Label = "@string/NoteList", MainLauncher = true, Icon = "@drawable/icon",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : BaseActivity
	{
		public const int ListViewId = 1111;

		int selectedId = 0;

		NoteItemListAdapter noteListAdapter;
		List<NoteItem> notes;
		ListView noteListView;
		ProgressBar homeProgressBar;
		EditText searchTextBox;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.HomeScreen);

			searchTextBox = FindViewById<EditText> (Resource.Id.SearchText);
			noteListView = FindViewById<ListView> (Resource.Id.NoteList);
			homeProgressBar = FindViewById<ProgressBar> (Resource.Id.HomeProgressBar);

			searchTextBox.TextChanged += SearchTextBox_TextChanged;

//			if(noteListView != null) {
//				noteListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
//					var noteDetails = new Intent (this, typeof (NoteEditScreen));
//					noteDetails.PutExtra ("NoteID", notes[e.Position].ID);
//					StartActivity (noteDetails);
//				};
//			}

			RegisterForContextMenu (noteListView);
		}

		void SearchTextBox_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			if (noteListAdapter != null) 
				noteListAdapter.InvokeFilter(searchTextBox.Text);
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			homeProgressBar.Visibility = ViewStates.Gone;
			searchTextBox.Visibility = ViewStates.Gone;
			searchTextBox.Text = string.Empty;
			BackgroundView = FindViewById<LinearLayout> (Resource.Id.HomeBackground);
			TextFields = new List<TextView> ();
			ApplyTheme ();
			Reload ();
		}


		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.ActionBarMenu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.SearchButton:
				if (searchTextBox.Visibility == ViewStates.Gone) {
					searchTextBox.Visibility = ViewStates.Visible;
				} else {
					searchTextBox.Visibility = ViewStates.Gone;
				}
				searchTextBox.Text = string.Empty;
				return true;

			case Resource.Id.PreferencesButton:
				StartActivity (typeof(SettingsScreen));
				return true;

			case Resource.Id.AddButton:
				StartActivity (typeof(NoteEditScreen));
				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}


		public override void OnCreateContextMenu (IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			if (v.Id == Resource.Id.NoteList) 
			{
				var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
				menu.SetHeaderTitle (notes[info.Position].Name);
				selectedId = notes[info.Position].ID;

				menu.Add(0,0,0,"Edit");
				menu.Add(0,1,0,"Delete");
			}
		}

		public override bool OnContextItemSelected (IMenuItem item)
		{
			Intent noteDetails;
			switch (item.ItemId) 
			{
				case 0:
					noteDetails = new Intent (this, typeof (NoteEditScreen));
					noteDetails.PutExtra ("NoteID", selectedId);
					StartActivity (noteDetails);
					return true;
				case 1:
					NoteItemManager.DeleteNote (selectedId);
					Reload ();
					return true;
				default:
					return base.OnContextItemSelected(item);
			}
		}

		async void Reload ()
		{
			noteListAdapter = new NoteItemListAdapter( this, new List<NoteItem>(), GetCurrentTheme (), GetCurrentFontSize ());
			noteListView.Adapter = noteListAdapter;

			homeProgressBar.Visibility = ViewStates.Visible;
			await DownloadNotes();
			homeProgressBar.Visibility = ViewStates.Gone;

			noteListAdapter = new NoteItemListAdapter( this, notes, GetCurrentTheme (), GetCurrentFontSize ());
			noteListView.Adapter = noteListAdapter;
		}

		public async Task DownloadNotes()
		{
			await Task.Delay(2000);
			notes = NoteItemManager.GetNotes();
		}
	}
}


