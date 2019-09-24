using GraphQL.Types;

public class ComplexInputGraph :
    InputObjectGraphType
{
    public ComplexInputGraph()
    {
        Field<ComplexInputInnerGraph>("inner");
    }
}