using System.Linq.Expressions;
using System.Reflection;

class Searcher<T> where T : IHasID
{
    // Properties to store the search state, ID, object list, and search categories
    public int ID { get; set; } = 0;
    public bool Searching { get; set; } = true;
    public List<T> ObjectList { get; set; } = new();
    public List<string> SearchCategories { get; set; }
    public string SearchCategory { get; set; }
    public string PrevSearchCategory { get; set; }

    // Constructor for initializing the Searcher with a list of objects and search categories
    public Searcher(List<T> listOfObjects, List<string> searchCategories)
    {
        ObjectList = listOfObjects;
        SearchCategories = searchCategories;
        SearchCategory = SearchCategories[1];
        PrevSearchCategory = "";
    }

    // Method to activate the search process
    public void Activate()
    {
        string searchQuery = "";
        string prevQuery;
        Find("");
        while (Searching)
        {
            prevQuery = searchQuery;
            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Backspace)
            {
                try { searchQuery = searchQuery.Remove(searchQuery.Length - 1); }
                catch { }
            }
            else
            {
                try
                {
                    searchQuery += char.ToLower(Convert.ToChar(key.ToString()));
                }
                catch
                {
                    InputChecker.CheckInput(key);
                }
            }
            if (searchQuery != prevQuery || SearchCategory != PrevSearchCategory) Find(searchQuery);
        }
    }

    // Method to find and display matching items based on the query and current search category
    public void Find(string query)
    {
        Button.Clear();
        Console.Clear();
        Button button;
        int buttonTop = 2;
        int buttonLeft = 0;

        // Create buttons for each search category
        foreach (string searchCategory in SearchCategories)
        {
            string categoryCopy = searchCategory;
            button = new(ConsoleColor.White, searchCategory, 0, buttonLeft,
                () => {
                    PrevSearchCategory = SearchCategory; SearchCategory = categoryCopy;
                    InputChecker.JumpToButton(SearchCategories.Count);
                });
            buttonLeft += searchCategory.Length + 1;
        }

        // Create a button to display the search query
        button = new(ConsoleColor.White, query, 1, () => { });

        // Iterate through the object list and create buttons for matching items
        foreach (T item in ObjectList)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(SearchCategory);
            object foundObject = propertyInfo.GetValue(item);
            if (foundObject != null)
            {
                string result = foundObject.ToString();
                string comparableString = result.ToLower();
                if (comparableString.StartsWith(query))
                {
                    button = new(result, buttonTop, () =>
                    {
                        Quit();
                        ID = item.ID;
                    });
                    buttonTop++;
                }
            }
        }

        Renderer.ShowButtons();

        if (Button.Buttons.Count > 0) InputChecker.JumpToButton(SearchCategories.Count);
        PrevSearchCategory = SearchCategory;
    }

    // Method to quit searching and return the ID of the selected item
    public void Quit()
    {
        Searching = false;
        return;
    }
}
