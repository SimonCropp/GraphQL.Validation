class TypeComparer :
    IComparer<Type>
{
    public int Compare(Type? x, Type? y) =>
        x?.FullName?.CompareTo(y?.FullName) ?? 0;
}