namespace Locators_for_Web_Elements
{
    public interface IPageFactory
    {
        TPage Create<TPage>() where TPage : notnull;
    }
}
