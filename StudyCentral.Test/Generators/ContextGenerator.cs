using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;

namespace StudyCentral.Test.Generators;

public class ContextGenerator
{
    public static StudyDbContext GetStudyDbContext()
    {
        var options = new DbContextOptionsBuilder<StudyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString() + "_db")
            .Options;
        
        var dbContext = new StudyDbContext(options);
        return dbContext;
    }
    
}