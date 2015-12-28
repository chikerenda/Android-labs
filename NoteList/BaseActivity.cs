using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;

namespace NoteList
{		
	public class BaseActivity : Activity
	{
		protected const string ThemeKey = "Theme";
		protected const string FontSizeKey = "FontSize";

		protected Android.Views.View BackgroundView;
		protected List<TextView> TextFields;
		protected ISharedPreferences prefs;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			prefs = Application.Context.GetSharedPreferences("NoteList", FileCreationMode.Private);
		}

		protected string GetCurrentTheme () 
		{
			return prefs.GetString (ThemeKey, "0");
		}

		protected string GetCurrentFontSize () 
		{
			return prefs.GetString (FontSizeKey, "0");
		}

		protected void SetTheme(string theme)
		{
			var editor = prefs.Edit ();
			editor.PutString (ThemeKey, theme);
			editor.Commit ();
		}

		protected void SetFontSize(string fontSize)
		{
			var editor = prefs.Edit ();
			editor.PutString (FontSizeKey, fontSize);
			editor.Commit ();
		}

		protected void ApplyTheme() 
		{
			switch (prefs.GetString (ThemeKey, "0")) 
			{
			case "0": // Dark
				BackgroundView.SetBackgroundColor (Color.Black);
				foreach (var field in TextFields) {
					field.SetTextColor (Color.White);
				}
				break;
			case "1": // Light
				BackgroundView.SetBackgroundColor (Color.Gray);
				foreach (var field in TextFields) {
					field.SetTextColor (Color.Blue);
				}
				break;
			}


			switch (prefs.GetString (FontSizeKey, "0")) 
			{
			case "0": // Small
				foreach (var field in TextFields) {
					field.SetTextSize (new Android.Util.ComplexUnitType(), 16);
				}
				break;
			case "1": // Medium
				foreach (var field in TextFields) {
					field.SetTextSize (new Android.Util.ComplexUnitType(), 24);
				}
				break;
			case "2": // Large
				foreach (var field in TextFields) {
					field.SetTextSize (new Android.Util.ComplexUnitType(), 32);
				}
				break;
			}
		}
	}
}

