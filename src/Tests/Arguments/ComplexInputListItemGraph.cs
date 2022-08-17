using GraphQL.Types;

public class ComplexInputListItemGraph :
    InputObjectGraphType<ComplexInputListItem>
{
    public ComplexInputListItemGraph()
    {
        Field<NonNullGraphType<IntGraphType>>("id")
            .Resolve(ctx => ctx.Source.Id);

        Field<StringGraphType, string?>("content")
            .Resolve(ctx => ctx.Source.Content);
    }
}