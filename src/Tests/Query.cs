using GraphQL;
using Newtonsoft.Json;

public class Query :
    ObjectGraphType
{
    public Query()
    {
        Field<ResultGraph>("noValidatorQuery")
            .Argument<NoValidatorInputGraph>("input")
            .Resolve(context =>
                {
                    var input = context.GetValidatedArgument<NoValidatorInput?>("input");
                    return new Result
                    {
                        Data = input?.Content ?? "it was null"
                    };
                }
            );
        Field<ResultGraph>("inputQuery")
            .Argument<InputGraph>("input")
            .Resolve(context =>
                {
                    var input = context.GetValidatedArgument<Input?>("input");
                    return new Result
                    {
                        Data = input?.Content ?? "it was null"
                    };
                }
            );

        Field<ResultGraph>("complexInputQuery")
            .Argument<ComplexInputGraph>("input")
            .Resolve(context =>
                {
                    var input = context.GetValidatedArgument<ComplexInput>("input");
                    return new Result
                    {
                        Data = JsonConvert.SerializeObject(input)
                    };
                }
            );

        Field<ResultGraph>("derivedComplexInputQuery")
            .Argument<DerivedComplexInputGraph>("input")
            .Resolve(context =>
                {
                    var input = context.GetValidatedArgument<DerivedComplexInput>("input");
                    return new Result
                    {
                        Data = JsonConvert.SerializeObject(input)
                    };
                }
            );

        Field<ResultGraph>("asyncQuery")
            .Argument<AsyncInputGraph>("input")
            .ResolveAsync(async context =>
                {
                    var input = await context.GetValidatedArgumentAsync<AsyncInput>("input");
                    return new Result
                    {
                        Data = input.Content
                    };
                }
            );

        Field<ResultGraph>("asyncComplexInputQuery")
            .Argument<AsyncComplexInputGraph>("input")
            .ResolveAsync(async context =>
                {
                    var input = await context.GetValidatedArgumentAsync<AsyncComplexInput>("input");
                    return new Result
                    {
                        Data = input.Inner!.Content
                    };
                }
            );
    }
}