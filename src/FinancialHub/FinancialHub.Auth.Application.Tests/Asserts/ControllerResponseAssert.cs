﻿namespace FinancialHub.Auth.Application.Tests.Asserts
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
                ResponseAssert.IsValid(expectedResponse, response!);
            });
        }

        public static void HasError(BaseErrorResponse expectedResponse, ObjectResult result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(400));
                Assert.That(result.Value, Is.TypeOf(expectedResponse.GetType()));

                var response = result.Value as ValidationErrorResponse;
                ResponseAssert.HasError(expectedResponse, response!);
            });
        }
    }
}
