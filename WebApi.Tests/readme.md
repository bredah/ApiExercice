# Unit Test

## EF - Memory Data Base

Why do you need a EF InMemory Provider:

- Just a database provider
- Built for test purposes
- Uses in-memory database
- No overhead of I/O operations
- Lighweight with minimal dependencies

> To use the .UseInMemoryDatabase() extension method, reference the NuGet package Microsoft.EntityFrameworkCore.InMemory.