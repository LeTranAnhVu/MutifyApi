#!/usr/bin/env bash
#
#echo "Start waiting to DB"
## Wait until DB is ready
#while ! exec 6<>/dev/tcp/db/5432; do
#    echo "Trying to connect to DB at 5432..."
#    sleep 5
#done


# Migrate database
#rm -rf ./Migrations
#dotnet ef migrations add InitialCreate
#dotnet ef migrations script -o ./db.sql
#dotnet ef database update
