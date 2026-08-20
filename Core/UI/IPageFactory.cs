namespace Locators_for_Web_Elements.Core.UI
{
    public interface IPageFactory
    {
        TPage Create<TPage>() where TPage : notnull;
    }
}
