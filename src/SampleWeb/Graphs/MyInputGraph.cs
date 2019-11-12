using GraphQL.Types;

public class MyInputGraph :
    InputObjectGraphType
{
    public MyInputGraph()
    {
        Field<StringGraphType>("content");
    }
}
