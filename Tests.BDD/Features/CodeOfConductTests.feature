Feature: Code Of Conduct Tests
  As a visitor of the EPAM website
  I want to download the Code of Ethical Conduct document
  So that I can review EPAM's ethics and compliance policy

  Scenario: Downloading the Code of Ethical Conduct PDF
    Given I am on the EPAM home page
    And I accept the cookies banner
    When I click the "Ethical Conduct" link
    Then the file "Code-Of-Conduct_01_26.pdf" should be downloaded
