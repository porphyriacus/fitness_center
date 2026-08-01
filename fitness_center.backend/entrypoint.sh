#!/bin/bash
set -e
MAX_RETRIES=5
RETRY_COUNT=0

until dotnet ef database update --project ../Infrastructure --startup-project . || [ $RETRY_COUNT -eq $MAX_RETRIES ]; do
  RETRY_COUNT=$((RETRY_COUNT + 1))
  >&2 echo "Migrations failed (attempt $RETRY_COUNT/$MAX_RETRIES), retrying in 5 seconds..."
  sleep 5
done

if [ $RETRY_COUNT -eq $MAX_RETRIES ]; then
  >&2 echo "Failed to apply migrations after $MAX_RETRIES attempts. Starting app anyway..."
else
  >&2 echo "Migrations applied successfully."
fi

exec dotnet API.dll