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
	// September 21, 2022
	// Conversions
	// collection we will look at are IQueryable, IEnumerable and List
	// List<T> - in-memory data
	
	// Display all albms and their track. Display the album title,
	//	artist name, and album tracks. For each track show the song name
	//	and play time. Show only albums with 25 or more tracks.
	
	List<AlbumTracks> albumList = Albums
						.Where( a => a.Tracks.Count >=25 )
						.Select( a => new AlbumTracks
						{
							Title = a.Title,
							Artist = a.Artist.Name,
							Songs = a.Tracks
									.Select( tr => new SongItem										{
										Song = tr.Name,
										Playtime = tr.Milliseconds / 1000
									})

									.ToList()	// List<SongItem>
						})
						.ToList()	// List<AlbumTracks>
						//.Dump()
						;
						
	// Using .FirstOrDefault()
	//			
	// first saw in CPSC1517 when checking to see if a record existed in a BLL service method
	
	// Find the first album by Deep Purple
	var artistParam = "Deep Purples";
	
	var resultsFOD = Albums
						.Where( a => a.Artist.Name.Equals(artistParam) )
						.Select( a => a )
						.OrderBy( a => a.ReleaseYear )
						//.First()	// this will ONLY display one instance
									// if the variable doesnt match any records, it will show error
						.FirstOrDefault()	// this will show NULL if no record is found
						//.Dump()
						;
						
	//if ( resultsFOD != null )
	//{
	//	resultsFOD.Dump();
	//}
	//else
	//{
	//	Console.WriteLine( $"No albums found for artist {artistParam}" );
	//}
	//
	// Distinct()
	// removes duplicate reported lines
	
	// Get a list of customer countries
	var resultsDistinct = Customers
								.OrderBy( c => c.Country)
								.Select(c => c.Country)	// List of country
								.Distinct()
								// .OrderBy() [sort] can be placed after .Distinct as long as we create an anonymous data or strongly typed dataset
								//.Dump()
								;
								
								
	// .Take() and .Skip()
	// In CPSC1517, when you want to take your supplied Paginator:
	//		the query method was to return ONLY the needed
	//		records for the display NOT the entire collection
	//			[a] the query was executed returning a collection of size x
	//			[b] obtained the total cont (x) of return records
	//			[c] calculated the number of records to skip (pagenumber - 1) * pagesize
	//			[d] on the return method statement you used,
	//				return variablename.Skip(rowsSkipped).Take(pagesize).ToList
	
	
	// Union
	// rules in linq are the same as sql
	// result is the same as sql, combine separate collection into one,
	// SYNTAX:	(queryA).Union(queryB).[Union( query ... )]
	// RULES:
	//		number of columns should be the same
	//		column datatypes MUST be the same
	//		ordering should be done as a method after the last Union
	//		hard coded values should go first on your query
	
	var resultsUnionA = ( Albums
								.Where( x => x.Tracks.Count() == 0 )
								.Select( x => new
									{
										title = x.Title,
										totalTracks = 0,
										totalCost = 0.00m,	// add "m" to make datatype decimal
										averageLength = 0.00
									})
						)
													
													
						.Union( Albums
							.Where( x => x.Tracks.Count() > 0 )
							.Select( x => new
							{
								title = x.Title,
								totalTracks = x.Tracks.Count(),
								totalCost = x.Tracks.Sum( tr => tr.UnitPrice ),
								averageLength = x.Tracks.Average( tr => tr.Milliseconds )
							})
							)
							.OrderBy( x => x.totalTracks )
							.Dump()
							;

							
}

public class SongItem
{
	public string Song{ get; set; }
	public double Playtime{ get; set; }	
}

public class AlbumTracks
{
	public string Title{ get; set; }
	public string Artist{ get; set; }	
	public List<SongItem> Songs { get; set;}
}

// You can define other methods, fields, classes and namespaces here