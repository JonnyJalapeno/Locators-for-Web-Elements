Feature: Carousel Article Title Tests
  As a visitor of the EPAM website
  I want the carousel's "Read More" link to open the article for the active slide
  So that the article I land on matches the slide I selected

  Scenario: Article title matches the active carousel slide title after swiping
    Given I am on the EPAM home page
    And I accept the cookies banner
    When I navigate to the Insights page
    And I swipe the carousel <swipeTimes> times
    And I click "Read More" on the active carousel slide
    Then the article title should match the active slide title

    Examples:
      | swipeTimes |
      | 3 |