using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NCalc;
using Android.OS;

namespace Calculator
{
	[Activity (Label = "Calculator", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			var resultTextBox = FindViewById<TextView> (Resource.Id.mainTextView);


			var numberButtons = new Dictionary<int, Button> ();
			numberButtons.Add (0, FindViewById<Button> (Resource.Id.button0));
			numberButtons.Add (1, FindViewById<Button> (Resource.Id.button1));
			numberButtons.Add (2, FindViewById<Button> (Resource.Id.button2));
			numberButtons.Add (3, FindViewById<Button> (Resource.Id.button3));
			numberButtons.Add (4, FindViewById<Button> (Resource.Id.button4));
			numberButtons.Add (5, FindViewById<Button> (Resource.Id.button5));
			numberButtons.Add (6, FindViewById<Button> (Resource.Id.button6));
			numberButtons.Add (7, FindViewById<Button> (Resource.Id.button7));
			numberButtons.Add (8, FindViewById<Button> (Resource.Id.button8));
			numberButtons.Add (9, FindViewById<Button> (Resource.Id.button9));

			foreach (var button in numberButtons) 
			{
				button.Value.Click += delegate {
					if (resultTextBox.Text.Length <= 20)
						resultTextBox.Text += button.Key.ToString ();
				};
			}
			FindViewById<Button> (Resource.Id.buttoPlus).Click += delegate {
				if (resultTextBox.Text.Length <= 20)
					resultTextBox.Text += "+";
			};
			FindViewById<Button> (Resource.Id.buttonMinus).Click += delegate {
				if (resultTextBox.Text.Length <= 20)
					resultTextBox.Text += "-";
			};
			FindViewById<Button> (Resource.Id.buttonDivide).Click += delegate {
				if (resultTextBox.Text.Length <= 20)
					resultTextBox.Text += "/";
			};
			FindViewById<Button> (Resource.Id.buttonMult).Click += delegate {
				if (resultTextBox.Text.Length <= 20)
					resultTextBox.Text += "*";
			};
			FindViewById<Button> (Resource.Id.buttonClear).Click += delegate {
				resultTextBox.Text = "";
			};
			FindViewById<Button> (Resource.Id.buttonC).Click += delegate {
				if (resultTextBox.Text != "") {
					resultTextBox.Text = resultTextBox.Text.Substring(0, resultTextBox.Text.Length - 1);
				}
			};
			FindViewById<Button> (Resource.Id.buttonEqual).Click += delegate {
				try {
					resultTextBox.Text = new Expression(resultTextBox.Text).Evaluate().ToString();
				} catch {
					resultTextBox.Text = "Err";
				}
			};
		}
	}
}


