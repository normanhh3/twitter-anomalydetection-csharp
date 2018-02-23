Feature: AnomalyDetection
	In order to accurately identify anomalies in time-series data
	As a developer
	I want to find time-series anomalies


@NaN
Scenario: Input data with leading and trailing NaN values
	Given I have an input set of test data
	And the first 10 values are NaN
	And the last value is NaN
	And the maximum anomalies are 0.02
	And the direction is both
	When I request the time-series anomalies for the input data
	Then I expect there to be 131 anomalies

@NaN
Scenario: Input data with NaN values in the middle
	Given I have an input set of test data
	And the middle value in the data is NaN
	And the direction is both
	And the maximum anomalies are 0.02
	When I request the time-series anomalies for the input data
	Then I expect an exception to be thrown

Scenario: Input data with no NaN values
	Given I have an input set of test data
	And the direction is both
	And the maximum anomalies are 0.02
	When I request the time-series anomalies for the input data
	Then I expect there to be 131 anomalies