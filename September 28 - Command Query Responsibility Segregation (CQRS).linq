<Query Kind="Statements" />

// September 28
// Command Query Responsibility Segregation ( CQRS )

// Using Playlist Management Screenshot from Moodle

// Query
// This is the only  query that needs on te VIEW model
public class SongItem{
	public string song { get; set; }
	public string album { get; set; }
	public string artist { get; set; }
	public double length { get; set; }
	public decimal price { get; set; }
	public int trackId { get; set; } 	// id is hidden on the plus button
}

// Second Query Class
public class PlaylistItem{
	public int trackNumber { get; set; }
	public string song { get; set; }
	public double length { get; set; }
	public int trackId { get; set; } 	// id is hidden on the checkbox as I/O
}


// Command
// AddTrack -> Pass in Playlist name and TrackId
// 			-> No class needed
//public void AddTrack(int playlsitID, ....)

// RemoveTrack -> we need Playlist name, List<int>
// 			   -> No class needed

// MoveTrack -> we need PlaylistName, and a command class
public class ReorgTrackItem{
	public int trackid { get; set; }
	public int currentTrackNumber { get; set; }
	public int reorgTrackNumber { get; set; }
}