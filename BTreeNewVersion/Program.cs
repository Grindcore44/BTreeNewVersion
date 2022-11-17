using BTreeNewVersion;

public static class Program
{
    public static void Main()
    {
        var btree = new BTree<int>(3);
        btree.AddNewValue(20);
        btree.AddNewValue(45);
        btree.AddNewValue(60);
        btree.AddNewValue(80);
        btree.AddNewValue(100);
        btree.AddNewValue(120);
        btree.AddNewValue(140);
        btree.AddNewValue(160);

    }

}