namespace gBanker.Core.Utility
{
    public class ConstantDropdownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }

    public class ConstantTreeItem
    {
        public string DisplayText { get; set; }
        public int ParentValue { get; set; }
        public int? ChildValue { get; set; }
        public string Selected { get; set; }
    }
}
