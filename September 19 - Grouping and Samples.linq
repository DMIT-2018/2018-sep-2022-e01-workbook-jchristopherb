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

// September 19
// Grouping

// When looking for collections that isnt built-in and it doesnt have a similar FK on each collection
// When you create a group, it builds two (2) components:
//	[a] Key component (deciding criteria value(s)) defining the group
//			referenced this component using the groupname.Key[.propertyName]
//				1 value for key: groupname.Key
//				n values for key: groupname.Key[propertyName]
// (property < - > field < - > attribute < - > value )
//	[b] data of the group (raw instances of the collection)

//	Ways to group:
// [a] by a single column (field,attribute, property)	groupname.Key
// [b] by a set of columns (anonymous data set)			groupname.Key.property
// [c] by use an entity (entity name / nav property)	groupname.Key.property

// concept processing:
// we start with a "pile" of data (original collection prior to grouping)
// specify the grouping property(ies)
// result of the group operation will be to be "place the data into smaller piles"
//	 the piles are dependant on the grouping property(ies) value(s)
//	 the grouping property(ies) become the key
//	 the individual instances are the data in the smaller piles
//	 the entire indivdual instances of the original collections is place in the smaller pile
//	 manipulate each of the "smaller piles" using your linq commands

// grouping is different than ordering
// Ordering is the final re-sequencing of a collection for a display (last thing to be done)
// Grouping re-organizes a collection into separate, usually smaller collection
//		for further processing (ie aggregates)

// Grouping is an excellent way to organize your data especially if you need
//		to process data on a property that is NOT a relative key such as a
//		foreign key which forms a "natural" group using the navigational properties


// SAMPLE:
// Display albums by ReleaseYear
//		this request DOES NOT need grouping
//		this request is an ordering of output : OrderBy
//		this ordering affect only display
Albums
	.OrderBy( x => x.ReleaseYear )

// Display albums by ReleaseYear
//		explicit request to breajup the display into desired "piles"
Albums
	.GroupBy( x => x.ReleaseYear )
	
// Processing on the groups created by the Group command
// Display the number of albums produced each year
Albums
	.GroupBy( x => x.ReleaseYear )
	.Where( egP => egP.Count() > 10 )		//filtering against each group pile
	.Select( eachgroupPile => new
	{
		Year = eachgroupPile.Key,
		NumberofAlbums = eachgroupPile.Count()
	})
	//.Where( x => x.NumberofAlbums > 10 )	//filtering against the output of the .Select() command
	
// Use a multiple set of properties to form the group
// Include a nested query to report on a small pile group

// Display albums grouped by ReleaseLAbel, ReleaseYear. 
// Display the ReleaseYear and number of albums. List only the years with
//		10 or more albums released. FOr each album display the title, artist, number of tracks on the album and release year

Albums
	.GroupBy( x => new {x.ReleaseLabel, x.ReleaseYear} )
	.Where( egP => egP.Count() > 2 )		//filtering against each group pile
	.Select( eachgroupPile => new
	{
		Label = eachgroupPile.Key.ReleaseLabel,
		Year = eachgroupPile.Key.ReleaseYear,
		NumberofAlbums = eachgroupPile.Count(),
		AlbumItems = eachgroupPile
						.Select( egPInstance => new
						{
							title = egPInstance.Title,
							artist = egPInstance.Artist.Name,
							trackCount = egPInstance.Tracks.Count(),
							YearOfAlbumm = egPInstance.ReleaseYear
						})
	})
	
	
	
	
	
	
	