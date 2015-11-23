using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace NoteList
{
	/// <summary>
	/// View/edit a note
	/// </summary>
	[Activity (Label = "noteDetailsScreen")]			
	public class NoteItemScreen : Activity 
	{
		NoteItem note = new NoteItem();
		Button cancelDeleteButton;
		EditText notesTextEdit;
		EditText nameTextEdit;
		Button saveButton;
		CheckBox doneCheckbox;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			int noteID = Intent.GetIntExtra("noteID", 0);
			if(noteID > 0) {
				note = NoteItemManager.GetNote(noteID);
			}

			// set our layout to be the home screen
			SetContentView(Resource.Layout.NoteDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.NameText);
			notesTextEdit = FindViewById<EditText>(Resource.Id.NotesText);
			saveButton = FindViewById<Button>(Resource.Id.SaveButton);

			// TODO: find the Checkbox control and set the value
			//doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			//doneCheckbox.Checked = note.Done;

			// find all our controls
			cancelDeleteButton = FindViewById<Button>(Resource.Id.CancelDeleteButton);

			// set the cancel delete based on whether or not it's an existing note
			cancelDeleteButton.Text = (note.ID == 0 ? "Cancel" : "Delete");

			nameTextEdit.Text = note.Name; 
			notesTextEdit.Text = note.Description;

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

		void Save()
		{
			note.Name = nameTextEdit.Text;
			note.Description = notesTextEdit.Text;
			//TODO: 
			//note.Done = doneCheckbox.Checked;

			NoteItemManager.SaveNote(note);
			Finish();
		}

		void CancelDelete()
		{
			if (note.ID != 0) {
				NoteItemManager.DeleteNote(note.ID);
			}
			Finish();
		}
	}
}

