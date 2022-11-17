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

    public Cell(TValue value, Node<TValue>? leftNode = null, Node<TValue>? rightNode = null)
    {
        Value = value;
        LeftNode = leftNode;
        RightNode = rightNode;
    }

    public TValue Value { get; }
    public Node<TValue>? LeftNode { get; set; }
    public Node<TValue>? RightNode { get; set; }
    public Cell<TValue>? NextCell { get; private set; }


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
        currentCell.NextCell.NextCell = null;
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
}

