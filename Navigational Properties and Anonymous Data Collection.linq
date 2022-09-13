<Query Kind="Expression">
  <Connection>
    <ID>47b2390c-f25a-47bb-8b1f-34c56494584b</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>WB320-17\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

// September 12, 2022
// Adding SQL connection to Linqpad
// Anonymous Data Types

//Using Navigational Properties and anonymous data set (collection)
//reference: Student Notes/Demo/eRestaurant/Linq: Query and Method Syntax

//	(1)
//Fnd all albums released in the 90s (1990 - 1999)
//Order the albums by ascending year and then alphabetically by album title
//Display the Year, Title, Artist Name and Release abel

// concerns: 	a) not all properies of Album are to be displayed
//				b) the order of the properties are to be displayed in a different sequence
//					then the definition of the properties of the Album
//				c) the artist name is not on the Album table BUT is on the Artist table

//SOLUTION: use an anonymous data collection

//the anonymous data instance is defined within the Select by declared fields  (properties)
//the order of the fields on the new defined instance will be done in specifying the properties of the
//	anonymous data collection

Albums
	//Filter
	.Where (x => x.ReleaseYear > 1989 && x.ReleaseYear < 2000)
	//Sort here with .ReleaseYear
	//.OrderBy (x => x.ReleaseYear)
	//.ThenBy (x => x.Title)
	.Select (x => new
		{
			Year = x.ReleaseYear,
			Title = x.Title,
			Artist = x.Artist.Name,
			Label = x.ReleaseLabel	
		})
	//Or sort here with .Year snce .ReleaseYear doesnt exist yet
		.OrderBy (x => x.Year)	// Year is in the anonymous data type definition, ReleaseYear is NOT
		.ThenBy (x => x.Title)
