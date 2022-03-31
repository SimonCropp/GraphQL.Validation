using GraphQL.Types;

public class ComplexInputListItemGraph :
    InputObjectGraphType<ComplexInputListItem>
{
    public ComplexInputListItemGraph()
    {
        Field<NonNullGraphType<IntGraphType>>()
            .Name("id")
            .Resolve(ctx => ctx.Source.Id);

        Field<StringGraphType, string?>()
            .Name("content")
            .Resolve(ctx => ctx.Source.Content);
    }
}