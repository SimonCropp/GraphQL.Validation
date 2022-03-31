using GraphQL;
using GraphQL.Types;

#region GetValidatedArgument

public class Query :
    ObjectGraphType
{
    public Query() =>
        Field<ResultGraph>(
            "inputQuery",
            arguments: new(
                new QueryArgument<MyInputGraph>
                {
                    Name = "input"
                }
            ),
            resolve: context =>
            {
                var input = context.GetValidatedArgument<MyInput>("input");
                return new Result
                {
                    Data = input.Content
                };
            }
        );
}

#endregion