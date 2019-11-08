using GraphQL.Types;

public class AsyncComplexInputGraph :
    InputObjectGraphType
{
    public AsyncComplexInputGraph()
    {
        Field<ComplexInputInnerGraph>("inner");
    }
}