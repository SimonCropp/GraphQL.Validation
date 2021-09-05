using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;

public class Query :
    ObjectGraphType
{
    public Query()
    {
        Field<ResultGraph>(
            "noValidatorQuery",
            arguments: new(new QueryArgument<NoValidatorInputGraph> { Name = "input"}),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<NoValidatorInput?>("input");
                return new Result
                {
                    Data = input?.Content ?? "it was null"
                };
            }
        );
        Field<ResultGraph>(
            "inputQuery",
            arguments: new(new QueryArgument<InputGraph> { Name = "input"}),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<Input?>("input");
                return new Result
                {
                    Data = input?.Content ?? "it was null"
                };
            }
        );

        Field<ResultGraph>(
            "complexInputQuery",
            arguments: new(new QueryArgument<ComplexInputGraph> { Name = "input" }),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<ComplexInput>("input");
                return new Result
                {
                    Data = JsonConvert.SerializeObject(input)
                };
            }
        );

        Field<ResultGraph>(
            "derivedComplexInputQuery",
            arguments: new(new QueryArgument<DerivedComplexInputGraph> { Name = "input" }),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<DerivedComplexInput>("input");
                return new Result
                {
                    Data = JsonConvert.SerializeObject(input)
                };
            }
        );

        FieldAsync<ResultGraph>(
            "asyncQuery",
            arguments: new(new QueryArgument<InputGraph> { Name = "input" }),
            resolve: async context =>
            {
                var input = await context.GetValidatedArgumentAsync<AsyncInput>("input");
                return new Result
                {
                    Data = input.Content
                };
            }
        );

        FieldAsync<ResultGraph>(
            "asyncComplexInputQuery",
            arguments: new(new QueryArgument<ComplexInputGraph> { Name = "input" }),
            resolve: async context =>
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