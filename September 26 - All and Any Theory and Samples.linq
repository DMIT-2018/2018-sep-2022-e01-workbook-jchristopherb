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

// September 26th
// Any and All
//		These filter tests return a true or false condition
//		They work at the complete collection level

//Genres.Count().Dump();
// 25

// show genres that have tracks which are not on any playlist
Genres
	.Where( g => g.Tracks.Any( tr => tr.PlaylistTracks.Count() == 0 ))
	.Select( g => g)
	//this will show 17 records
	//.Dump()
	;
	
//show genres that have all their tracks appearing at least once on a playlust
Genres
	.Where( g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
	.Select( g => g)
	//this will show 8 items
	//.Dump()
	;
	
// there maybe times that using a !Any() -> All(!relationship)
//		and !All -> Any(!relationship)

// Using All and Any in comparing 2 collections
// If your collection is NOT a complex record, there is a Linq method
//		called .Except that can be used to solve your query
//		Resources: https://dotnettutorials.net/lesson/linq-except-method/

// Compare the track collection of two people using All and Any
// Roberto Almeida and Michelle Brooks
var almeida = PlaylistTracks
					.Where( x => x.Playlist.UserName.Contains("AlmeidaR"))
					.Select( x => new
					{
						song = x.Track.Name,
						genre = x.Track.Genre.Name,
						id = x.TrackId,
						artist = x.Track.Album.Artist.Name
					})
					.Distinct()
					.OrderBy( x => x.song)
					//.Dump()  // 110 songs
					;
					
var brooks = PlaylistTracks
					.Where( x => x.Playlist.UserName.Contains("BrooksM"))
					.Select( x => new
					{
						song = x.Track.Name,
						genre = x.Track.Genre.Name,
						id = x.TrackId,
						artist = x.Track.Album.Artist.Name
					})
					.Distinct()
					.OrderBy( x => x.song)
					//.Dump()  // 88 songs
					;

// We need the List of tracks that BOTH Roberto and Michelle like.
// We need to compare 2 datasets together, data in list A that is also in B
// Lets assume listA is Roberto and lsitB is Michelle
// ListA is what you wish to report from
// ListB is what you wish to report to

// What songs does Roberto like but not Michelle
var c1 = almeida
			.Where(rob => !brooks.Any(mich => mich.id == rob.id))
			.OrderBy(rob => rob.song)
			//.Dump() // there are 109 songs that are not on Michelle's list
			;
			
var c2 = almeida
			.Where(rob => brooks.All(mich => mich.id != rob.id))
			.OrderBy(rob => rob.song)
			//.Dump() // there are 109 songs that are not on Michelle's list
			;
			
var c3 = brooks
			.Where(mich => almeida.All(rob => rob.id != mich.id))
			.OrderBy(mich => mich.song)
			.Dump()		// returns 87 songs except the Eclipse song
			;

// What songs does BOTH michelle and roberto like
var c4 = brooks
			.Where(mich => almeida.Any(rob => rob.id == mich.id))
			.OrderBy(mich => mich.song)
			//.Dump()	// returns 1 record which is Eclipse
			;