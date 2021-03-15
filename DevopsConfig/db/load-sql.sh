#!/usr/bin/env bash
#
echo "Start waiting to until db.sql is generated"
sql_file="/app/shared_data/db.sql"
until [ -f $sql_file ]
do
  sleep 5
done
echo "Start dump to db"
# Dump sql file
if [ "$MIGRATE_NEW_DB" -eq 1 ]; then
  psql -U "$POSTGRES_USER" "$POSTGRES_DB" < $sql_file
  echo "Dump db completed!"
else
  echo "No Dump data!"
fi

exit 0