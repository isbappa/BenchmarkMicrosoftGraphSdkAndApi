# Project Description

This repository contains code for a comparative analysis of two methods to fetch group data from Azure Active Directory (AAD): using the Microsoft Graph SDK and direct REST API calls.


## Objective

The primary objective is to evaluate and compare the performance of Microsoft Graph SDK and Microsoft REST API in terms of execution time and efficiency when dealing with large datasets, specifically 10,000 groups in AAD.


## Methods

**Microsoft Graph SDK:** Utilizes the GraphServiceClient to fetch groups with paging.
**Microsoft REST API:** Directly calls the Microsoft Graph REST API using HttpClient.

## Benchmarking

The benchmarking is conducted using BenchmarkDotNet. Two methods are evaluated:

`GetGroupsUsingMicrosoftSdkAsyncBenchmark`
`GetGroupsUsingMicrosoftRestApiAsyncBenchmark`

## Prerequisites
- .NET SDK 8.0 or higher
- An Azure AD tenant with groups for testing

## Usage

To run the benchmarks:

1. Clone the project repository.
2. Open the project in your preferred code editor.
3. Replace the following placeholders with your own values:
   - `YOUR TENANT ID HERE`: Replace with your Azure Active Directory tenant ID.
   - `YOUR CLIENT ID HERE`: Replace with your client ID.
   - `YOUR CLIENT SECRET HERE`: Replace with your client secret.																
4. Exucute the following commands in the terminal:
   - `dotnet restore`
   - `dotnet build`
   - `dotnet run -c Release`


## Contributing

Contributions to this project are welcome. Please open an issue or submit a pull request for any enhancements, bug fixes, or suggestions.