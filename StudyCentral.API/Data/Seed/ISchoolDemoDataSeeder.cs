namespace StudyCentral.API.Data.Seed;

public interface ISchoolDemoDataSeeder
{
    Task<SchoolDemoSeedResult> SeedAsync();
}