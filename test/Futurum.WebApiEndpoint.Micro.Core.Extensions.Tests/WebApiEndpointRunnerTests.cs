using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Tests;

public class WebApiEndpointRunnerTests
{
    private const string REQUEST_PATH = "/RequestPath";

    private const string ErrorMessage1 = "ERROR_MESSAGE1";
    private const string ErrorMessage2 = "ERROR_MESSAGE2";

    private const string VALUE = "Value";

    public class Sync
    {
        public class Value
        {
            public class ErrorMessage
            {
                [Fact]
                public void Success()
                {
                    var value = 1;

                    var results = WebApiEndpointRunner.Run(() => value,
                                                           CreateHttpContext(),
                                                           ToOk,
                                                           ErrorMessage2);

                    ValidateOk(results, value);
                }

                [Fact]
                public void Exception()
                {
                    var value = 1;

                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return value;
                                                           },
                                                           CreateHttpContext(),
                                                           ToOk,
                                                           ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void Success()
                {
                    var value = 1;

                    var results = WebApiEndpointRunner.Run(() => value,
                                                           CreateHttpContext(),
                                                           ToOk,
                                                           () => ErrorMessage2);

                    ValidateOk(results, value);
                }

                [Fact]
                public void Exception()
                {
                    var value = 1;

                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return value;
                                                           },
                                                           CreateHttpContext(),
                                                           ToOk,
                                                           () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok<int>, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateOk(Results<Ok<int>, BadRequest<ProblemDetails>> results, int value)
            {
                results.Result.Should().BeOfType<Ok<int>>();
                results.Result.As<Ok<int>>().Value.Should().Be(value);
            }
        }
    }

    public class Async
    {
        public class Value
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task Success()
                {
                    var value = 1;

                    var results = await WebApiEndpointRunner.RunAsync(() => Task.FromResult(value),
                                                                      CreateHttpContext(),
                                                                      ToOk,
                                                                      ErrorMessage2);

                    ValidateOk(results, value);
                }

                [Fact]
                public async Task Exception()
                {
                    var value = 1;

                    var results = await WebApiEndpointRunner.RunAsync(() =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return Task.FromResult(value);
                                                                      },
                                                                      CreateHttpContext(),
                                                                      ToOk,
                                                                      ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task Success()
                {
                    var value = 1;

                    var results = await WebApiEndpointRunner.RunAsync(() => Task.FromResult(value),
                                                                      CreateHttpContext(),
                                                                      ToOk,
                                                                      () => ErrorMessage2);

                    ValidateOk(results, value);
                }

                [Fact]
                public async Task Exception()
                {
                    var value = 1;

                    var results = await WebApiEndpointRunner.RunAsync(() =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return Task.FromResult(value);
                                                                      },
                                                                      CreateHttpContext(),
                                                                      ToOk,
                                                                      () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok<int>, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateOk(Results<Ok<int>, BadRequest<ProblemDetails>> results, int value)
            {
                results.Result.Should().BeOfType<Ok<int>>();
                results.Result.As<Ok<int>>().Value.Should().Be(value);
            }
        }

    }


    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}
