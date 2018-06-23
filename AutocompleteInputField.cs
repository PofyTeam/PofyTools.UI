using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PofyTools.Data;

public class AutocompleteInputField : MonoBehaviour
{
    public InputField inputField;
    public Dropdown suggestions;

    public IContentProvider<List<string>> contentProvider;
    private List<string> _content = null;
    private List<string> _filteredContent = new List<string> ();

    public void SetProvider (IContentProvider<List<string>> provider)
    {
        this.contentProvider = provider;
        // this._content = this.contentProvider.GetContent ();
    }

    private void Awake ()
    {
        //Input field
        this.inputField.onValueChanged.AddListener (this.OnInputValueChange);

        //Dropdown
        this.suggestions.onValueChanged.AddListener (this.OnSuggestionSelected);
    }

    void OnInputValueChange (string value)
    {
        value = value.Trim ().ToLower ();
        if (!string.IsNullOrEmpty (value))
        {
            this._content = this.contentProvider.GetContent ();
            this._filteredContent.Clear ();
            this.suggestions.ClearOptions ();

            foreach (var element in this._content)
            {
                if (element.Contains (value))
                {
                    this._filteredContent.Add (element);
                }
            }

            if (this._filteredContent.Count > 0)
            {
                this.suggestions.AddOptions (this._filteredContent);
                this.suggestions.Show ();
                this.inputField.Select ();
                this.inputField.MoveTextEnd (false);
            }
            else
            {
                this.suggestions.Hide ();
                this.inputField.Select ();
                this.inputField.MoveTextEnd (false);
            }
        }
        else
        {
            this.suggestions.Hide ();
            this.inputField.Select ();
            this.inputField.MoveTextEnd (false);
        }
    }

    void OnSuggestionSelected (int index)
    {
        string value = this.suggestions.options[index].text;
        this.inputField.text = value;
    }
}

//public interface IContentProvider<T>
//{
//    T GetContent ();
//}