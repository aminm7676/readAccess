namespace readAccess
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string Name { get; }
        public int Order { get; set; }

        public ColumnNameAttribute(string name)
        {
            Name = name;
            Order = int.MaxValue;
        }

        public ColumnNameAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }

}
