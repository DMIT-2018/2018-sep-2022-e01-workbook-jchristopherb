<Query Kind="Statements">
  <Connection>
    <ID>3f59d92f-dadf-49df-93a4-c406402b2079</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>WC320-14\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

// September 23, 2022
// (Issue#5 related sample)
// Problem:	One needs to have processed information from a collection to use
//			against the same collection

// Solution to this type of problem is to use multiple queries
// The early query(ies) will produced the need information/criteria
//		to execute against the same collection in later query(ies)
// Basically we need to do some pre-processing


// Query one will generate data/information that will be used in the next query (query two)

// Display the employees that have the most customers to support.
// Display the employee name and number of customers that employee supports.

// What is not wanted is a list of all employees sorted by number of customers supported.

// One could create a list of all employees, with the customer support count, ordered DESC
//		by support count. BUT, this is NOT what is requested.

// What information do i need?
//	[a] I need to know the maximum number of customers that any particular employee is supporting.
//	[b] I need to take that piece of data and compare to all employees.

//	[1] Get a list of employees and the count of the customers each supports
//	[2] From that list I cna obtain the largest number
//	[3] using the number, review all the employees and their counts, reporting only the busiest employees


var PreprocessEmployeeList = Employees
								.Select( x => new
								{
									Name = x.FirstName + " " + x.LastName,
									CustomerCount = x.SupportRepCustomers.Count()
								})
								//.Dump()
								;
								
//var highCount = PreprocessEmployeeList
//					.Max(x => x.CustomerCount)
//					//.Dump()
//					;
//					
//var BusyEmployees = PreprocessEmployeeList
//						.Where( x => x.CustomerCount == highCount )
//						.Dump()
//						;
						
						
						
var BusyEmployees = PreprocessEmployeeList
						.Where( x => x.CustomerCount == PreprocessEmployeeList.Max(x => x.CustomerCount) )
						.Dump()
						;