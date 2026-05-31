Feature: User Registration
  As a new user
  I want to register an account
  So that I can track my daily food points and exams

  Scenario: Successful user registration
    Given I have valid registration data
    When I register with email "newuser@test.com" and username "newuser"
    Then the registration should succeed
    And a new user ID should be returned

  Scenario: Registration fails with duplicate email
    Given a user already exists with email "existing@test.com"
    When I try to register with the same email "existing@test.com"
    Then the registration should fail
    And the error code should be "User.EmailTaken"

  Scenario: Registration fails with invalid password
    Given I have valid registration data
    When I register with a weak password "123"
    Then a validation error should occur
