using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestAPI.ResultPattern
{
  public record Result
  {
    public bool IsSuccess {get;}
    public Error? Error {get;}

    protected Result(bool isSuccess, Error? error){
      IsSuccess = isSuccess;
      Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new (false, error ?? throw new ArgumentNullException(nameof(error)));
    // public static implicit operator Result(Error error) => Failure(error);

  }

  public record Result<T> {
    
    public bool IsSuccess {get;}
    public Error? Error {get;}
    public T? Value {get;}

    private Result(T value) {
      IsSuccess = true;
      Value = value;
    }
    private Result(Error error) {
      IsSuccess = false;
      Error = error;
    }
    public static Result<T> Success (T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

  }
}

