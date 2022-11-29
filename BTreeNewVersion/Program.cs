using BTreeNewVersion;

public static class Program
{
    public static void Main()
    {
        var btree = new BTree<int>(3);
        btree.AddNewValue(50);
        btree.AddNewValue(40);
        btree.AddNewValue(60);
        btree.AddNewValue(20);
        btree.AddNewValue(30);
        btree.AddNewValue(35);
        btree.AddNewValue(45);

    }

}