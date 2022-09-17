<Query Kind="Program">
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

void Main()
{

// Nested Queries
// Sometimes referred to as subqueries
// Simply put: it is a query within a query [ .. ]

// List all sales support employees showing their
//	full name (last, first), title and phone
// FOR EACH EMPLOYEE, show a list of customers they support (nested query)
// Show the customer full name (Last, First), city and State

// employee 1, title, phone
//	customer 2000, city, state
//	customer 2109, city, state
//	customer 5000, city, state

// employee 2, title, phone
//	customer 3000, city, state

// They appears to be separate list that need to be within one final dataset collection
// We need:
// 	List of employees
// 	List of employee customers
// Concern: The list are intermixed!

// C# point of view in a class definition
// First, this is a composite class
// 	The class is describing an employee
// 	Each instance of the employee will have a list of employee customers

// class EmployeeItem
// What do we need:
//		Fullname (property)
//		Title (property)
//		Phone (property)
//		collection of customers (property: List<T>)

// class CustomerItem
// What we need:
//		Fullname (property)
//		City (property)
//		State (property)

	var results = Employees
					.Where ( x => x.Title.Contains("Sales Support") )
					.Select ( x => new EmployeeItem
						{
							FullName = x.LastName + ", " + x.FirstName,
							Title = x.Title,
							Phone = x.Phone,
							CustomerList = x.SupportRepCustomers
											.Select ( c => new CustomerItem 
												{
													FullName = c.LastName + ", " + c.FirstName,
													City = c.City,
													State = c.State
												}
											)
						}
					);
	results.Dump();
	
	// September 16
	// List all albums that are from 1990,
	// Display the album title and artist name.
	// For each album, display it's tracks
	
	var albumTracks = Albums
						.Where ( x => x.ReleaseYear == 1990 )
						.Select ( x => new {
						
						Title = x.Title,
						Artist = x.Artist.Name,	// Navigational property to get the artist name.
												// The Artist is the FK of the Table Albums
						Tracks = x.Tracks
									.Select (y => new {		//	Sub-query
										Song = y.Name,
										Genre = y.Genre.Name	// The collection Tracks has a parent table of Genre (FK)
																// Since we need the genre name, we need to go up
																// It is important to understand the ERD and which data is needed.
									})
						
						});
						
	albumTracks.Dump();
	
	
	// END September 16

}

public class CustomerItem
{
	public string FullName { get; set;}
	public string City { get; set;}
	public string State { get; set;}

}

public class EmployeeItem
{
	public string FullName { get; set;}
	public string Title { get; set;}
	public string Phone { get; set;}
	public IEnumerable<CustomerItem> CustomerList { get; set;}

}
	
	

