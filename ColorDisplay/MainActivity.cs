using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ColorDisplay
{
	[Activity (Label = "ColorDisplay", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


			SeekBar redSeekBar = FindViewById<SeekBar> (Resource.Id.redSeekBar);
			SeekBar greenSeekBar = FindViewById<SeekBar> (Resource.Id.greenSeekBar);
			SeekBar blueSeekBar = FindViewById<SeekBar> (Resource.Id.blueSeekBar);
			RelativeLayout backgroundLayout = FindViewById<RelativeLayout> (Resource.Id.backgroundLayout);

			backgroundLayout.SetBackgroundColor(Android.Graphics.Color.Argb(255, redSeekBar.Progress*255/100, greenSeekBar.Progress*255/100, blueSeekBar.Progress*255/100));

			redSeekBar.ProgressChanged += seekBarProgressChanged;
			greenSeekBar.ProgressChanged += seekBarProgressChanged;
			blueSeekBar.ProgressChanged += seekBarProgressChanged;
		}

		private void seekBarProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e) 
		{
			if (e.FromUser)
			{
				SeekBar redSeekBar = FindViewById<SeekBar> (Resource.Id.redSeekBar);
				SeekBar greenSeekBar = FindViewById<SeekBar> (Resource.Id.greenSeekBar);
				SeekBar blueSeekBar = FindViewById<SeekBar> (Resource.Id.blueSeekBar);
				RelativeLayout backgroundLayout = FindViewById<RelativeLayout> (Resource.Id.backgroundLayout);

				backgroundLayout.SetBackgroundColor(Android.Graphics.Color.Argb(255, redSeekBar.Progress*255/100, greenSeekBar.Progress*255/100, blueSeekBar.Progress*255/100));
			}
		}
	}
}


