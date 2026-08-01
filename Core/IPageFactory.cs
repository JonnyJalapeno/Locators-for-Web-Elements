namespace Locators_for_Web_Elements.Core
{
    public interface IPageFactory
    {
        TPage Create<TPage>() where TPage : notnull;
    }
}
