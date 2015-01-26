Notes:

To enable migration
-------------------------
enable-migrations
add-migration "Initialize Database"
Update-Database -Verbose

To update the db
------------------------
add-migration "EnterUpdateDescription"
Update-Database -Verbose


Generate a sql file 
-------------------------
Update-Database -Script -SourceMigration:0