namespace BTreeNewVersion;


public class Node<TValue>
where TValue : IComparable<TValue>
{
    public Node(int relations, Cell<TValue>? cell = null)
    {
        MaxNumberCells = relations - 1;
        HeadCell = cell;
    }
    public int MaxNumberCells { get; }
    public Cell<TValue> HeadCell { get; private set; }
    public Node<TValue>? ParentNode { get; private set; }
    public int CountCellInNode => HeadCell.CountCell();
    public bool Odd => CountCellInNode % 2 == 0;

    public Node<TValue>? Add(DataTransferObject<TValue> dto)
    {
        if (MaxNumberCells > CountCellInNode)
        {
            AddNewCellInNotFullNode(dto);
        }
        else
        {

            DataTransferObject<TValue> newDto = DivisionNode(SearchMedianValue(dto.Value), dto);

            if (ParentNode != null)
            {
                return ParentNode.Add(newDto);
            }
            else
            {
                Node<TValue> newHead = new Node<TValue>(
                    MaxNumberCells + 1,
                    new Cell<TValue>(newDto.Value, null, newDto.LeftNode, newDto.RightNode));
                newDto.LeftNode.ParentNode = newHead;
                newDto.RightNode.ParentNode = newHead;
                return newHead;
            }
        }
        return null;
    }

    public Node<TValue> SearchBottomNode(TValue value)
    {
        var currentCell = HeadCell;
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

    public Cell<TValue> CreateNewCell(DataTransferObject<TValue> dto)
    {
        if (dto.LeftNode != null)
        {
            dto.LeftNode.ParentNode = this;
            dto.RightNode.ParentNode = this;
        }

       return new Cell<TValue>(dto.Value, null, dto.LeftNode, dto.RightNode); 
    }

    public void AddNewCellInNotFullNode(DataTransferObject<TValue> dto)
    {
        if (HeadCell == null)
        {
            HeadCell = CreateNewCell(dto);
        }
        else
        {
            Cell<TValue> currentCell = HeadCell;
            int compareResult = dto.Value.CompareTo(currentCell.Value);

            if (compareResult == -1)
            {
                SetLeftNode(0, dto.RightNode);
                HeadCell = HeadCell.AddFirst(CreateNewCell(dto));
            }
            else if (currentCell.NextCell != null)
            {
                for (int i = 1; i < CountCellInNode; i++)
                {
                    int compareNextCellResult = dto.Value.CompareTo(currentCell.NextCell.Value);
                    if (compareResult == 1 && (compareNextCellResult == -1 || compareNextCellResult == 0))
                    {
                        SetRightNode(i - 1, dto.LeftNode);
                        SetLeftNode(i, dto.RightNode);
                        HeadCell = HeadCell.AddCellBeforeIndex(CreateNewCell(dto), i);

                        break;
                    }
                    currentCell = currentCell.NextCell;
                    compareResult = compareNextCellResult;

                    if (currentCell.NextCell == null)
                    {
                        SetRightNode(CountCellInNode - 1, dto.LeftNode);
                        HeadCell = HeadCell.AddLastCell(CreateNewCell(dto));
                        break;
                    }
                }
            }
            else
            {
                HeadCell.RightNode = dto.LeftNode;
                HeadCell = HeadCell.AddLastCell(CreateNewCell(dto));
            }
        }
    }

    public void SetLeftNode(int index, Node<TValue> node)
    {
        HeadCell.GetCellByIndex(index).LeftNode = node;
    }
    public void SetRightNode(int index, Node<TValue> node)
    {
        HeadCell.GetCellByIndex(index).RightNode = node;
    }

    public Node<TValue> FormBranchs(TValue value, Cell<TValue> cell)
    {
        Node<TValue> rightNode = new Node<TValue>(MaxNumberCells + 1);
        var halfCount = CountCellInNode / 2;
        var beforeHalfCount = halfCount - 1;
        var cellByHalfcount = HeadCell.GetCellByIndex(halfCount);

        if (Odd || value.CompareTo(cellByHalfcount.Value) < 0)
        {
            rightNode.HeadCell = cellByHalfcount;
            HeadCell.DeleteAllCellAfterIndex(beforeHalfCount);
        }
        else
        {
            rightNode.HeadCell = cell;
            HeadCell.DeleteAllCellAfterIndex(halfCount);
        }

        return rightNode;
    }

    public DataTransferObject<TValue> DivisionNode(int index, DataTransferObject<TValue> dto)
    {
        var halfCount = CountCellInNode / 2;
        var cellByHalfcount = HeadCell.GetCellByIndex(halfCount);
        DataTransferObject<TValue>? result = null;

        if (index > 0)
        {
            result = new DataTransferObject<TValue>(HeadCell.GetCellByIndex(index).Value);
            HeadCell.DeleteCellByIndex(index);
            AddNewCellInNotFullNode(dto);
        }

        result ??= new DataTransferObject<TValue>(dto.Value);

        result.LeftNode = this;
        result.RightNode = FormBranchs(result.Value, cellByHalfcount.NextCell);

        return result;
    }

    public int SearchMedianValue(TValue newValue)
    {
        const int NEW_VALUE_INDEX = -1;
        var halfCount = CountCellInNode / 2;
        var possibleMiddleCellIndex = halfCount - 1;
        var possibleMiddleCell = HeadCell.GetCellByIndex(possibleMiddleCellIndex);

        if (Odd == true)
        {
            if (newValue.CompareTo(possibleMiddleCell.Value) < 0)
            {
                return possibleMiddleCellIndex;
            }

            if (newValue.CompareTo(possibleMiddleCell.NextCell!.Value) >= 0)
            {
                return possibleMiddleCellIndex + 1;
            }

            return NEW_VALUE_INDEX;
        }
        else
        {
            if (newValue.CompareTo(possibleMiddleCell.Value) > 0 &&
                newValue.CompareTo(possibleMiddleCell.NextCell!.NextCell!.Value) < 0)
            {
                return NEW_VALUE_INDEX;
            }

            return halfCount;
        }
    }
}

public class DataTransferObject<TValue>
where TValue : IComparable<TValue>
{
    public TValue Value { get; set; }
    public Cell<TValue>? NextCell { get; set; }
    public Node<TValue>? LeftNode { get; set; }
    public Node<TValue>? RightNode { get; set; }

    public DataTransferObject(TValue value, Cell<TValue>? nextCell = null, Node<TValue>? leftNode = null, Node<TValue>? rightNode = null)
    {
        Value = value;
        NextCell = nextCell;
        LeftNode = leftNode;
        RightNode = rightNode;
    }
}