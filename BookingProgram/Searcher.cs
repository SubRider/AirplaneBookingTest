using System.Linq.Expressions;
using System.Reflection;

class Searcher<T>
{
    public bool Searching { get; private set; } = true;
    public List<T> ObjectList { get; set; } = new();
    public List<string> SearchCategories { get; set; }
    public string SearchCategory { get; set; }
    public string PrevSearchCategory { get; set; }

    public Searcher(List<T> listOfObjects, List<string> searchCategories)
    {
        ObjectList = listOfObjects;
        SearchCategories = searchCategories;
        SearchCategory = SearchCategories[1];
        PrevSearchCategory = "";
    }

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
    public void Find(string query)
    {
        Button.Clear();
        Console.Clear();
        Button button;
        int buttonTop = 2;
        int buttonLeft = 0;
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
        button = new(ConsoleColor.White, query, 1, () => { });
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
                    button = new(result, buttonTop, () => { Searching = false; BookingMenu.ClassReservationMenu(); });
                    buttonTop++;
                }
            }
        }
        Renderer.ShowButtons();
        if (Button.Buttons.Count > 0) InputChecker.JumpToButton(SearchCategories.Count);
        PrevSearchCategory = SearchCategory;
    }
}
