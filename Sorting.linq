<Query Kind="Expression">
  <Connection>
    <ID>54bf9502-9daf-4093-88e8-7177c12aaaaa</ID>
    <NamingService>2</NamingService>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\ChinookDemoDb.sqlite</AttachFileName>
    <DisplayName>Demo database (SQLite)</DisplayName>
    <DriverData>
      <PreserveNumeric1>true</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.Sqlite</EFProvider>
      <MapSQLiteDateTimes>true</MapSQLiteDateTimes>
      <MapSQLiteBooleans>true</MapSQLiteBooleans>
    </DriverData>
  </Connection>
</Query>

//Sorting - September 9, 2022

//There is a significant difference between query syntax
//	and method syntax

//Query Syntax is much like sql
//	orderby field  {[ascending] | descending} [,field ...]

//	ascending is the default option

//Method syntax us a serues of individual methods
//	.OrderBy( x => x.field)	-	first field ONLY
//	.OrderByDescending(x => x.field) - first field ONLY
//	.ThenBy(x => x.field) - each following field
//	.ThenByDescending(x => x.field)	- each following field

//Example: Find all of the album tracks for the band Queen. Order the track
//	by the track name alphabetically.

// Solution 1: Query Syntax Method
from x in Tracks
where x.Album.Artist.Name.Contains("Queen")
orderby x.AlbumId, x.Name ascending
select x

// Solution 2: λ Method Syntax
Tracks
	.Where (x => x.Album.Artist.Name.Contains("Queen"))
	.OrderBy (x => x.Album.Title)
	.ThenBy (x => x.Name)
	
// Sorting DOES NOT matter what the order of the functions. They are just different process
//	but the same result. BUT there are limits i.e. Filters (ThenBy) can NOT be AFTER the (.Where)
//Order of sorting and filter can be interchanged

Tracks
	.OrderBy (x => x.Album.Title)
	.ThenBy (x => x.Name)
	.Where (x => x.Album.Artist.Name.Contains("Queen"))
