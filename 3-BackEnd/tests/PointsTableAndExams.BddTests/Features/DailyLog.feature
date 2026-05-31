Feature: Daily Food Log
  As a registered user
  I want to log my daily food consumption
  So that I can track my points intake

  Scenario: Create a daily log for today
    Given I am a registered user
    When I create a daily log for today
    Then the daily log should be created successfully

  Scenario: Cannot create duplicate log for same date
    Given I am a registered user
    And I already have a log for today
    When I try to create another log for today
    Then the operation should fail with "DailyLog.AlreadyExists"

  Scenario: Add a food item to daily log
    Given I am a registered user
    And I have a daily log for today
    When I add a food item with 2 servings of 25 points each
    Then the log total should be 50 points
