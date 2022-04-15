# Social-Media
I developed an interest in microservices and distributed systems, and believed building a social media backend api may be a good start. 
## Architecture
### Languages/Framework
* ASP.NET Core (C#)
### DevOps
* Docker used for containerization
* Kubernetes used for orchestration
* Nginx Ingress Controller for loadbalancing. Allows for traffice to be directed to appropriate services and ingress level authentication.
### Messaging
* RabbitMQ, using the direct Publisher/Consumer model. Each service/need will have its own queue. 
* Will explore GRPC in the future
### Database
* Microsoft SQL Server for keeping track of users, posts, newfeeds
* Neo4j Graph Database for keeping track of connections amongst users
* Will explore Redis caching in the future
### Microservices
* Authentication Service
  * Handles logins, regisration and authentication
  * Passwords are hashed using Bcrypt and JWT is utilized to authenticate the user
* Email Service
  * Email sent out on registration for user awareness
  * Will explore emails for resetting accounts, confirmation for creating accounts
* Personal Network Service
  * Handles the follower and following interactions between users 
* Post Service
  * Handles users creating posts and comments
* Feed Service
  * Generates the user's home feed and a user's profile feed
  * To look into various feed generation models
### To Explore in The Future
* Redis Caching
* OAuth
* Kafka
* GRPC
* Cassandra Database
* Vault/Secret Managent
* Distributed Logging (eg. Elastic)


