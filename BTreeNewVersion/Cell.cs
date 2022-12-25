using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeNewVersion;

public class Cell<TValue>
where TValue : IComparable<TValue>
{
    private TValue _value;

    public Cell(TValue value, Cell<TValue>? next = null, Node<TValue>? leftNode = null, Node<TValue>? rightNode = null)
    {
        Value = value;
        NextCell = next;
        LeftNode = leftNode;
        RightNode = rightNode;

    }

    public TValue Value { get; }
    public Cell<TValue>? NextCell { get; set; }
    public Node<TValue>? LeftNode { get; set; }
    public Node<TValue>? RightNode { get; set; }



    public int CountCell()
    {
        if (this == null)
        {
            return 0;
        }

        int count = 1;
        count += NextCell?.CountCell() ?? 0;

        return count;
    }

    public Cell<TValue> AddFirst(Cell<TValue> newCell)
    {
        newCell.NextCell = this;
        return newCell;
    }

    public Cell<TValue> AddLastCell(Cell<TValue> cell)
    {
        Cell<TValue> currentCell = this;

        while (currentCell.NextCell != null)
        {
            currentCell = currentCell.NextCell;
        }
        currentCell.NextCell = cell;
        return this;

    }

    public Cell<TValue> AddCellBeforeIndex(Cell<TValue> cell, int index)
    {
        if (index == 0)
        {
            return AddFirst(cell);
        }

        Cell<TValue> currentCell = this;
        for (int i = 1; i < index; i++)
        {
            currentCell = currentCell.NextCell;
        }

        if (currentCell.NextCell == null)
        {
            return AddLastCell(cell);
        }

        Cell<TValue> cellAfter = currentCell.NextCell;
        currentCell.NextCell = cell;
        cell.NextCell = cellAfter;
        return this;
    }

    public Cell<TValue> DeleteCellByIndex(int index)
    {
        if (index == 0)
        {
            return NextCell;
        }
        else
        {
            Cell<TValue> currentCell = this;
            for (int i = 1; i < index; i++)
            {
                currentCell = currentCell.NextCell;
            }

            currentCell.NextCell = currentCell.NextCell.NextCell;
        }
        return this;
    }
    public void DeleteAllCellAfterIndex(int index)
    {
        if (index == 0)
        {
            NextCell = null;
        }
        else
        {
            Cell<TValue> currentCell = this;
            for (int i = 1; i < index + 1; i++)
            {
                currentCell = currentCell.NextCell;
            }

            currentCell.NextCell = null;
        }
    }
    public Cell<TValue> GetCellByIndex(int index)
    {
        if (index < 0 || index > CountCell())
        {
            throw new ArgumentOutOfRangeException();
        }

        Cell<TValue> cell = this;

        for (int i = 0; i < index; i++)
        {
            cell = cell.NextCell;
        }

        return cell;
    }

    public void DeleteCellByValue(TValue value)
    {
        if (value.CompareTo(NextCell.Value) == 0)
        {
            NextCell = null;
        }
        else
        {
            Cell<TValue> currentCell = NextCell;
            while (NextCell != null)
            {
                if (value.CompareTo(currentCell.NextCell.Value) == 0)
                {
                    currentCell.NextCell = null;
                    break;
                }
                currentCell = currentCell.NextCell;
            }
        }
       
    }
}

