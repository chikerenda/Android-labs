using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using Uri = Android.Net.Uri;
using Environment = System.Environment;
using System;
using Android.Content.PM;

namespace NoteList
{
	[Activity (Label = "@string/Settings",
		ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]			
	public class SettingsScreen : BaseActivity 
	{
		int selectedTheme;
		int selectedFontSize;

		Spinner themeSpinner;
		Spinner fontSizeSpinner;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.SettingsScreen);

			themeSpinner = FindViewById<Spinner> (Resource.Id.ThemeSpinner);
			fontSizeSpinner = FindViewById<Spinner> (Resource.Id.FontSizeSpinner);



			selectedTheme = Convert.ToInt32 (prefs.GetString (ThemeKey, "0"));
			selectedFontSize = Convert.ToInt32 (prefs.GetString (FontSizeKey, "0"));

			themeSpinner.ItemSelected += ThemeSpinner_ItemSelected;
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.themes, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			themeSpinner.Adapter = adapter;

			fontSizeSpinner.ItemSelected += FontSizeSpinner_ItemSelected;
			adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.font_sizes, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			fontSizeSpinner.Adapter = adapter;

			themeSpinner.SetSelection (selectedTheme);
			fontSizeSpinner.SetSelection (selectedFontSize);
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			BackgroundView = FindViewById<ScrollView> (Resource.Id.SettingsBackground);
			TextFields = new List<TextView> {
				FindViewById<TextView> (Resource.Id.SettingsText1),
				FindViewById<TextView> (Resource.Id.SettingsText2)
			};
			ApplyTheme ();
		}

		void ThemeSpinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			selectedTheme = e.Position;
			SetTheme (selectedTheme.ToString ());
			ApplyTheme ();
		}

		void FontSizeSpinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			selectedFontSize = e.Position;
			SetFontSize (selectedFontSize.ToString ());
			ApplyTheme ();
		}
	}
}

