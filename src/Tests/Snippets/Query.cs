using GraphQL;
using GraphQL.Types;

#region GetValidatedArgument

public class MyQuery :
    ObjectGraphType
{
    public MyQuery()
    {
        Field<ResultGraph>(
            "inputQuery",
            arguments: new QueryArguments(
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
}

#endregion