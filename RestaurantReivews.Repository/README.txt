This is the Repository assembly. This assembly contains the Interface used for the 
repository class: SystemRepository : ISystemRepository.

It also contains the EntityFrameworkCore Context class.

In this case, I'm using EntityFrameworkCore and Sqlite, to store the data in a
memory-only database for Unit Testing the SystemRepository class. The API project
will store an actual database file, and the filename is passed to the constructor
of the RestaurantReviewsContext class.

The database, whether it's memory-only or file, is instantiated in the context
class' constructor:

    Database.EnsureCreated();

Although this specific exercise is using Sqlite, it can easily be converted to
use MS SQL Server, or any database system where Entity Framework can be integrated.
