Feature: Site search
  As a visitor of the EPAM website
  I want to search the site for a keyword
  So that I can find articles related to that keyword

  Scenario Outline: All search results relate to the searched term
    Given I am on the EPAM home page
    And I accept the cookies banner
    When I open the site search
    And I search for "<SearchWord>"
    Then all search results should relate to "<SearchWord>"

    Examples:
      | SearchWord |
      | BLOCKCHAIN |
      # | Cloud       |
      # | Automation  |
