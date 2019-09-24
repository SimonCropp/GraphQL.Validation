using GraphQL.Types;

public class ComplexInputInnerGraph :
    InputObjectGraphType<ComplexInputInner>
{
    public ComplexInputInnerGraph()
    {
        Field<StringGraphType, string>()
            .Name("content")
            .Resolve(ctx => ctx.Source.Content);
    }
}