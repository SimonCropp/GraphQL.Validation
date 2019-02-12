using GraphQL.Types;

#region graph
public class MyInputGraph : InputObjectGraphType
{
    public MyInputGraph()
    {
        Field<StringGraphType>("content");
    }
}
#endregion