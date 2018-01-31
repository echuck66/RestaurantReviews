This is a .NET Core Web API Project.

It differs somewhat from a typical ASP.NET Web API project, but essentially 
it works the same way in that it receives requests, processes them, and returns
data. The type of data posted and returned depends on properties contained in the 
request. Typically, data can be received in XML or JSON formats and returned in the 
same types of format. Serialization is handled automatically, so we don't need
to go through extra serialization/deserialization steps when sending/receiving 
requests.

One big IMPORTANT note:
In this system, we accept annonymous requests, but such a system might require some
form of authentication. In that case, I would typically use ASP.NET Authentication to
setup an authentication database, separate from the application data. This could be a 
centralized Single-Signon system that can handle requests from a variety of sources:

    web applications
    mobile applications
    desktop applications
    IoT devices
    etc.

For this type of system, I'd typically use something like IdentityServer4 and OpenID/OAuth 
and allow for external logins too via services like Facebook, Google+, LinkedIn, etc.

I'd also utilize tokenized access controls for the API so that we have authentication tokens
included in the request itself. This way, we can pass off authentication to the Single
Sign On application and accept and process authenticated requests from a variety of 
applications like a web site, mobile application, desktop application, etc. (all of the
sources able to authenticate listed above).

Since this is a coding exercise and not a full-blown system to be used in a real-world
environment, I'll also omit things like system logging, but this can easily be added
using a package such as Serilog (Nuget) with a Serilog Sink that can be configured to
write logging data to a variety of sources like a file, database, Azure or AWS 
logging service.

REST notes:

REST is an acronym for 'REpresentational State Transfer' and it essentially boils down 
to how HTTP Requests should be sent/received and describes expected behavior.

For this exercise:

1) Get a list of restaurants by city
   HTTP GET
   Pass a city and state value and return an ICollection<Restaurant> to the client

2) Post a restaurant that is not in the database
   HTTP POST
   Post a serialized Restaurant object and return that object to the client
   after it has been persisted to a database
   - subsequent calls to 'update' the object should use HTTP PUT

3) Post a review for a restaurant
   HTTP POST
   Post a serialized RestaurantReview object and return that object to the client
   after it has been persisted to a database
   - subsequent calls to 'update' the object should use HTTP PUT

4) Get of a list of reviews by user
   HTTP GET
   Pass a username value and return an ICollection<RestaurantReview> to the client

5) Delete a review
   HTTP DELETE
   Pass a key value representing a RestaurantReview object and delete that object 
   from the database, then return confirmation that it has been deleted to the client

Although this type of application would typically have multiple API Controllers
(e.g. RestaurantController and ReviewController), I am including all Controller
Actions in a single controller: ReviewController.

