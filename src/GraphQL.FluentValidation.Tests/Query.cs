using GraphQL.Types;

public class Query :
    ObjectGraphType
{
    public Query()
    {
        Field<ResultGraph>(
            "inputQuery",
            arguments: new QueryArguments(
                new QueryArgument<InputGraph> { Name = "input" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<Input>("input");
                return new Result(){Data = id.Content};
            }
        );
    }
}