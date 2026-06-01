using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;

namespace StudyCentral.Test;

public class ContextGenerator
{
    public static StudyDbContext GetStudyDbContext()
    {
        var options = new DbContextOptionsBuilder<StudyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString() + "_db")
            .Options;
        
        var dbContext = new StudyDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
    
}