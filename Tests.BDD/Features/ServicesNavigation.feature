Feature: Navigate to Services section
  As a visitor of the EPAM website
  I want to navigate to a specific service category from the main navigation
  So that I can review EPAM's expertise in that area

  Background:
    Given I am on the EPAM home page
    And I accept the cookies banner

  Scenario Outline: Navigate to a service category via the Services dropdown
    When I open the "Services" navigation menu
    And I select the "<Category>" service category
    Then the page title should contain "<Category>"
    And the "Our Related Expertise" section should be displayed

    Examples:
      | Category        |
      | Generative AI   |
      | Responsible AI  |
