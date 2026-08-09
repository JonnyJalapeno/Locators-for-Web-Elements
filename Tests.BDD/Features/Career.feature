Feature: Career search
  As a job seeker
  I want to search EPAM careers by keyword, country and workplace type
  So that I can find job openings that match my criteria

  Scenario Outline: Job description contains the searched keyword
    Given I am on the EPAM home page
    And I accept the cookies banner
    When I go to the Careers page
    And I click "Search Careers" on the Careers page
    And I accept the cookies banner on the Careers page
    And I select "<Country>" from the country dropdown
    And I filter jobs by "Remote" workplace type
    And I search careers for the keyword "<Keyword>"
    And I expand the job description
    Then the job description should contain the keyword "<Keyword>"

    Examples:
      | Keyword    | Country |
      | blockchain | Serbia  |
      # | python   | Uzbekistan |
