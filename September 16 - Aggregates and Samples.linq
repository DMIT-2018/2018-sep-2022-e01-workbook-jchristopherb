<Query Kind="Expression">
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

// September 16 2022

/*

Aggregates
	- can be done on ANY collection
	- IMPORTANT!!!! work ONLY on a collection of values
	- DO NOT work on single instance (non declared collection)
	
	 .Count()				- counts the number of instances in the collection
	 .Sum( x => ... )		- sums (totals) a numeric field (numeric expression) in a collection
	 .Min( x => ... )		- finds the minimun value of collection for a field
	 .Max( x => ... )		- finds the maximum value of collection for a field
	 .Average( x => ... )	- finds the average balue of a nummeric field (numeric expression) in a collection
	 
	 
IMPORTANT!!!!!!
	.Sum() , .Min(), .Max(), and .Average() MUST on at least one record in their collection
	.Sum() and .Average() MUST work on numeric fields and the field CANNOT be null

*/

// Method Syntax
//		collection.aggregate( x => expression )
//		collection.Select( ... ).aggregate
//		collectionset.Count()	- .Count() does not contain an expression

// for Sum, Min, Max and Average: the result is a SINGLE VALUE

// you can use multiple aggregates on a single column
//	.Sum( x => expression ).Min( x => expression)

// [1]	Find the average playing time (length) of tracks in our music collection
// 		thought process:
//			average is an aggregate
//			what is the collection? The tracks table is a collection.
//			What is the expression? The length of play is in Milliseconds

Tracks.Average( x => x.Milliseconds )	// each x has multiple fields
//	OR
Tracks.Select( x => x.Milliseconds ).Average()	// a single list of numbers

// [2]	List all albums of the 60s showing the title asrtist and various aggregates for albums containing tracks
// 		For each album, show the number of tracks, the total price of all tracks and the average playing length of the album tracks
//		thought process:
//			Start at Albums
//			can i get the artist name? (x.Artist.Name)
//			can i get a collection of tracks for an album? (x.Tracks)
//			can i get the number of tracks in the collection? (.Count())
//			can i get the total price of the tracks? (.Sum())
//			get the average of the play length (.Average())

Albums
	.Where( x => x.Tracks.Count() > 0
			&& (x.ReleaseYear > 1959
			&& x.ReleaseYear < 1970 ) )	// Filter: at least an album has one track
										// Aggregate inside a filter is OKAY
										// This will return all albums with at least 1 track
	.Select( x => new
		{
			Title = x.Title,
			Artist = x.Artist.Name,
			NumberOfTracks = x.Tracks.Count(),
			TotalPrice = x.Tracks.Sum( tr => tr.UnitPrice ),
			AverageTrackLength = x.Tracks.Select( tr => tr.Milliseconds ).Average()
		})
	
