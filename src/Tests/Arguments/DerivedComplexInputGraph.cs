using GraphQL.Types;

public class DerivedComplexInputGraph :
    InputObjectGraphType
{
    public DerivedComplexInputGraph()
    {
        Field<ComplexInputInnerGraph>("inner");

        Field<ListGraphType<NonNullGraphType<ComplexInputListItemGraph>>>("items");

        Field<StringGraphType>("someProperty");
    }
}