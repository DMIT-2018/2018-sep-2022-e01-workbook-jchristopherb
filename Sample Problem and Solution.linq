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

//September 12 2022
//List all albums by release label. Any label with no label shold be indicated as Unknown
//List Title, Label and Artist Name
//OrderBy ReleaseLabel

//understand the problem
//	Collection: Albums
//	Selective Data: anonymous data set
//	Label (nullable): either unknown or label name ***
//	order by release label

//design
//	Albums
//	Select (new{})
//	fields: title
//			label  ?? ternary operator (condition(S) ? true value : false value)
//			Artist.Name

//coding and testing
Albums
	//.OrderBy (x => x.ReleaseLabel)
	.Select(x => new
	{
		Title = x.Title,
		Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel,
		Artist = x.Artist.Name
	})
	.OrderBy (x => x.Label)


//	List all albums showing all title, artist name, year and decade of release using
//	oldies, 70s, 80s, 90s, or modern.
// 	Order by decade.

// < 1970
//	oldies
// else
//	( < 1980 then 70's
//		else
//			( < 1990 then 80's
//			else
//				( < 2000 then 90s
//				else
//					( modern))

Albums
	//.OrderBy (x => x.ReleaseLabel)
	.Select(x => new
	{
		Title = x.Title,
		Artist = x.Artist.Name,
		Year = x.ReleaseYear,
		Decade = x.ReleaseYear <= 1970 ? "Oldies" :
				 x.ReleaseYear < 1980 ? "70s" :
				 x.ReleaseYear < 1990 ? "80s" :
				 x.ReleaseYear <  2000 ? "90s" : "Modern"
				 
	})
	.OrderBy (x => x.Year)