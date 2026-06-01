using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class CourseServiceTests
{
    
    // Functional test
    // Tests all courses are returned
    [Fact]
    public async Task GetCourses_Returns_ListOfCourses()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);


        var courses = new List<Course>
        {
            TestCourseFactory.Create(),
            TestCourseFactory.Create(),
            TestCourseFactory.Create()
        };
        
        dbContext.Courses.AddRange(courses);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await service.GetAllCourses();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
    }
    
    [Fact]
    public async Task GetCourse_ValidId_CorrectCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        
        var course1 = TestCourseFactory.Create();
        var course2 = TestCourseFactory.Create();
        var course3 = TestCourseFactory.Create();
        
        dbContext.Courses.Add(course1);
        dbContext.Courses.Add(course2);
        dbContext.Courses.Add(course3);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await service.GetCourse(course2.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(course2.Id, result.Id);
        Assert.Equal(course2.Title, result.Title);
        Assert.Equal(course2.Description, result.Description);
    }
    
    // Functional test
    // Tests all courses are returned
    [Fact]
    public async Task CreateCourse_ValidData_CreatesNewCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        
        
        // Act
        var course = TestCourseFactory.Create();
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();
        var result = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Title, result.Title);
        Assert.Equal(course.Description, result.Description);
    }

    [Fact]
    public async Task UpdateCourse_ValidId_CorrectCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        
        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var newTeacher = TestUserFactory.Create(role: UserRole.Teacher);
        dbContext.Add(teacher);
        dbContext.Add(newTeacher);
        await dbContext.SaveChangesAsync();
        
        var course = TestCourseFactory.Create(
            title: "Original Title",
            description: "Original Description"
            );
        
        course.Teacher = teacher;
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();
        
        var updatedInfo = new UpdateCourseDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            TeacherId = newTeacher.Id
        };
        
        // Act
        var result = await service.UpdateCourse(course.Id, updatedInfo);
       
        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Title, result.Title);
        Assert.Equal(course.Description, result.Description);
        Assert.Equal(course.TeacherId, result.TeacherId);
    }

    [Fact]
    public async Task DeleteCourse_ValidId_CorrectCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        var course = TestCourseFactory.Create();
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();
        
        // Act
        await service.DeleteCourse(course.Id);
        var deletedCourse = dbContext.Courses.FirstOrDefault(c => c.Id == course.Id);
        var courseCount = dbContext.Courses.Count(c => c.Id == course.Id);
        
        // Assert
        Assert.Null(deletedCourse);
        Assert.Equal(0, courseCount);
        
        
    }
    
    [Fact]
    public async Task GetCoursesByTeacherId_ValidId_CorrectCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        
        var teacher  = TestUserFactory.Create(role: UserRole.Teacher);
        dbContext.Users.Add(teacher);
        await dbContext.SaveChangesAsync();
        
        var course1 = TestCourseFactory.Create();
        var course2 = TestCourseFactory.Create();
        var course3 = TestCourseFactory.Create();
        course1.Teacher = teacher;
        course2.Teacher = teacher;
        
        dbContext.Courses.Add(course1);
        dbContext.Courses.Add(course2);
        dbContext.Courses.Add(course3);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await service.GetCoursesByTeacherId(teacher.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(course1.Id, result[0].Id);
        Assert.Equal(course2.Id, result[1].Id);
    }
    
    [Fact]
    public async Task GetCoursesByStudentId_ValidId_CorrectCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        
        var student  = TestUserFactory.Create(role: UserRole.Student);
        dbContext.Users.Add(student);
        await dbContext.SaveChangesAsync();
        
        var course1 = TestCourseFactory.Create();
        var course2 = TestCourseFactory.Create();
        var course3 = TestCourseFactory.Create();
        course1.Teacher = student;
        course2.Teacher = student;
        
        dbContext.Courses.Add(course1);
        dbContext.Courses.Add(course2);
        dbContext.Courses.Add(course3);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await service.GetCoursesByTeacherId(student.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(course1.Id, result[0].Id);
        Assert.Equal(course2.Id, result[1].Id);
    }

    [Fact]
    public async Task GetEnrolledStudents_ValidId_CorrectCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        
        var course1 = TestCourseFactory.Create();
        var course2 = TestCourseFactory.Create();
        dbContext.Courses.Add(course1);
        dbContext.Courses.Add(course2);
        await dbContext.SaveChangesAsync();

        var students = new List<User>()
        {
            TestUserFactory.Create(role: UserRole.Student),
            TestUserFactory.Create(role: UserRole.Teacher),
            TestUserFactory.Create(role: UserRole.Student),
            TestUserFactory.Create(role: UserRole.Teacher)
        };
        
        course1.Students = students;
        dbContext.Users.AddRange(students);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await service.GetEnrolledStudents(course1.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }
    
    [Fact]
    public async Task UnenrollStudent_ValidIds_StudentUnenrolled()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);
        
        var course = TestCourseFactory.Create();
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        var student = TestUserFactory.Create(role: UserRole.Student);
        course.Students = new List<User> { student };
        dbContext.Users.Add(student);
        await dbContext.SaveChangesAsync();
        
        // Act
        await service.UnenrollStudent(course.Id, student.Id);
        var updatedCourse = await dbContext.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == course.Id);
        
        // Assert
        Assert.NotNull(updatedCourse);
        Assert.Empty(updatedCourse.Students);
    }
    
}