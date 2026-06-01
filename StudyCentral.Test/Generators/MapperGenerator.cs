using AutoMapper;
using StudyCentral.API.Models;

namespace StudyCentral.Test.Generators;

public static class MapperGenerator
{
    public static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        return config.CreateMapper();
    }
}