public class ComplexInputGraph :
    InputObjectGraphType
{
    public ComplexInputGraph()
    {
        Field<ComplexInputInnerGraph>("inner");

        Field<ListGraphType<NonNullGraphType<ComplexInputListItemGraph>>>("items");
    }
}