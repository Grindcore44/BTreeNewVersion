using System.ComponentModel.DataAnnotations;

namespace BTreeNewVersion;


public class Node<TValue>
where TValue : IComparable<TValue>
{
    public Node(int relations, Cell<TValue>? cell = null)
    {
        MaxNumberCells = relations - 1;
        FirstCellInNode = cell;
    }
    public int MaxNumberCells { get; }
    public Cell<TValue> FirstCellInNode { get; private set; }
    public Node<TValue>? ParentNode { get; private set; }
    public int CountCellInNode => FirstCellInNode.CountCell();

    public Node<TValue>? Add(DataTransferObject<TValue> dto)
    {
        if (MaxNumberCells > CountCellInNode)
        {
            AddNewCellInNotFullNode(dto);
        }
        else
        {

            DataTransferObject<TValue> newDto = DivisionNode(SearchMedianValue(dto), dto);

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

    public void AddNewCellInNotFullNode(DataTransferObject<TValue> dto)
    {
        if (FirstCellInNode == null)
        {
            FirstCellInNode = new Cell<TValue>(dto.Value, null, dto.LeftNode, dto.RightNode);
        }
        else
        {
            Cell<TValue> currentCell = FirstCellInNode;
            int compareResult = dto.Value.CompareTo(currentCell.Value);

            if (compareResult == -1)
            {
                SetLeftNode(0, dto.RightNode);
                FirstCellInNode = FirstCellInNode.AddFirst(new Cell<TValue>(dto.Value, null, dto.LeftNode, dto.RightNode));
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
                        FirstCellInNode = FirstCellInNode.AddCellBeforeIndex(new Cell<TValue>(dto.Value, dto.NextCell, dto.LeftNode, dto.RightNode), i);

                        break;
                    }
                    currentCell = currentCell.NextCell;
                    compareResult = compareNextCellResult;

                    if (currentCell.NextCell == null)
                    {
                        SetRightNode(CountCellInNode - 1, dto.LeftNode);
                        FirstCellInNode = FirstCellInNode.AddLastCell(new Cell<TValue>(dto.Value, null, dto.LeftNode, dto.RightNode));
                        break;
                    }
                }
            }
            else
            {
                FirstCellInNode.LeftNode = dto.RightNode;
                FirstCellInNode = FirstCellInNode.AddLastCell(new Cell<TValue>(dto.Value, null, dto.LeftNode, dto.RightNode));
            }
        }
    }

    public void SetLeftNode(int index, Node<TValue> node)
    {
        FirstCellInNode.GetCellByIndex(index).LeftNode = node;
    }
    public void SetRightNode(int index, Node<TValue> node)
    {
        FirstCellInNode.GetCellByIndex(index).RightNode = node;
    }
    public DataTransferObject<TValue> DivisionNode(int index, DataTransferObject<TValue> dto)
    {

        Node<TValue> rightNode = new Node<TValue>(MaxNumberCells + 1);

        if (index < 0)
        {
            if (CountCellInNode % 2 == 0)
            {
                var firstCellLeftNode = FirstCellInNode.GetCellByIndex(CountCellInNode / 2);
                rightNode.AddNewCellInNotFullNode(new DataTransferObject<TValue>
                (firstCellLeftNode.Value, firstCellLeftNode.NextCell, firstCellLeftNode.LeftNode,
                 firstCellLeftNode.RightNode));

                FirstCellInNode.DeleteAllCellAfterIndex((CountCellInNode / 2) - 1);
            }
            else
            {
                if (dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(CountCellInNode / 2).Value) == -1)
                {
                    var firstCellLeftNode = FirstCellInNode.GetCellByIndex(CountCellInNode / 2);
                    rightNode.AddNewCellInNotFullNode(new DataTransferObject<TValue>
                    (firstCellLeftNode.Value, firstCellLeftNode.NextCell, firstCellLeftNode.LeftNode,
                    firstCellLeftNode.RightNode));

                    FirstCellInNode.DeleteAllCellAfterIndex((CountCellInNode / 2) - 1);
                }
                else
                {
                    var firstCellLeftNode = FirstCellInNode.GetCellByIndex((CountCellInNode / 2) + 1);
                    rightNode.AddNewCellInNotFullNode(new DataTransferObject<TValue>
                    (firstCellLeftNode.Value, firstCellLeftNode.NextCell, firstCellLeftNode.LeftNode,
                    firstCellLeftNode.RightNode));

                    FirstCellInNode.DeleteAllCellAfterIndex(CountCellInNode / 2);
                }
            }
        }
        else
        {
            if (CountCellInNode % 2 == 0)
            {
                var newDto = new DataTransferObject<TValue>(FirstCellInNode.GetCellByIndex(index).Value);
                FirstCellInNode.DeleteCellByIndex(index);
                AddNewCellInNotFullNode(dto);
                dto = newDto;

                var firstCellLeftNode = FirstCellInNode.GetCellByIndex(CountCellInNode / 2);
                rightNode.AddNewCellInNotFullNode(new DataTransferObject<TValue>(firstCellLeftNode.Value,
                firstCellLeftNode.NextCell, firstCellLeftNode.LeftNode, firstCellLeftNode.RightNode));
                FirstCellInNode.DeleteAllCellAfterIndex((CountCellInNode / 2) - 1);
            }
            else
            { 
                if (dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(index).Value) == -1)
                {
                    var newDto = new DataTransferObject<TValue>(FirstCellInNode.GetCellByIndex(index).Value);
                    FirstCellInNode.DeleteCellByIndex(index);
                    AddNewCellInNotFullNode(dto);
                    dto = newDto;

                    var firstCellLeftNode = FirstCellInNode.GetCellByIndex(CountCellInNode / 2);
                    rightNode.AddNewCellInNotFullNode(new DataTransferObject<TValue>(firstCellLeftNode.Value,
                    firstCellLeftNode.NextCell, firstCellLeftNode.LeftNode, firstCellLeftNode.RightNode));
                    FirstCellInNode.DeleteAllCellAfterIndex((CountCellInNode / 2) - 1);
                }

                else
                {
                    var newDto = new DataTransferObject<TValue>(FirstCellInNode.GetCellByIndex(index).Value);
                    FirstCellInNode.DeleteCellByIndex(index);
                    AddNewCellInNotFullNode(dto);
                    dto = newDto;

                    var firstCellLeftNode = FirstCellInNode.GetCellByIndex((CountCellInNode / 2) - 1);
                    rightNode.AddNewCellInNotFullNode(new DataTransferObject<TValue>(firstCellLeftNode.Value,
                    firstCellLeftNode.NextCell, firstCellLeftNode.LeftNode, firstCellLeftNode.RightNode));
                    FirstCellInNode.DeleteAllCellAfterIndex((CountCellInNode / 2));
                }
            }

        }

        dto.LeftNode = this;
        dto.RightNode = rightNode;

        if (ParentNode != null)
        {
            dto.LeftNode.ParentNode = ParentNode;
            dto.RightNode.ParentNode = ParentNode;
        }

        return dto;
    }

    public int SearchMedianValue(DataTransferObject<TValue> dto)
    {
        if (CountCellInNode % 2 == 0)
        {
            if (dto.Value.CompareTo(FirstCellInNode.Value) == -1)
            {
                return (CountCellInNode / 2) - 1;
            }

            else if (dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(CountCellInNode - 1).Value) == 1 ||
                dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(CountCellInNode - 1).Value) == 0)
            {
                return CountCellInNode / 2;
            }

            else
            {
                int leftMiddleIndex = CountCellInNode / 2 - 1;
                int rightMiddleIndex = CountCellInNode / 2;
                

                if (dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(leftMiddleIndex).Value) == -1)
                {
                    return leftMiddleIndex;
                }
                else if ((dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(rightMiddleIndex).Value) == 1) ||
                    (dto.Value.CompareTo(FirstCellInNode.GetCellByIndex(rightMiddleIndex).Value) == 0))
                {
                    return rightMiddleIndex;
                }
                else
                {
                    return -1;
                }
            }
        }
        else
        {
            if (dto.Value.CompareTo(FirstCellInNode.GetCellByIndex((CountCellInNode / 2) - 1).Value) == 1 &&
                dto.Value.CompareTo(FirstCellInNode.GetCellByIndex((CountCellInNode / 2) + 1).Value) == -1)
            {
                return -1;
            }
            else
            {
                return (CountCellInNode / 2);
            }
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
        LeftNode = LeftNode;
        RightNode = RightNode;
    }
}