#!/usr/bin/env bash
#
#echo "Start waiting to DB"
## Wait until DB is ready
#while ! exec 6<>/dev/tcp/db/5432; do
#    echo "Trying to connect to DB at 5432..."
#    sleep 5
#done

# Install tool
dotnet tool install -g dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"
# Migrate database
rm -rf ./Migrations

shared_data_dir="/app/shared_data"
[ ! -d $shared_data_dir ] && mkdir $shared_data_dir
dotnet ef migrations add InitialCreate
dotnet ef migrations script -o "$shared_data_dir/db.sql"

