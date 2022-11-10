using System.Threading;
using Selenium.Entities;
using Xunit;

namespace Selenium.Tests;

[Collection("Collection")]
public class ProjectTests
{
    private readonly TestsFixture _fixture;

    public ProjectTests(TestsFixture fixture) =>
        _fixture = fixture;

    [Theory]
    [InlineData("ТестИмя1", "ТестИмя1")]
    // [InlineData("xdXDxDXd", "xdXDxDXd")]
    // [InlineData("sasasa", "sasasa")]
    public void Add_LoggedInAndTitleProvided_ShouldCreateNewProject(string title, string expected)
    {
        // arrange
        var project = new Project {Title = title};

        // act
        _fixture.App.AccountHelper.Login(_fixture.Account);

        _fixture.App.ProjectHelper.AddProject(project);
        Thread.Sleep(3000);

        // assert
        Assert.Equal(expected, _fixture.App.ProjectHelper.GetLastCreatedProject().Title);
    }

    [Theory]
    // [InlineData(2)]
    [InlineData(1)]
    public void Delete_ProjectExists_ShouldDeleteProject(int projectNumber)
    {
        // act
        _fixture.App.AccountHelper.Login(_fixture.Account);

        var realId = _fixture.App.ProjectHelper.GetRealProjectId(projectNumber);
        _fixture.App.ProjectHelper.DeleteProject(projectNumber);
        Thread.Sleep(3000);

        // assert
        Assert.False(_fixture.App.ProjectHelper.IsProjectExists(realId));
    }

    [Theory]
    [InlineData(1, "NewTitle")]
    // [InlineData(2, "wlkrlkgertle")]
    public void UpdateTitle_ProjectExists_ShouldChangeTitle(int projectNumber, string title)
    {
        // arrange

        // act
        _fixture.App.AccountHelper.Login(_fixture.Account);
        _fixture.App.ProjectHelper.UpdateProjectTitle(projectNumber, title);
        Thread.Sleep(3000);

        // assert
        Assert.Equal(title, _fixture.App.ProjectHelper.GetProjectByNumber(projectNumber).Title);
    }
}