﻿using GraphQL;
using GraphQL.Types;

#region GetValidatedArgument

public class Query :
    ObjectGraphType
{
    public Query() =>
        Field<ResultGraph>("inputQuery")
            .Argument<MyInputGraph>("input")
            .Resolve(context =>
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