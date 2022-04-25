# NatsTestAPI
A client app for NATS Pub/Sub with API made in 4 hours

A client app that launches a console window and an API running through IIS Express while application is running.

# TO LAUNCH 
 1. Begin connection with NATS server. For development purposes, I was connecting to "demo.nats.io" through Port 4222.
 2. If a different server is preferred, You will need to edit NatsController line #76 in API and MessagerClient line #81 in Console where the url is hard written.
 3. The default Subscription is "nats.demo.pubsub", which can be changed at NatsController line #16. After that, API can be built as IIS and Console as exe. 
 4. If API is deployed other than local, MessagerClient line #14 needs to be updated with API's new address.

# ISSUES
-Connection 'timesout' sporatically. -Unimplemeted solution: Write a recursive method to 'Ping' the server every few seconds to avoid timeout.
 
-Was able to implement an event handler for new messages without the API, however using the API GET method for NATS seems to throw more exceptions or miss messages. -Unimplemented solution: currently elbow deep in NATS documentation trying to figure this out.

Any future updates/fixes will be published on my github at https://github.com/JIHigg/NatsTestAPI

# Parting thoughts
This was an exciting challenge, and I thank you for giving me the opportunity to write an API for something new. I was unfamiliar with NATS before this, and it was fun to try it out and learn something. I have done my best in four hours, and I greatly enjoyed the chance to use my skills in an area I have not done before. I would love to be on your team and to get the opportunity to learn more from you all. If we make a good fit, first After Work pizza is on me (with or without pineapple, I won't judge!).
