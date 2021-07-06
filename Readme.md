Steps
1. install dotnet ef tools
   dotnet tool install --global dotnet-ef
2. EF migration
   dotnet ef database update
3. Run WebApp
4. run service fabric with local db, should set the following:
   4a. With the SqlLocalDB.exe command line share your connection issuing this command:
		sqllocaldb share MSSqlLocalDB SharedDB
   4b.Open the LocalDB with SQL Server Management Studio and go to the /Security/Logins, add the NETWORK SERVICE local account 
		and in service roles as sysadmin, in User Mapping add it as dbo (dbo.dbowner) to your database
   4c. Use the shared name in your connection string like this:
      "Data Source=(localdb)\.\SharedDB;Initial Catalog=[YOUR DB];Integrated Security=SSPI;"
   4d. With the SqlLocalDB.exe command line share your connection issuing this command:
		sqllocaldb stop MSSQLLocalDB
		sqllocaldb start MSSQLLocalDB


Okta Backend Authentication and Authorization Steps:
1. Okta Security=>API trust origin settings
2. Okta Security=>API=>Default Authorization claim settings as the following: 
	a) Add ID token claim (name: groups; Include in token type: ID Token; Value type: Groups; Filter: Matchs Regex, .*; Include In: Any scope );
	b) Add accept token cliam (name: groups; Include in token type: Access Token; Value type: Groups; Filter: Matchs Regex, .*; Include In: Any scope );
