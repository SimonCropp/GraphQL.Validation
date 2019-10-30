using GraphQL;
using GraphQL.Types;

public class Query :
    ObjectGraphType
{
    public Query()
    {
        Field<ResultGraph>(
            "inputQuery",
            arguments: new QueryArguments(
                new QueryArgument<InputGraph> { Name = "input", }
            ),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<Input>("input");
                return new Result
                {
                    Data = input.Content
                };
            }
        );

        Field<ResultGraph>(
            "complexInputQuery",
            arguments: new QueryArguments(
                new QueryArgument<ComplexInputGraph> { Name = "input", }
            ),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<ComplexInput>("input");
                return new Result
                {
                    Data = input.Inner!.Content
                };
            }
        );

        FieldAsync<ResultGraph>(
            "asyncQuery",
            arguments: new QueryArguments(
                new QueryArgument<InputGraph> { Name = "input", }
            ),
            resolve: async context =>
            {
                var input = await context.GetValidatedArgumentAsync<AsyncInput>("input");
                return new Result
                {
                    Data = input.Content
                };
            }
        );
    }

}