using GraphQL.Types;

#region Graph
public class MyInputGraph :
    InputObjectGraphType
{
    public MyInputGraph() =>
        Field<StringGraphType>("content");
}
#endregion