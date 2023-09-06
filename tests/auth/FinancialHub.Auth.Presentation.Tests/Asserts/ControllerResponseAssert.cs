using FinancialHub.Common.Tests.Assertions.Responses;

namespace FinancialHub.Auth.Presentation.Tests.Asserts
{
    public static class ControllerResponseAssert
    {
        public static void IsValid<T>(BaseResponse<T> expectedResponse, ObjectResult result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as BaseResponse<T>;
                BaseResponseAssert.IsValid(expectedResponse, response!);
            });
        }

        public static void HasError(BaseErrorResponse expectedResponse, ObjectResult result, int expectedStatusCode = 400)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(expectedStatusCode));
                Assert.That(result.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as ValidationErrorResponse;
                BaseResponseAssert.HasError(expectedResponse, response!);
            });
        }
    }
}
