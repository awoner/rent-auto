#!/bin/bash

# Define variables
BASE_URL="http://localhost:85"
NUM_USERS=10
NUM_REQUESTS=100

# Define functions for different endpoints
function load_cars_get_endpoint() {
    C:\siege-windows\siege.exe -c $NUM_USERS -r $NUM_REQUESTS "$BASE_URL/api/v1/cars" 
}

function load_cars_put_endpoint() {
    C:\siege-windows\siege.exe -c $NUM_USERS -r $NUM_REQUESTS "$BASE_URL/api/v1/cars" -X PUT 
}

function load_users_get_endpoint() {
    C:\siege-windows\siege.exe -c $NUM_USERS -r $NUM_REQUESTS "$BASE_URL/api/v1/users" 
}

function load_users_put_endpoint() {
    C:\siege-windows\siege.exe -c20 -t60S "$BASE_URL/api/v1/users" PUT
}

# Main function to start the load testing
function main() {
    echo "Loading cars GET endpoint..."
    load_cars_get_endpoint
    echo "Cars GET endpoint load test completed."

    echo "Loading cars PUT endpoint..."
    load_cars_put_endpoint
    echo "Cars PUT endpoint load test completed."

    echo "Loading users GET endpoint..."
    load_users_get_endpoint
    echo "Users GET endpoint load test completed."

    echo "Loading users PUT endpoint..."
    load_users_put_endpoint
    echo "Users PUT endpoint load test completed."
}

# Call main function
main