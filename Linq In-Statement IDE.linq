<Query Kind="Statements">
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

// Creating Query using C# Statements (Sept 9, 2022)

//The Statement IDE
//this environment expects the use of c# statement grammar
//the results of a querry is NOT automatically displayed as
//	is the Expression environment
//to display the results you need to .Dump() the variable
//	holding the result
//IMPORTANT!!! .Dump() is a Linqpad mthod. It is NOT a C# method.
//Within the Statement environment, one can run ALL the queries
//	in one execution


/*

QUERY SYNTAX @ C# Expression

from arowoncollection in Albums
select arowoncollection

*/

var qsyntaxlist = from arowoncollection in Albums
					select arowoncollection;
					
//dump after the query syntax
qsyntaxlist.Dump();


/*

METHOD SYNTAX @ C# Expression

Albums
   .Select (arowoncollection => arowoncollection)

*/

var msyntaxlist = Albums
   				.Select (arowoncollection => arowoncollection)
				.Dump();
					
//dump after the method syntax
//msyntaxlist.Dump();

var QueenAlbums = Albums
	.Where(a => a.Artist.Name.Contains("Queen"))
	.Dump()
	;


