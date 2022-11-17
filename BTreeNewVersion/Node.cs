namespace BTreeNewVersion;

public class Node<TValue>
where TValue : IComparable<TValue>
{
    public Node(int relations, Cell<TValue> cell)
    {
        MaxNumberCells = relations - 1;
        FirstCellInNode = cell;
    }
    public int MaxNumberCells { get; }
    public Cell<TValue> FirstCellInNode { get; private set; }
    public Node<TValue>? ParentNode { get; private set; }
    public int CountCellInNode => FirstCellInNode.CountCell();

    public Node<TValue>? Add(Cell<TValue> newCell)
    {
        if (MaxNumberCells > CountCellInNode)
        {
            AddNewCellInNotFullNode(newCell);
        }
        else
        {
            Cell<TValue> medianCell = DivisionNode(SearchMedianValue(newCell));
            if (ParentNode != null)
            {
                ParentNode.Add(medianCell);
            }
            else
            {
                Node<TValue> newHead = new Node<TValue>(MaxNumberCells + 1, medianCell);
                ParentNode = newHead;
                return newHead;
            }
        }
        return null;
    }

    public Node<TValue> SearchBottomNode(TValue value)
    {
        var currentCell = FirstCellInNode;
        for (int i = 0; i < CountCellInNode; i++)
        {
            int compareResult = value.CompareTo(currentCell.Value);

            if (currentCell.LeftNode == null && currentCell.RightNode == null)
            {
                return this;
            }

            if (compareResult == -1)
            {
                return currentCell.LeftNode.SearchBottomNode(value);
            }

            if (compareResult == 0)
            {
                return currentCell.RightNode.SearchBottomNode(value);
            }

            if (compareResult == 1 && currentCell.NextCell == null)
            {
                return currentCell.RightNode.SearchBottomNode(value);
            }

            if (compareResult == 1 && value.CompareTo(currentCell.NextCell.Value) == -1)
            {
                return currentCell.RightNode.SearchBottomNode(value);
            }

            if (compareResult == 1 && value.CompareTo(currentCell.NextCell.Value) == 1)
            {
                currentCell = currentCell.NextCell;
            }
        }

        return this;
    }

    public void AddNewCellInNotFullNode(Cell<TValue> newCell)
    {
        Cell<TValue> currentCell = FirstCellInNode;
        int compareResult = newCell.Value.CompareTo(currentCell.Value);

        if (compareResult == -1)
        {
            FirstCellInNode = FirstCellInNode.AddFirst(newCell);
        }
        else if (currentCell.NextCell != null)
        {

            for (int i = 1; i < CountCellInNode; i++)
            {
                int compareNextCellResult = newCell.Value.CompareTo(currentCell.NextCell.Value);
                if (compareResult == 1 && (compareNextCellResult == -1 || compareNextCellResult == 0))
                {
                    FirstCellInNode = FirstCellInNode.AddCellBeforeIndex(newCell, i);
                    break;
                }
                currentCell = currentCell.NextCell;
                compareResult = compareNextCellResult;

                if (currentCell.NextCell == null)
                {
                    FirstCellInNode = FirstCellInNode.AddLastCell(newCell);
                    break;
                }
            }
        }
        else
        {
            FirstCellInNode = FirstCellInNode.AddLastCell(newCell);
        }
    }

    public Cell<TValue> DivisionNode(Cell<TValue> medianCell)
    {

        Cell<TValue> cell = FirstCellInNode.NextCell;
        Cell<TValue> firstCell = new Cell<TValue>(FirstCellInNode.Value, FirstCellInNode.LeftNode, FirstCellInNode.RightNode);
        Node<TValue> leftNode = new Node<TValue>(MaxNumberCells + 1, firstCell);
        Node<TValue> rightNode = new Node<TValue>(MaxNumberCells + 1, FirstCellInNode.GetCellByIndex(MaxNumberCells - 1));

        while (cell != null)
        {
            if (medianCell.Value.CompareTo(cell.Value) == 1)
            {
                leftNode.AddNewCellInNotFullNode(new Cell<TValue>(cell.Value, cell.LeftNode, cell.RightNode));
            }
            else
            {
                if (cell.NextCell == null)
                {
                    break;
                }
                rightNode.AddNewCellInNotFullNode(new Cell<TValue>(cell.Value, cell.LeftNode, cell.RightNode));
            }
            cell = cell.NextCell;
        }

        medianCell.LeftNode = leftNode;
        medianCell.RightNode = rightNode;

        if (ParentNode != null)
        {
            medianCell.LeftNode.ParentNode = ParentNode;
            medianCell.RightNode.ParentNode = ParentNode;
        }

        return medianCell;
    }

    public Cell<TValue> SearchMedianValue(Cell<TValue> newCell)
    {
        if (CountCellInNode % 2 == 0)
        {
            if (newCell.Value.CompareTo(FirstCellInNode.Value) == -1)
            {
                int indexNode = (CountCellInNode / 2) - 1;
                Cell<TValue> cell = FirstCellInNode.GetCellByIndex(indexNode);
                FirstCellInNode = FirstCellInNode.DeleteCellByIndex(indexNode);
                AddNewCellInNotFullNode(newCell);
                return cell;
            }

            else if (newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex(CountCellInNode - 1).Value) == 1 ||
                newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex(CountCellInNode - 1).Value) == 0)
            {
                int indexNode;
                if (CountCellInNode == 2)
                {
                    indexNode = CountCellInNode / 2;
                }

                else
                {
                    indexNode = (CountCellInNode / 2);
                }

                Cell<TValue> cell = FirstCellInNode.GetCellByIndex(indexNode);
                FirstCellInNode = FirstCellInNode.DeleteCellByIndex(indexNode);
                AddNewCellInNotFullNode(newCell);
                return cell;
            }

            else
            {
                int leftMiddleIndex = CountCellInNode / 2 - 1;
                int rightMiddleIndex = CountCellInNode / 2 + 1;
                if (newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex(leftMiddleIndex).Value) == -1)
                {
                    Cell<TValue> cell = FirstCellInNode.GetCellByIndex(leftMiddleIndex);
                    FirstCellInNode = FirstCellInNode.DeleteCellByIndex(leftMiddleIndex);
                    AddNewCellInNotFullNode(newCell);
                    return cell;
                }
                else if ((newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex(rightMiddleIndex).Value) == 1) ||
                    (newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex(rightMiddleIndex).Value) == 0))
                {
                    Cell<TValue> cell = FirstCellInNode.GetCellByIndex(rightMiddleIndex);
                    FirstCellInNode = FirstCellInNode.DeleteCellByIndex(rightMiddleIndex);
                    AddNewCellInNotFullNode(newCell);
                    return cell;
                }
                else
                {
                    return newCell;
                }
            }
        }
        else
        {
            if (newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex((CountCellInNode / 2) - 1).Value) == 1 &&
                newCell.Value.CompareTo(FirstCellInNode.GetCellByIndex((CountCellInNode / 2) + 1).Value) == -1)
            {
                return newCell;
            }
            else
            {
                int indexNode = (CountCellInNode / 2); // CellInNodeCount = 3, return cell index 1 ( second cell)
                Cell<TValue> cell = FirstCellInNode.GetCellByIndex(indexNode);
                FirstCellInNode = FirstCellInNode.DeleteCellByIndex(indexNode);
                AddNewCellInNotFullNode(newCell);
                return cell;
            }
        }
    }
}

