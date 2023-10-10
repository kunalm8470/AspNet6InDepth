using Api.Models.Requests;
using System.Collections;

namespace Controllers.Tests.TestData;

public class AddPersonsDtoTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new AddPersonDto
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 10
            }
        };

        yield return new object[]
        {
            new AddPersonDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Age = 10
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
