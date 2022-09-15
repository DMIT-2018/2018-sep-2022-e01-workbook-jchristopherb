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

	// pretend that the Main() is the webpage
	
	
	
	//find songs by partial song name.
	//display the album title, song, and artist name.
	//oder by song.
	
	/*
	var songCollection = Tracks
							.Where( x => x.Name.Contains("dance"))
							.OrderBy(x => x.Name)
							.Select(x => new SongList
								{
									Album = x.Album.Title,
									Song = x.Name,
									Artist = x.Album.Artist.Name
								}
							);
		songCollection.Dump();
	*/		
	
	// assume a calue was entered into the webpage
	// assume that a post button was pressed
	//assume Main() is OnPost event
	string inputvalue = "dance";
	List<SongList> songCollection = SongsByPartialName(inputvalue);
	songCollection.Dump();	//assume is the webpage display
}

// You can define other methods, fields, classes and namespaces here

//C# really enjoys strongly typed data fields
//	whether these fields are primitive data types (int, double,...)
//	or developer defined datatypes (class)

public class SongList
{
	//use auto implemented property
	public string Album{ get; set; }
	public string Song{ get; set; }
	public string Artist{ get; set; }
}

// imagine the following method exist in a service in your BLL
// this method receives the web page parameter value for the query
// thi method will need to return a collection

// <SongList> - datatype
// SongsByPartialName - method name
// (string partialSongName) - parameter

// if VAR isnt recognized, use IEnumerable<datatype>

List<SongList> SongsByPartialName (string partialSongName)
{
	var songCollection = Tracks
								.Where( x => x.Name.Contains(partialSongName))
								.OrderBy(x => x.Name)
								.Select(x => new SongList
									{
										Album = x.Album.Title,
										Song = x.Name,
										Artist = x.Album.Artist.Name
									}
								);
	return songCollection.ToList();
}