I typically use a UnitOfWork Pattern when working with EntityFramework
to enable transactional processing. This is probably over-kill for this
exercise, but you asked to showcase my skills, so here it is.

The way the UoW Pattern works is that you encapsulate your business logic for 
distinct processes into a single call to a class that returns a Results object.
If the call fails at any point before changes are saved, then it acts as a 
Roll Back would when using standard Transactions.

By separating the actual repository from the UoW, we can enable Unit Testing 
by using Dependency Injection (not a sophisticated type of injection in this 
case, but still DI-type pattern). Unit tests can instantiate a UoW class using
a 'mocked' representation of the ISystemRepository interface to represent 
calls to the database to read and write data.