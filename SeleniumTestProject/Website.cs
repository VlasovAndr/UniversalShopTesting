
namespace SeleniumTestProject
{
    public class Website
    {
        public string Url { get; set; }
        public Pages Pages { get; set; }
    }

    public class Pages
    {
        public StartPage StartPage { get; set; }
        public ProductListPage ProductListPage { get; set; }
        public ProductDetailsPage ProductDetailsPage { get; set; }
        public BasketPage BasketPage { get; set; }
    }

    public class StartPage
    {
        public string SearchField { get; set; }
        public string ItemForSearch { get; set; }
    }

    public class ProductListPage
    {
        public string SelectedItem { get; set; }
    }

    public class ProductDetailsPage
    {
        public string NameOfItem { get; set; }
        public string AddToBasketButton { get; set; }
        public string BasketButton { get; set; }
    }

    public class BasketPage
    {
        public string NameOfItem { get; set; }
    }
}
