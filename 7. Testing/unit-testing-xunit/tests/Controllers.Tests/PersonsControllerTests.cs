using Api.Controllers;
using Api.Interfaces;
using Api.Models;
using Api.Models.Requests;
using Controllers.Tests.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Controllers.Tests;

public class PersonsControllerTests
{
	// MethodName_SceanrioToTest_Returns/Throws

	private readonly Mock<IPersonsRepository> _mockPersonsRepository;

	private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

	private readonly PersonsController _sut;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsControllerTests(ITestOutputHelper testOutputHelper)
	{
		_mockPersonsRepository = new Mock<IPersonsRepository>();

		_mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

		_sut = new PersonsController(
			_mockPersonsRepository.Object,
			_mockHttpContextAccessor.Object
		);

        _testOutputHelper = testOutputHelper;
    }

	[Theory]
	[InlineData(1, 10)]
    [InlineData(2, 20)]
    public async Task GetPersonsAsync_WhenPassedValidPageAndLimit_ReturnsHttp200OkWithListOfPersons(int page, int limit)
	{
		// Arrange
		_mockHttpContextAccessor
		.SetupGet(x => x.HttpContext.RequestAborted)
		.Returns(CancellationToken.None);

		_mockPersonsRepository
		.Setup(x => x.GetPersonOffsetPaginationAsync(
			It.Is<int>(y => y == page),
			It.Is<int>(y => y == limit),
			It.IsAny<CancellationToken>()
		))
		.ReturnsAsync(new List<Person>
		{
			new Person
			{
				Id = Guid.NewGuid(),
				FirstName = "John",
				LastName = "Doe",
				Age = 20
			},

            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Doe",
                Age = 20
            }
        });

		// Act
		ActionResult<IReadOnlyList<Person>> response = await _sut.GetPersonsAsync(page, limit);

		// Assert
		ObjectResult result = response.Result as ObjectResult;

		Assert.NotNull(result);

		IReadOnlyList<Person> payload = result.Value as IReadOnlyList<Person>;

		Assert.NotNull(payload);

		Assert.True(payload.Count > 0);

		_mockPersonsRepository
		.Verify(x => x.GetPersonOffsetPaginationAsync(
			It.Is<int>(y => y == page),
			It.Is<int>(y => y == limit),
			It.IsAny<CancellationToken>()
		), Times.Once);
    }

	[Theory]
	[ClassData(typeof(AddPersonsDtoTestData))]
	public async Task AddPersonAsync_WhenPassedAValidRequestBody_ReturnsHttp201Created(AddPersonDto dto)
	{
		_testOutputHelper.WriteLine("11");

        // Arrange
        _mockHttpContextAccessor
		.SetupGet(x => x.HttpContext.RequestAborted)
		.Returns(CancellationToken.None);

		Person p = new()
		{
			FirstName = dto.FirstName,
			LastName = dto.LastName,
			Age = dto.Age
		};

		_mockPersonsRepository
		.Setup(x => x.AddPersonAsync(It.Is<Person>(y => y.FirstName == p.FirstName && y.LastName == p.LastName && y.Age == p.Age), It.IsAny<CancellationToken>()))
		.ReturnsAsync(p);

		// Act
		ActionResult<Person> response = await _sut.AddPersonAsync(dto);

        // Assert
        ObjectResult result = response.Result as ObjectResult;

		Assert.NotNull(result);

		Assert.Equal(StatusCodes.Status201Created, result.StatusCode);

		_mockPersonsRepository
		.Verify(x => x.AddPersonAsync(It.Is<Person>(y => y.FirstName == p.FirstName && y.LastName == p.LastName && y.Age == p.Age), It.IsAny<CancellationToken>()), Times.Once());
    }
}