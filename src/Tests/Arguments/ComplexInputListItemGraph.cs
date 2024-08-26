public class ComplexInputListItemGraph :
    InputObjectGraphType<ComplexInputListItem>
{
    public ComplexInputListItemGraph()
    {
        Field<NonNullGraphType<IntGraphType>>("id");
        Field<StringGraphType, string?>("content");
    }
}