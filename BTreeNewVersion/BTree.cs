namespace BTreeNewVersion;

public class BTree<TValue>
    where TValue : IComparable<TValue>
{
    public Node<TValue> _head;
    public BTree(int relations)
    {
        MaxRelations = relations;
    }
    public int MaxRelations { get; }

    public void AddNewValue(TValue value)
    {
        if (_head == null)
        {
            _head = new Node<TValue>(MaxRelations, new Cell<TValue>(value));
        }
        else
        {
            Node<TValue> currentNode = _head.SearchBottomNode(value);
            Node<TValue> possibleHead =  currentNode.Add(new DataTransferObject<TValue>(value, null));
            if (possibleHead != null)
            {
                _head = possibleHead;
            }
        }
    }
}
