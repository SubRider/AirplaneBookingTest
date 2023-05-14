using System.Linq.Expressions;
using System.Reflection;

class SearchMenu<T> where T : IHasID
{
    // Properties to store the search state, ID, object list, and search categories

    public List<T> ObjectList { get; set; } = new();


    // Constructor for initializing the Searcher with a list of objects and search categories
    public SearchMenu(List<T> listOfObjects)
    {
        ObjectList = listOfObjects;
    }

    public void Activate(List<(string searchCategory, string criterium)> criteria)
    {
        Renderer.ClearLines();
        List<T> results = Find(criteria);
        Window w1 = Window.Windows[0];
        List<Button> tempButtons = new(Button.Buttons);
        foreach (Button button in tempButtons.Where(b => b is not InputButton)) button.Remove();
        foreach (T item in results)
        {
            _ = new Button(item.ToString(), Button.Buttons.Count, w1, () => { BookingMenu.ResultID = item.ID; BookingMenu.NextMenu(); });
        }
        Renderer.ShowButtons(w1);
    }

    // Method to find and display matching items based on the query and current search category
    public List<T> Find(List<(string searchCategory, string criterium)> criteria)
    {
        List<List<T>> resultLists = new();


        // Iterate through the object list and create buttons for matching items
        foreach ((string searchCategory, string criterium) in criteria)
        {
            List<T> partialResults = new();
            foreach (T item in ObjectList)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(searchCategory);
                object foundObject = propertyInfo.GetValue(item);
                if (foundObject != null)
                {
                    string result = foundObject.ToString();
                    string comparableString = result.ToLower();
                    if (comparableString.StartsWith(criterium.ToLower()))
                    {
                        partialResults.Add(item);
                    }
                }
            }
            resultLists.Add(partialResults);
        }
        return resultLists.Aggregate((common, next) => common.Intersect(next).ToList());
    }
}
