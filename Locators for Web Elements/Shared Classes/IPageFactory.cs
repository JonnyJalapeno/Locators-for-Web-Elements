using System;
using System.Collections.Generic;
using System.Text;
using EpamTests.PageObjects.Components;

namespace Locators_for_Web_Elements.Shared_Classes
{
    public interface IPageFactory
    {
        HomeCarousel CreateInsightsPage();
        //CareersPage CreateCareersPage();
        //AboutPage CreateAboutPage();
        // one method per page HomePage can navigate to
    }
}
