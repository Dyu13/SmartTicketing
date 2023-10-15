# SmartTicketing

# API
- run docker compose up in a terminal from the root project folder
- in docker: RabbitMQ, API (project structure is Clean Architecture so the Dockerfile for it will copy and restore all the dependencies), external AI app (compute the summary)
- The communication between the API and Microservice AI is done async through Messaging Queue Events, the payload will contain the SignalR connection id of the client app
- The communication back from the AI service to the API is done through the SignalR which will connect to the hub and send a message with the connection id and the result summary
- The communication between the client app and API is done through REST with OData support and for the summary a SignalR connection is established and the connection id passed allong so that when the message has to be send back, it will use the connection id
- Both the API and the AI depends on RabbitMQ to work properly, but depends_on and wait_for_it.sh script were not enough to ensure that the RabbitMQ is ready to handle connections while the apps are already started and registering the connections, so a hardcoded 15s delay was placed before the connection is established and since it is done in the register services step on startup, the cold start is bigger (better to use Polly or connect on demand or properly helth check in docker-compose)

# Client
- angular app
- npm i
- npm run build
- npm start
- it will run on localhost:4200

# Database
- InMemory
- Have a background worker which populates it on startup
- There is only one user in database: test@ticketing.com with the password: testPw
- There are 300 tickets and on getting them, there is pagination with max 100 per page
- EF Core is used as a repo for the database and it also apply the OData queries on the Queriable to be efficient
